using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RTS4.Common;
using RTS4.Data.Serialization.Converters;
using RTS4.Data.Utility;
using Reflection = System.Reflection;

namespace RTS4.Data.Serialization {
    public interface ITypeSerializer {
        object Deserialize(SerializationContext context, XElement el);
    }

    public class TypeSerializer<T> : ITypeSerializer where T : new() {

        public class PropertyInfo {
            //public readonly SerializationName SerializationName;

            public Reflection.PropertyInfo Property;

            public string TagName { get; private set; }
            public string AttributeName { get; private set; }
            public XmlCondition[] Conditions { get; private set; }
            public ISerialConverter Converter { get; private set; }

            public string Name { get { return Property.Name; } }

            /*public PropertyInfo(string name, SerializationName serialName) {
                Name = name;
                SerializationName = serialName;
                if (serialName != null && serialName.Converter != null) {
                    Converter = serialName.Converter.GetConstructor(Type.EmptyTypes).Invoke(new object[] { }) as ISerialConverter;
                }
            }*/
            public PropertyInfo(Reflection.PropertyInfo property) {
                Property = property;
            }

            public void ExtractFrom(ElementXml el) {
                TagName = el.TagName;
                Conditions = el.Conditions;
                Debug.Assert(Converter == null || el.Converter == null);
                if (el.Converter != null) Converter = el.Converter.GetConstructor(Type.EmptyTypes).Invoke(new object[] { }) as ISerialConverter;
            }
            public void ExtractFrom(AttributeXml at) {
                AttributeName = at.AttributeName;
                Debug.Assert(Converter == null || at.Converter == null);
                if (at.Converter != null) Converter = at.Converter.GetConstructor(Type.EmptyTypes).Invoke(new object[] { }) as ISerialConverter;
            }

            public bool Test(XElement el) {
                foreach (var condition in Conditions) {
                    var attrib = el.AttributeCase(condition.Parameter);
                    if (attrib == null || attrib.Value != condition.Value) return false;
                }
                return true;
            }
        }

        public readonly Type Type;
        public PropertyInfo[] Properties;

        public TypeSerializer() {
            Type = typeof(T);
            var properties = Type.GetProperties();
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
            foreach (var property in properties) {
                string propertyName = property.Name;
                /*SerializationName serialName = null;
                var serialNameAttribs = property.GetCustomAttributes(typeof(SerializationName), true);
                Debug.Assert(serialNameAttribs.Length <= 1, "Too many SerializationNames");
                if (serialNameAttribs.Length > 0) serialName = ((SerializationName)serialNameAttribs[0]);
                else serialName = new SerializationName(propertyName.ToLowerInvariant());*/

                var elXml = (ElementXml)property.GetCustomAttributes(typeof(ElementXml), true).FirstOrDefault();
                var atXml = (AttributeXml)property.GetCustomAttributes(typeof(AttributeXml), true).FirstOrDefault();

                if (elXml != null || atXml != null) {
                    var info = new PropertyInfo(property);
                    if (elXml != null) info.ExtractFrom(elXml);
                    if (atXml != null) info.ExtractFrom(atXml);
                    propertyInfos.Add(info);
                }
            }
            Properties = propertyInfos.ToArray();
        }

        object ITypeSerializer.Deserialize(SerializationContext context, XElement el) {
            return Deserialize(context, el);
        }
        public virtual T Deserialize(SerializationContext context, XElement root) {
            object obj = new T();
            foreach (var property in Properties) {
                IEnumerable<XElement> attribEls;
                if (property.TagName == null) {
                    attribEls = new[] { root };
                } else {
                    //attribEls = root.Elements().Where(e => string.Compare(e.Name.LocalName, property.TagName, true) == 0).Where(e => property.Test(e));
                    attribEls = root.ElementsCase(property.TagName).Where(e => property.Test(e));
                }
                
                List<object> values = new List<object>();
                if (property.AttributeName != null) {
                    foreach (var el in attribEls.SelectMany(e => e.AttributesCase(property.AttributeName))) {
                        values.Add(GetValue(context, property, el.Value));
                    }
                } else {
                    foreach (var el in attribEls) {
                        values.Add(GetValue(context, property, el));
                    }
                }
                if (values.Count > 0) {
                    AppendValues(context, obj, property.Property, values.ToArray());
                }
            }
            return (T)obj;
        }

        public static object GetValue(SerializationContext context, PropertyInfo property, string value) {
            if (property.Converter != null) {
                return property.Converter.Convert(context, value);
            } else {
                return GenericConversion.Convert(value, property.Property.PropertyType);
            }
        }
        public static object GetValue(SerializationContext context, PropertyInfo property, XElement el) {
            var propertyType = property.Property.PropertyType;
            if (propertyType.IsArray) propertyType = propertyType.GetElementType();
            if (propertyType == typeof(bool) && string.IsNullOrEmpty(el.Value)) return true;
            if (propertyType == typeof(string)) return el.Value;
            if (property.Converter != null) {
                return property.Converter.Convert(context, el.Value);
            } else if (propertyType.IsPrimitive || propertyType == typeof(XReal)) {
                return GenericConversion.Convert(el.Value, propertyType);
            } else {
                var serializer = context.GetSerializer(propertyType);
                return serializer.Deserialize(context, el);
            }
        }

        public static void AppendValues(SerializationContext context, object obj, Reflection.PropertyInfo property, params object[] values) {
            var propertyType = property.PropertyType;
            if (propertyType.IsArray && !values[0].GetType().IsArray) {
                Array oldArr = (Array)property.GetValue(obj, null);
                int oldArrL = (oldArr != null ? oldArr.Length : 0);
                Array newArr = Array.CreateInstance(propertyType.GetElementType(), oldArrL + values.Length);
                if (oldArr != null) Array.Copy(oldArr, newArr, oldArr.Length);
                for (int v = 0; v < values.Length; ++v) newArr.SetValue(values[v], oldArrL + v);
                property.SetValue(obj, newArr, null);
            } else {
                if (values.Length > 1) {
                    context.LogError(property.Name + " should not appear more than once!");
                }
                property.SetValue(obj, values[0], null);
            }
        }

    }
}

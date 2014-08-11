using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RTS4.Data.Serialization {
    public class ActionSerializer<T> : TypeSerializer<T> where T : UnitAction, new() {

        public Func<T> Generator;

        public ActionSerializer() {
            var constructor = Type.GetConstructor(Type.EmptyTypes);
            Generator = delegate {
                return constructor.Invoke(new Object[] { }) as T;
            };
        }

        public override T Deserialize(SerializationContext context, XElement root) {
            var action = Generator();
            if (action == null) return null;
            var parameters = root.Elements("param");
            foreach (var property in Properties) {
                var propertyType = property.Property.PropertyType;
                var serialName = property.TagName;
                List<object> values = new List<object>();
                foreach(var param in parameters.Where(p => string.Compare(p.Attribute("name").Value, serialName, true, CultureInfo.InvariantCulture) == 0)) {
                    values.Add(GetValue(context, property, param));
                    /*if (propertyType.IsArray) {
                        values.Add(context.GetSerializer(propertyType.GetElementType()).Deserialize(context, param));
                    } else {
                        if (propertyType == typeof(bool)) {
                            property.Property.SetValue(action, true);
                        } else {
                            property.Property.SetValue(action, context.GetSerializer(propertyType).Deserialize(context, param));
                        }
                    }*/
                }
                if (values.Count > 0) {
                    AppendValues(context, action, property.Property, values.ToArray());
                }
            }
            return base.Deserialize(context, root);
        }

    }
}

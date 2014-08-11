using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RTS4.Data.Serialization {
    public struct XmlCondition {
        public string Parameter;
        public string Value;

        public XmlCondition(string param, string value) {
            Parameter = param;
            Value = value;
        }

        public static XmlCondition[] FromStrings(params string[] conditions) {
            if (conditions == null) return null;
            return conditions.
                Where(c => c != null).
                Select(c => {
                    var split = c.Split('=');
                    Debug.Assert(split.Length == 2);
                    return new XmlCondition(split[0], split[1]);
                }).
                ToArray();
        }
    }

    /*public class SerializationName : Attribute {


        public readonly string TagName;
        public readonly string AttributeName;
        public readonly XmlCondition[] Conditions;
        public readonly Type Converter;

        public SerializationName() { }
        public SerializationName(string name) : this(name, null) {
        }
        public SerializationName(string name, string attribute, params string[] conditions) : this(name, attribute, null, conditions) {
        }
        public SerializationName(string name, string attribute, Type converter, params string[] conditions) {
            TagName = name;
            AttributeName = attribute;
            Converter = converter;
            Conditions = XmlCondition.FromStrings(conditions);
        }
    }*/

    public class ElementXml : Attribute {

        public string TagName;
        public XmlCondition[] Conditions;
        public Type Converter;

        // Just for Attribute-ness
        public string ConditionStr { get { return null; } set { Conditions = XmlCondition.FromStrings(value); } }

        public ElementXml(string tag) : this(tag, null, null) { }
        public ElementXml(string tag, string conditions) : this(tag, null, conditions) { }
        public ElementXml(string tag, Type converter) : this(tag, converter, null) { }
        public ElementXml(string tag, Type converter, string conditions) {
            TagName = tag;
            Converter = converter;
            ConditionStr = conditions;
        }
    }
    public class AttributeXml : Attribute {

        public string AttributeName;
        public Type Converter;

        public AttributeXml(string name) : this(name, null) { }
        public AttributeXml(string name, Type converter) {
            AttributeName = name;
            Converter = converter;
        }

    }
}

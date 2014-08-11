using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace RTS4.ModHQ.UI {
    public static class XmlEx {

        public static string AttributeOrNull(this XElement el, string name) {
            if (el == null) return null;
            var attr = el.Attribute(name);
            return (attr != null ? attr.Value : null);
        }
        public static string AttributeOrDefault(this XElement el, string name, string def) {
            return el.AttributeOrNull(name) ?? def;
        }

        public static string ElementOrNull(this XElement el, string name) {
            if (el == null) return null;
            var elem = el.Element(name);
            return (elem != null ? elem.GetImmediateValue() : null);
        }
        public static string ElementOrDefault(this XElement el, string name, string def) {
            return el.ElementOrNull(name) ?? def;
        }

        public static string GetImmediateValue(this XElement el) {
            string res = null;
            for (var node = el.FirstNode; node != null; node = node.NextNode) {
                if (node is XText) {
                    if (res == null) res = ((XText)node).Value.Trim();
                    else res += ((XText)node).Value.Trim();
                }
            }
            return res;
        }

    }
}

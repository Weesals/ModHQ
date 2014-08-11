using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RTS4.Common;
using RTS4.Data.Utility;

namespace RTS4.Data {
    public static class XElementExt {

        public static bool IgnoreCase = false;

        public static string AttributeOrDefault(this XElement el, XName name, string dflt) {
            if (el == null) return dflt;
            var attrib = el.AttributeCase(name);
            return attrib != null ? attrib.Value : dflt;
        }
        public static T AttributeOrDefault<T>(this XElement el, XName name, T dflt) where T : struct {
            if (el == null) return dflt;
            var attrib = el.AttributeCase(name);
            if (attrib == null) return dflt;
            if (typeof(T) == typeof(XReal)) {
                var val = new XReal(PrimitiveConverter<string, float>.Convert(attrib.Value));
                return (T)(object)val;
            }
            return PrimitiveConverter<string, T>.Convert(attrib.Value);
        }

        public static string ElementOrDefault(this XElement el, XName name, string dflt) {
            if (el == null) return dflt;
            var attrib = el.ElementCase(name);
            return attrib != null ? attrib.Value : dflt;
        }
        public static T ElementOrDefault<T>(this XElement el, XName name, T dflt) where T : struct {
            if (el == null) return dflt;
            var attrib = el.ElementCase(name);
            if (attrib == null) return dflt;
            if (typeof(T) == typeof(XReal)) {
                var val = new XReal(PrimitiveConverter<string, float>.Convert(attrib.Value));
                return (T)(object)val;
            }
            return PrimitiveConverter<string, T>.Convert(attrib.Value);
        }

        public static XElement ElementCase(this XElement el, XName name) {
            return IgnoreCase ? el.ElementIgcase(name) : el.Element(name);
        }
        public static XAttribute AttributeCase(this XElement el, XName name) {
            return IgnoreCase ? el.AttributeIgCase(name) : el.Attribute(name);
        }
        public static IEnumerable<XElement> ElementsCase(this XElement el, XName name) {
            return IgnoreCase ? el.ElementsIgcase(name) : el.Elements(name);
        }
        public static IEnumerable<XAttribute> AttributesCase(this XElement el, XName name) {
            return IgnoreCase ? el.AttributesIgcase(name) : el.Attributes(name);
        }

        public static XElement ElementIgcase(this XElement el, XName name) {
            return el.Elements().Where(e => string.Compare(e.Name.LocalName, name.LocalName, true) == 0).FirstOrDefault();
        }
        public static IEnumerable<XElement> ElementsIgcase(this XElement el, XName name) {
            return el.Elements().Where(e => string.Compare(e.Name.LocalName, name.LocalName, true) == 0);
        }
        public static XAttribute AttributeIgCase(this XElement el, XName name) {
            return el.Attributes().Where(e => string.Compare(e.Name.LocalName, name.LocalName, true) == 0).FirstOrDefault();
        }
        public static IEnumerable<XAttribute> AttributesIgcase(this XElement el, XName name) {
            return el.Attributes().Where(e => string.Compare(e.Name.LocalName, name.LocalName, true) == 0);
        }

        public struct Case : IDisposable {
            bool old;
            public Case(bool ignoreCase) {
                old = IgnoreCase;
                IgnoreCase = ignoreCase;
            }
            public void Dispose() {
                IgnoreCase = old;
            }
        }
    }
}

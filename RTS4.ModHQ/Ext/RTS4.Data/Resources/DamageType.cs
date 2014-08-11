using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Serialization;
using RTS4.Data.Serialization.Converters;

namespace RTS4.Data.Resources {
    public class DamageType {

        public enum DamageTypeE { Crush, Hack, Pierce };

        //[SerializationName("param", "type")]
        [AttributeXml("type")]
        public DamageTypeE Type { get; set; }

        //[SerializationName("param", "value1")]
        [AttributeXml("value1")]
        public XReal Damage { get; set; }

        //[SerializationName("param", "value2")]
        [AttributeXml("value2")]
        public XReal AreaOfEffect { get; set; }

        //[SerializationName("param", "options", typeof(StringSplitter))]
        [AttributeXml("options", typeof(StringSplitter))]
        public string[] Options { get; set; }

    }
}

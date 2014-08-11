using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Serialization;
using RTS4.Data.Serialization.Converters;

namespace RTS4.Data.Resources {
    public class DamageBonus {

        //[SerializationName("param", "type", typeof(FlagConverter))]
        [AttributeXml("type", typeof(FlagConverter))]
        public UnitFlag Flag { get; set; }

        //[SerializationName("param", "value1")]
        [AttributeXml("value1")]
        public XReal Factor { get; set; }

    }
}

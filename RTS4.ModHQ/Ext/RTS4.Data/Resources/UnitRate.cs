using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Serialization;
using RTS4.Data.Serialization.Converters;

namespace RTS4.Data.Resources {
    public class UnitRate {
        [AttributeXml("type", typeof(FlagConverter))]
        public UnitFlag Type { get; set; }

        [AttributeXml("value1")]
        public XReal Rate { get; set; }
    }

}

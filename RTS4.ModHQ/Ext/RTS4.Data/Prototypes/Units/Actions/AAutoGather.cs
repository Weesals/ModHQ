using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Serialization;

namespace RTS4.Data.Actions {
    public class AAutoGather : UnitAction {
        public class GatherRate {
            //[SerializationName("type")]
            [AttributeXml("type")]
            public ResourceType Type { get; set; }

            //[SerializationName("value1")]
            [AttributeXml("value1")]
            public float Rate { get; set; }

            public override string ToString() { return Type + "-" + Rate; }
        }

        public GatherRate[] Rate;

    }
}

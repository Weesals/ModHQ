using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Serialization;
using RTS4.Data.Serialization.Serializers;

namespace RTS4.Data.Actions {
    public class AGather : UnitAction {
        [SerializationSerializer(typeof(AGatherSerializer))]
        public class GatherType {
            //[SerializationName("type")]
            [AttributeXml("type")]
            public ResourceType Resource { get; set; }
            //[SerializationName("MaximumRange")]
            [AttributeXml("type")]
            public XReal MaximumRange { get; set; }
            //[SerializationName("Rate")]
            public XReal Rate { get; set; }
        }

        //[SerializationName(null)]
        public GatherType[] Types;

    }
}

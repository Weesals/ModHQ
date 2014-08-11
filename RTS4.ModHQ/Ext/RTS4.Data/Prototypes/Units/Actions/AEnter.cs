using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Serialization;

namespace RTS4.Data.Actions {
    public class AEnter : UnitAction {

        //[SerializationName("MaximumRange")]
        [ElementXml("param", "name=MaximumRange")]
        [AttributeXml("value1")]
        public XReal MaximumRange { get; set; }

    }
}

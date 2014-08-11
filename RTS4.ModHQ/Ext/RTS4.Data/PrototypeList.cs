using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Common;
using RTS4.Data.Serialization;

namespace RTS4.Data {
    public class PrototypeList {

        //[SerializationName("unit")]
        [ElementXml("unit")]
        public Prototype[] Prototypes { get; set; }

    }
}

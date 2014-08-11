using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Data.Serialization;

namespace RTS4.Data {
    [SerializationSerializer(typeof(ActionSerializer<>))]
    public class UnitAction {

        //[SerializationName("Persistent")]
        [ElementXml("param", "name=Persistent")]
        public bool Persistent { get; set; }

    }
}

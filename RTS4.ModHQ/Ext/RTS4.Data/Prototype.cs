using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Data.Serialization;

namespace RTS4.Data {
    [SerializationSerializer(typeof(PrototypeSerializer))]
    public class Prototype {

        //[SerializationName(null)]
        public UnitFlag[] Flags { get; private set; }

        public void RegisterToFlag(UnitFlag flag) {
            flag.AddPrototype(this);
            if (Flags == null) Flags = new[] { flag };
            else Flags = Flags.Concat(new[] { flag }).ToArray();
        }

    }
}

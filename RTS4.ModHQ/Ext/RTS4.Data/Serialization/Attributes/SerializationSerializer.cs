using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.Data.Serialization {
    public class SerializationSerializer : Attribute {

        public Type Serializer { get; private set; }

        public SerializationSerializer(Type serializer) {
            Serializer = serializer;
        }

    }
}

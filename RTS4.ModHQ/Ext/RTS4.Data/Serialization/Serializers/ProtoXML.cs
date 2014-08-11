using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RTS4.Data.Serialization {
    public class ProtoXML {

        public PrototypeList Deserialize(SerializationContext context, XElement root) {
            var serializer = new TypeSerializer<PrototypeList>();
            return serializer.Deserialize(context, root);
        }

    }
}

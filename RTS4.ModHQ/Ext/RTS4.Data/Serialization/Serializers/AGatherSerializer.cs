using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RTS4.Data.Actions;

namespace RTS4.Data.Serialization.Serializers {
    public class AGatherSerializer : ActionSerializer<AGather> {

        public override AGather Deserialize(SerializationContext context, XElement root) {
            return base.Deserialize(context, root);
        }

    }
}

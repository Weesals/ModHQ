using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS4.Data.Lists;

namespace RTS4.Data.Serialization.Converters {
    public class FlagConverter : ISerialConverter {

        public object Convert(SerializationContext context, string data) {
            var registry = context.GetService<FlagRegistry>();
            return registry.Get(data);
        }

    }
}

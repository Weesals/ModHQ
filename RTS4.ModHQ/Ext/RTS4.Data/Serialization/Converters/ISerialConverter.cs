using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.Data.Serialization.Converters {
    public interface ISerialConverter {

        object Convert(SerializationContext context, string data);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.Data.Serialization.Converters {
    public class StringSplitter : ISerialConverter {

        public char Delimiter = '|';

        public object Convert(SerializationContext context, string data) {
            return data.Split(Delimiter);
        }

    }
}

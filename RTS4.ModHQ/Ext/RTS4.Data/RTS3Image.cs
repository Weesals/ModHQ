using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS4.Data {
    public abstract class RTS3Image {

        public abstract int Width { get; }
        public abstract int Height { get; }

        public abstract byte[] Get32BitUncompressed();

    }
}

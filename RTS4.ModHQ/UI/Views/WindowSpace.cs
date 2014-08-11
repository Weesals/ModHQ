using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RTS4.ModHQ.UI.Views {
    public class WindowSpace {

        public float SourceWidth = 1024;
        public float SourceHeight = 768;

        public float Width = 1024;
        public float Height = 768;

        public float WidthRatio { get { return Width / SourceWidth; } }
        public float HeightRatio { get { return Height / SourceHeight; } }

        public Rect Transform(Rect rect) {
            return new Rect(
                rect.X * WidthRatio,
                rect.Y * HeightRatio,
                rect.Width * WidthRatio,
                rect.Height * HeightRatio
            );
        }

    }
}

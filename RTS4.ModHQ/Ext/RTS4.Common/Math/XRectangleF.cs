using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS4.Common {
    public struct XRectangleF {

        public XReal X, Y, Width, Height;

        public XRectangleF(XReal x, XReal y, XReal width, XReal height) {
            X = x; Y = y; Width = width; Height = height;
        }

        public XReal Left { get { return X; } set { Width += X - value; X = value; } }
        public XReal Right { get { return X + Width; } set { Width = value - X; } }
        public XReal Top { get { return Y; } set { Height += Y - value; Y = value; } }
        public XReal Bottom { get { return Y + Height; } set { Height = value - Y; } }

        public bool Contains(XVector2 pnt) { return pnt.X >= Left && pnt.Y >= Top && pnt.X < Right && pnt.Y < Bottom; }

        public static XRectangleF Inflate(XRectangleF rect, XReal x, XReal y) {
            rect.X -= x;
            rect.Y -= y;
            rect.Width += x * 2;
            rect.Height += y * 2;
            return rect;
        }

    }
}

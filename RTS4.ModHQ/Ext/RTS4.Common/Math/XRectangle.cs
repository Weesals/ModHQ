using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS4.Common {
    public struct XRectangle {

        public int X, Y, Width, Height;

        public XRectangle(int x, int y, int width, int height) {
            X = x; Y = y; Width = width; Height = height;
        }

        public int Left { get { return X; } set { Width += X - value; X = value; } }
        public int Right { get { return X + Width - 1; } set { Width = value - X + 1; } }
        public int Top { get { return Y; } set { Height += Y - value; Y = value; } }
        public int Bottom { get { return Y + Height - 1; } set { Height = value - Y + 1; } }

        public bool Contains(XPoint2 pnt) { return pnt.X >= Left && pnt.Y >= Top && pnt.X < Right && pnt.Y < Bottom; }

        public XRectangle Inflate(int x, int y) {
            X -= x;
            Y -= y;
            Width += x * 2;
            Height += y * 2;
            return this;
        }

        public XRectangle Inflate(int amount) {
            Inflate(amount, amount);
            return this;
        }

        public XRectangle RestrictToWithin(XRectangle other) {
            int l = Left;
            int r = Right;
            int t = Top;
            int b = Bottom;
            if (l < other.Left) l = other.Left;
            if (r > other.Right) r = other.Right;
            if (t < other.Top) t = other.Top;
            if (b > other.Bottom) b = other.Bottom;
            X = l; Y = t;
            Width = r - l + 1; Height = b - t + 1;
            return this;
        }

        public XRectangle ExpandToInclude(XPoint2 pnt) {
            int l = Left;
            int r = Right;
            int t = Top;
            int b = Bottom;
            if (pnt.X < l) l = pnt.X; if (pnt.X > r) r = pnt.X;
            if (pnt.Y < t) t = pnt.Y; if (pnt.Y > b) b = pnt.Y;
            X = l; Y = t;
            Width = r - l + 1; Height = b - t + 1;
            return new XRectangle(l, t, r - l + 1, b - t + 1);
        }

        public override string ToString() { return "(" + X + ", " + Y + ", " + Width + ", " + Height + ")"; }

        public static XRectangle Invalid { get { return new XRectangle(int.MaxValue / 2, int.MaxValue / 2, int.MinValue, int.MinValue); } }
    }
}

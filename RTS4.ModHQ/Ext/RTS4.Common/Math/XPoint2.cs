using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS4.Common {
    public struct XPoint2 {
        public int X, Y;
        public XPoint2(int x, int y) { X = x; Y = y; }

        public static XPoint2 operator *(XPoint2 p1, int s) {
            return new XPoint2(p1.X * s, p1.Y * s);
        }
        public static XPoint2 operator /(XPoint2 p1, int s) {
            return new XPoint2(p1.X / s, p1.Y / s);
        }

        public static XPoint2 operator +(XPoint2 p1, XPoint2 p2) {
            return new XPoint2(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static XPoint2 operator -(XPoint2 p1, XPoint2 p2) {
            return new XPoint2(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static bool operator ==(XPoint2 p1, XPoint2 p2) {
            return p1.X == p2.X && p1.Y == p2.Y;
        }
        public static bool operator !=(XPoint2 p1, XPoint2 p2) {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public XVector2 ToVector2() { return new XVector2(X, Y); }

        public int AbsLength() {
            return (X < 0 ? -X : X) + (Y < 0 ? -Y : Y);
        }

        public override string ToString() { return X + ", " + Y; }
        public override bool Equals(object obj) {
            if (!(obj is XPoint2)) return false;
            var other = (XPoint2)obj;
            return other.X == X && other.Y == Y;
        }
        public override int GetHashCode() {
            return X ^ (Y >> 9);
        }

        public static int Dot(XPoint2 p1, XPoint2 p2) {
            return p1.X * p2.X + p1.Y * p2.Y;
        }
        public static int Cross(XPoint2 p1, XPoint2 v2) {
            return p1.X * v2.Y - p1.Y * v2.X;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS4.Common {
    public struct XPoint3 {
        public int X, Y, Z;
        public XPoint3(int x, int y, int z) { X = x; Y = y; Z = z; }

        public static XPoint3 operator *(XPoint3 p1, int s) {
            return new XPoint3(p1.X * s, p1.Y * s, p1.Z * s);
        }
        public static XPoint3 operator /(XPoint3 p1, int s) {
            return new XPoint3(p1.X / s, p1.Y / s, p1.Z / s);
        }

        public static XPoint3 operator +(XPoint3 p1, XPoint3 p2) {
            return new XPoint3(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }
        public static XPoint3 operator -(XPoint3 p1, XPoint3 p2) {
            return new XPoint3(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static bool operator ==(XPoint3 p1, XPoint3 p2) {
            return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
        }
        public static bool operator !=(XPoint3 p1, XPoint3 p2) {
            return p1.X != p2.X || p1.Y != p2.Y || p1.Z != p2.Z;
        }

        public XVector3 ToVector3() { return new XVector3(X, Y, Z); }

        public int AbsLength() {
            return (X < 0 ? -X : X) + (Y < 0 ? -Y : Y) + (Z < 0 ? -Z : Z);
        }

        public override string ToString() {
            return X + ", " + Y + ", " + Z;
        }
        public override bool Equals(object obj) {
            if (!(obj is XPoint3)) return false;
            var other = (XPoint3)obj;
            return other.X == X && other.Y == Y && other.Z == Z;
        }
        public override int GetHashCode() {
            return X ^ (Y >> 9) ^ (Z >> 16);
        }

        public static int Dot(XPoint3 p1, XPoint3 p2) {
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
        }
        public static XPoint3 Cross(XPoint3 p1, XPoint3 v2) {
            return new XPoint3(p1.Y * v2.Z - p1.Z * v2.Y, p1.Z * v2.X - p1.X * v2.Z, p1.X * v2.Y - p1.Y * v2.X);
        }
    }

}

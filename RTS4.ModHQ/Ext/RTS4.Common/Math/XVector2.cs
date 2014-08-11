using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS4.Common {
    public struct XVector2 {

        public XReal X, Y;

        public XReal LengthSquared {
            get { return X * X + Y * Y; }
        }
        public XReal Length {
            get { return (XReal)Math.Sqrt(LengthSquared.ToFloat); }
        }

        public XVector2(XReal x, XReal y) {
            X = x; Y = y;
        }

        public static XVector2 operator +(XVector2 v1, XVector2 v2) {
            return new XVector2(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static XVector2 operator -(XVector2 v1, XVector2 v2) {
            return new XVector2(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static XVector2 operator *(XVector2 v1, XVector2 v2) {
            return new XVector2(v1.X * v2.X, v1.Y * v2.Y);
        }
        public static XVector2 operator /(XVector2 v1, XVector2 v2) {
            return new XVector2(v1.X / v2.X, v1.Y / v2.Y);
        }
        public static XVector2 operator *(XVector2 v1, XReal s) {
            return new XVector2(v1.X * s, v1.Y * s);
        }
        public static XVector2 operator /(XVector2 v1, XReal s) {
            return new XVector2(v1.X / s, v1.Y / s);
        }

        public static bool operator ==(XVector2 v1, XVector2 v2) {
            return v1.X == v2.X && v1.Y == v2.Y;
        }
        public static bool operator !=(XVector2 v1, XVector2 v2) {
            return v1.X != v2.X || v1.Y != v2.Y;
        }

        public static XReal Dot(XVector2 v1, XVector2 v2) {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
        public static XReal Cross(XVector2 v1, XVector2 v2) {
            return v1.X * v2.Y - v1.Y * v2.X;
        }
        public static XReal DistanceSquared(XVector2 v1, XVector2 v2) {
            return (v1 - v2).LengthSquared;
        }
        public static XReal Distance(XVector2 v1, XVector2 v2) {
            return (v1 - v2).Length;
        }

        public static XVector2 Lerp(XVector2 v1, XVector2 v2, XReal lerp) {
            return new XVector2(v1.X + (v2.X - v1.X) * lerp, v1.Y + (v2.Y - v1.Y) * lerp);
        }
    }
}

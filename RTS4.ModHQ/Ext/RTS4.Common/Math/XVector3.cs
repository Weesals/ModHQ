using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS4.Common {
    public struct XVector3 {

        public XReal X, Y, Z;

        public XReal LengthSquared {
            get { return X * X + Y * Y + Z * Z; }
        }
        public XReal Length {
            get { return (XReal)Math.Sqrt(LengthSquared.ToFloat); }
        }

        public XVector3(XReal x, XReal y, XReal z) {
            X = x; Y = y; Z = z;
        }

        public static XVector3 operator +(XVector3 v1, XVector3 v2) {
            return new XVector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static XVector3 operator -(XVector3 v1, XVector3 v2) {
            return new XVector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static XVector3 operator *(XVector3 v1, XVector3 v2) {
            return new XVector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }
        public static XVector3 operator /(XVector3 v1, XVector3 v2) {
            return new XVector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }
        public static XVector3 operator *(XVector3 v1, XReal s) {
            return new XVector3(v1.X * s, v1.Y * s, v1.Z * s);
        }
        public static XVector3 operator /(XVector3 v1, XReal s) {
            return new XVector3(v1.X / s, v1.Y / s, v1.Z / s);
        }

        public static bool operator ==(XVector3 v1, XVector3 v2) {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }
        public static bool operator !=(XVector3 v1, XVector3 v2) {
            return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
        }

        public static XReal Dot(XVector3 v1, XVector3 v2) {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }
        public static XVector3 Cross(XVector3 v1, XVector3 v2) {
            return new XVector3(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X
            );
        }
        public static XReal DistanceSquared(XVector3 v1, XVector3 v2) {
            return (v1 - v2).LengthSquared;
        }
        public static XReal Distance(XVector3 v1, XVector3 v2) {
            return (v1 - v2).Length;
        }

        public void Normalize() {
            XReal scale = 1 / Length;
            X *= scale; Y *= scale; Z *= scale;
        }

        public static XVector3 Transform(XVector3 pos, XMatrix t) {
            return Transform(ref pos, ref t);
        }
        public static XVector3 Transform(ref XVector3 pos, ref XMatrix t) {
            return new XVector3(
                pos.X * t.M11 + pos.Y * t.M21 + pos.Z * t.M31 + t.M41,
                pos.X * t.M12 + pos.Y * t.M22 + pos.Z * t.M32 + t.M42,
                pos.X * t.M13 + pos.Y * t.M23 + pos.Z * t.M33 + t.M43
            );
        }
        public static XVector3 TransformNormal(XVector3 pos, XMatrix t) {
            return TransformNormal(ref pos, ref t);
        }
        public static XVector3 TransformNormal(ref XVector3 pos, ref XMatrix t) {
            return new XVector3(
                pos.X * t.M11 + pos.Y * t.M21 + pos.Z * t.M31,
                pos.X * t.M12 + pos.Y * t.M22 + pos.Z * t.M32,
                pos.X * t.M13 + pos.Y * t.M23 + pos.Z * t.M33
            );
        }

    }
}

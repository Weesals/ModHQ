using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS4.Common {
    public struct XReal : IConvertible {

        float value;

        public XReal(float v) {
            value = v;
        }
        public XReal(int v) {
            value = v;
        }

        public float ToFloat { get { return (float)value; } }
        public int ToInt { get { return (int)value; } }

        public XReal Rounded { get { return new XReal((float)Math.Round(value)); } }
        public int RoundedToInt { get { return value < 0 ? (int)(value - 0.5f) : (int)(value + 0.5f); } }

        public static XReal operator -(XReal v1) { return new XReal(-v1.value); }

        public static XReal operator +(XReal v1, XReal v2) { return new XReal(v1.value + v2.value); }
        public static XReal operator -(XReal v1, XReal v2) { return new XReal(v1.value - v2.value); }
        public static XReal operator *(XReal v1, XReal v2) { return new XReal(v1.value * v2.value); }
        public static XReal operator /(XReal v1, XReal v2) { return new XReal(v1.value / v2.value); }

        public static XReal operator +(XReal v1, int v2) { return new XReal(v1.value + v2); }
        public static XReal operator -(XReal v1, int v2) { return new XReal(v1.value - v2); }
        public static XReal operator *(XReal v1, int v2) { return new XReal(v1.value * v2); }
        public static XReal operator /(XReal v1, int v2) { return new XReal(v1.value / v2); }
        public static XReal operator +(int v1, XReal v2) { return new XReal(v1 + v2.value); }
        public static XReal operator -(int v1, XReal v2) { return new XReal(v1 - v2.value); }
        public static XReal operator *(int v1, XReal v2) { return new XReal(v1 * v2.value); }
        public static XReal operator /(int v1, XReal v2) { return new XReal(v1 / v2.value); }

        public static float operator +(XReal v1, float v2) { return v1.value + v2; }
        public static float operator -(XReal v1, float v2) { return v1.value - v2; }
        public static float operator *(XReal v1, float v2) { return v1.value * v2; }
        public static float operator /(XReal v1, float v2) { return v1.value / v2; }
        public static float operator +(float v2, XReal v1) { return v1.value + v2; }
        public static float operator -(float v2, XReal v1) { return v1.value - v2; }
        public static float operator *(float v2, XReal v1) { return v1.value * v2; }
        public static float operator /(float v2, XReal v1) { return v1.value / v2; }

        public static double operator +(XReal v1, double v2) { return v1.value + v2; }
        public static double operator -(XReal v1, double v2) { return v1.value - v2; }
        public static double operator *(XReal v1, double v2) { return v1.value * v2; }
        public static double operator /(XReal v1, double v2) { return v1.value / v2; }
        public static double operator +(double v2, XReal v1) { return v1.value + v2; }
        public static double operator -(double v2, XReal v1) { return v1.value - v2; }
        public static double operator *(double v2, XReal v1) { return v1.value * v2; }
        public static double operator /(double v2, XReal v1) { return v1.value / v2; }

        public static bool operator <(XReal v1, XReal v2) { return v1.value < v2.value; }
        public static bool operator <=(XReal v1, XReal v2) { return v1.value <= v2.value; }
        public static bool operator >(XReal v1, XReal v2) { return v1.value > v2.value; }
        public static bool operator >=(XReal v1, XReal v2) { return v1.value >= v2.value; }
        public static bool operator ==(XReal v1, XReal v2) { return v1.value == v2.value; }
        public static bool operator !=(XReal v1, XReal v2) { return v1.value != v2.value; }

        public override bool Equals(object obj) {
            return (obj is XReal) && (value == ((XReal)obj).value);
        }
        public override string ToString() { return value.ToString(); }

        public static explicit operator int(XReal v) { return v.ToInt; }

        public static explicit operator XReal(double v) { return new XReal((float)v); }
        public static explicit operator XReal(float v) { return new XReal(v); }
        public static implicit operator XReal(int v) { return new XReal(v); }


        public static XReal NaN { get { return new XReal(float.NaN); } }
        public static XReal MinValue { get { return (XReal)float.MinValue; } }
        public static XReal MaxValue { get { return (XReal)float.MaxValue; } }
        public bool IsNaN { get { return float.IsNaN(value); } }


        public static XReal Parse(string data) {
            return (XReal)float.Parse(data);
        }

        public static XReal Abs(XReal v) { return (XReal)(Math.Abs(v.value)); }
        public static XReal Sqrt(XReal v) { return (XReal)(Math.Sqrt(v.value)); }
        public static XReal Pow(XReal v, XReal p) { return (XReal)(Math.Pow(v.value, p.value)); }
        public static XReal Sin(XReal v) { return (XReal)(Math.Sin(v.value)); }
        public static XReal Cos(XReal v) { return (XReal)(Math.Cos(v.value)); }
        public static XReal Tan(XReal v) { return (XReal)(Math.Tan(v.value)); }
        public static XReal Asin(XReal v) { return (XReal)(Math.Asin(v.value)); }
        public static XReal Acos(XReal v) { return (XReal)(Math.Acos(v.value)); }
        public static XReal Atan(XReal v) { return (XReal)(Math.Atan(v.value)); }
        public static XReal Atan2(XReal y, XReal x) { return (XReal)(Math.Atan2(y.value, x.value)); }

        public TypeCode GetTypeCode() {
            TypeCode tc = Type.GetTypeCode(typeof(XReal));
            return tc;
        }

        public bool ToBoolean(IFormatProvider provider) { return value != 0; }
        public byte ToByte(IFormatProvider provider) { return (byte)value; }
        public char ToChar(IFormatProvider provider) { return (char)value; }
        public DateTime ToDateTime(IFormatProvider provider) { throw new InvalidCastException(); }
        public decimal ToDecimal(IFormatProvider provider) { return (decimal)value; }
        public double ToDouble(IFormatProvider provider) { return (double)value; }
        public short ToInt16(IFormatProvider provider) { return (short)value; }
        public int ToInt32(IFormatProvider provider) { return (int)value; }
        public long ToInt64(IFormatProvider provider) { return (long)value; }
        public sbyte ToSByte(IFormatProvider provider) { return (sbyte)value; }
        public float ToSingle(IFormatProvider provider) { return (float)value; }
        public string ToString(IFormatProvider provider) { return value.ToString(); }
        public object ToType(Type conversionType, IFormatProvider provider) {
            if (conversionType == typeof(XReal)) return this;
            throw new InvalidCastException();
        }
        public ushort ToUInt16(IFormatProvider provider) { return (ushort)value; }
        public uint ToUInt32(IFormatProvider provider) { return (uint)value; }
        public ulong ToUInt64(IFormatProvider provider)  { return (ulong)value; }

        public static readonly XReal Zero = new XReal(0);
    }
}

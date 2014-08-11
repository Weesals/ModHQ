using System;
using System.Diagnostics;
using System.Reflection;
using RTS4.Common;

namespace RTS4.Data.Utility {
    struct AuxClass<T> {
        public static T ReturnSelf(T t) { return t; }
    }
    struct Operators {
        public static int Add(int i1, int i2) { return i1 + i2; }
        public static short Add(short i1, short i2) { return (short)(i1 + i2); }
    }
    struct TypeForward<T1, T2> {
        private struct IntlToSelf<T1, T2> {
            public static T1 IntlConvert(T1 v) { return v; }
            public static Func<T1, T2> Convert;
            static IntlToSelf() { Convert = Delegate.CreateDelegate(typeof(Func<T1, T2>), typeof(IntlToSelf<T1, T2>).GetMethod("IntlConvert")) as Func<T1, T2>; }
        }
        // Convert a type to itself, T1 and T2 should be the same!
        // This just prevents boxing, and allows the cast through the C# compiler
        // Essentially the same as (T2)(object)o1
        public static T2 ToSelf(T1 o1) {
            //return __refvalue( __makeref(o1),T2);
            return IntlToSelf<T1, T2>.Convert(o1);
        }
    }
    public static class PrimitiveConverter<T1, T2> {

        private static T1 IntlConvert(T1 v) { return v; }

        public delegate T2 ConvertDel(T1 t1);
        public static ConvertDel Convert;

        static PrimitiveConverter() {
            var t1 = typeof(T1);
            var t2 = typeof(T2);
            if (t1 == t2) {
                Convert = Delegate.CreateDelegate(typeof(ConvertDel), typeof(AuxClass<T1>).GetMethod("ReturnSelf")) as ConvertDel;
            } else if (t1 == typeof(XReal) || t2 == typeof(XReal)) {
                if (t1 == typeof(XReal)) {
                    Convert = (v1) => {
                        var r1 = TypeForward<T1, XReal>.ToSelf(v1);
                        if (t2 == typeof(float)) return TypeForward<float, T2>.ToSelf(r1.ToFloat);
                        if (t2 == typeof(double)) return TypeForward<double, T2>.ToSelf((double)r1.ToFloat);
                        if (t2 == typeof(int)) return TypeForward<int, T2>.ToSelf(r1.ToInt);
                        if (t2 == typeof(short)) return TypeForward<short, T2>.ToSelf((short)r1.ToInt);
                        return PrimitiveConverter<float, T2>.Convert(r1.ToFloat);
                    };
                } else {
                    Convert = (v1) => {
                        var r1 = new XReal(PrimitiveConverter<T1, float>.Convert(v1));
                        return TypeForward<XReal, T2>.ToSelf(r1);
                    };
                }
            } else if (t1.IsEnum || t2.IsEnum) {
                if (t1.IsEnum) {
                    if (t2 == typeof(string)) Convert = (v1) => { return TypeForward<string, T2>.ToSelf(v1.ToString()); };
                    else if (t2 == typeof(int)) Convert = (v1) => { return TypeForward<int, T2>.ToSelf(System.Convert.ToInt32(v1)); };
                    else throw new NotImplementedException();
                } else {
                    if (t1 == typeof(string)) Convert = (v1) => { return (T2)Enum.Parse(t2, v1.ToString(), true); };
                    else if (t1 == typeof(int)) Convert = (v1) => { return (T2)Enum.ToObject(t2, System.Convert.ToInt32(v1)); };
                    else throw new NotImplementedException();
                }
            } else if(t1 == typeof(string) && t2 == typeof(int)) {
                Convert = (v1) => {
                    var str = TypeForward<T1, string>.ToSelf(v1);
                    int res = 0;
                    if (!Int32.TryParse(str, out res)) res = (int)Math.Round(float.Parse(str));
                    return TypeForward<int, T2>.ToSelf(res);
                };
            } else {
                string typeName = null;
                if (t2 == typeof(sbyte)) typeName = "SByte";
                else if (t2 == typeof(byte)) typeName = "Byte";
                else if (t2 == typeof(short)) typeName = "Int16";
                else if (t2 == typeof(ushort)) typeName = "UInt16";
                else if (t2 == typeof(int)) typeName = "Int32";
                else if (t2 == typeof(uint)) typeName = "UInt32";
                else if (t2 == typeof(float)) typeName = "Single";
                else if (t2 == typeof(double)) typeName = "Double";
                else throw new NotImplementedException("No converter for " + t2);
                var method = typeof(System.Convert).GetMethod("To" + typeName, new[] { t1 });
                Debug.Assert(method != null, "Unable to find converter for " + t1.Name + " to " + t2.Name);
                Convert = Delegate.CreateDelegate(typeof(ConvertDel), method) as ConvertDel;
            }
        }

        public static object ConvertBoxed(object input) {
            return Convert((T1)input);
        }
    }
    public static class PrimitiveOperators<T1> {
        public delegate T1 BinaryDel(T1 t1, T1 t2);
        public static BinaryDel Add;
        static PrimitiveOperators() {
            var addMethod = typeof(Operators).GetMethod("Add", new Type[] { typeof(T1), typeof(T1) });
            string t1 = typeof(T1).Name;
            if (addMethod != null) Add = Delegate.CreateDelegate(typeof(BinaryDel), addMethod) as BinaryDel;
        }
    }

    public static class GenericConversion {
        public delegate object ConversionDel(object v1);
        public static ConversionDel GetConversion(Type from, Type to) {
            var converClass = typeof(PrimitiveConverter<,>).MakeGenericType(from, to);
            return (ConversionDel)converClass.GetField("ConvertBoxed").GetValue(null);
        }

        public static object Convert<T>(T input, Type destType) {
            var converClass = typeof(PrimitiveConverter<,>).MakeGenericType(typeof(T), destType);
            var convertMethod = converClass.GetMethod("ConvertBoxed");
            return convertMethod.Invoke(null, new object[] { input });
        }
    }

}

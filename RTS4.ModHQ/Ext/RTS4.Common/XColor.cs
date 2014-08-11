using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS4.Common {
    public class XColor {

        public byte A, R, G, B;
        public ulong Hex {
            get { return (((((((ulong)A << 8) | R) << 8) | G) << 8) | B); }
            set { A = (byte)(value >> 24); R = (byte)(value >> 16); G = (byte)(value >> 8); B = (byte)value; }
        }

        public XColor(byte r, byte g, byte b) { A = 255; R = r; G = g; B = b; }
        public XColor(byte r, byte g, byte b, byte a) { A = a; R = r; G = g; B = b; }

        public XColor Modify(float amount) {
            return new XColor(
                (byte)Math.Min(R * amount, 255),
                (byte)Math.Min(G * amount, 255),
                (byte)Math.Min(B * amount, 255),
                A
            );
        }

        public static XColor White { get { return new XColor(255, 255, 255); } }
        public static XColor LightGray { get { return new XColor(192, 192, 192); } }
        public static XColor Gray { get { return new XColor(128, 128, 128); } }
        public static XColor DarkGray { get { return new XColor(64, 64, 64); } }
        public static XColor Black { get { return new XColor(0, 0, 0); } }
        public static XColor Transparent { get { return new XColor(0, 0, 0, 0); } }
        public static XColor Red { get { return new XColor(255, 0, 0); } }
        public static XColor Green { get { return new XColor(0, 255, 0); } }
        public static XColor Blue { get { return new XColor(0, 0, 255); } }
        public static XColor Yellow { get { return new XColor(255, 255, 0); } }
        public static XColor Orange { get { return new XColor(255, 128, 0); } }
        public static XColor Brown { get { return new XColor(192, 128, 0); } }
        public static XColor Pink { get { return new XColor(255, 0, 255); } }
        public static XColor Purple { get { return new XColor(255, 0, 192); } }
        public static XColor Aqua { get { return new XColor(0, 255, 255); } }

        public static XColor Lerp(XColor c1, XColor c2, float lerp) {
            return new XColor(
                (byte)(c1.R + (c2.R - c1.R) * lerp),
                (byte)(c1.G + (c2.G - c1.G) * lerp),
                (byte)(c1.B + (c2.B - c1.B) * lerp),
                (byte)(c1.A + (c2.A - c1.A) * lerp)
            );
        }

        private static bool IsDelim(char c) {
            return c == ' ' || c == ',';
        }
        public static XColor Parse(string p) {
            XColor res = new XColor(0, 0, 0, 255);
            int i = 0;
            for(int c=0;c<4;++c) {
                int start = i;
                while (i < p.Length && !IsDelim(p[i])) ++i;
                byte val = 0;
                if (!byte.TryParse(p.Substring(start, i - start), out val)) {
                    val = c < 3 ? (byte)0 : (byte)255;
                }
                switch (c) {
                    case 0: res.R = val; break;
                    case 1: res.G = val; break;
                    case 2: res.B = val; break;
                    case 3: res.A = val; break;
                }
                while (i < p.Length && IsDelim(p[i])) ++i;
            }
            return res;
        }
    }
}

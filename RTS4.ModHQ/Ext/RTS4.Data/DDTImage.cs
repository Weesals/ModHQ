using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.Data {
    // Info from
    // http://games.build-a.com/aom/files/help.htm
    // http://aoe3.heavengames.com/cgi-bin/forums/display.cgi?action=ct&f=14,30356,,10
    // http://forum.xentax.com/viewtopic.php?f=15&t=1984&view=next
    // http://www.gamedev.net/topic/466903-set-image-in-picturebox-in-c/
    // cont+alt+shift+triple click to get source from AOMEd
    public class DDTImage : RTS3Image {

        public struct ImageHeader {
            public enum UsageE { Texture = 0, Texture2 = 1, Palette = 3, BumpMap = 6, CubeMap = 8 };
            public enum FormatE { Uncompressed32 = 1, Palette = 3, DXT1 = 4, DXT3 = 5, DXT5 = 6, Grayscale8 = 7 };

            public UsageE Usage;
            public int AlphaBits;
            public FormatE Format;
            public int MipLevels;
            public int Width, Height;
        }

        public struct Pixel {
            public byte R, G, B, A;
            public override string ToString() {
                return (((((((A << 8) + R) << 8) + G) << 8) + B)).ToString("x");
            }
            public Pixel Lerp(Pixel p2, int lerp, int denom) {
                Pixel p1 = this;
                return new Pixel() {
                    R = (byte)(p1.R + ((int)p2.R - p1.R) * lerp / denom),
                    G = (byte)(p1.G + ((int)p2.G - p1.G) * lerp / denom),
                    B = (byte)(p1.B + ((int)p2.B - p1.B) * lerp / denom),
                    A = (byte)(p1.A + ((int)p2.A - p1.A) * lerp / denom),
                };
            }
        }

        public override int Width { get { return Header.Width; } }
        public override int Height { get { return Header.Height; } }

        public ImageHeader Header;
        public Pixel[] Palette;

        public byte[] PixelData;

        private static void SerializeHeader(ref ImageHeader header, Serializer serializer) {
            byte usage = (byte)header.Usage, alpha = (byte)header.AlphaBits, format = (byte)header.Format, mips = (byte)header.MipLevels;
            if (serializer.Serialize(ref usage)) header.Usage = (ImageHeader.UsageE)usage;
            if (serializer.Serialize(ref alpha)) header.AlphaBits = alpha;
            if (serializer.Serialize(ref format)) header.Format = (ImageHeader.FormatE)format;
            if (serializer.Serialize(ref mips)) header.MipLevels = mips;
            serializer.Serialize(ref header.Width);
            serializer.Serialize(ref header.Height);
        }

        public static DDTImage Load(Stream file) {
            long beg = file.Position;
            var reader = new BinaryReader(file);
            string header = "";
            for (int h = 0; h < 3; ++h) header += reader.ReadChar();
            if (header != "RTS") throw new InvalidDataException();
            int version = (int)reader.ReadChar() - (int)'0';
            if (version == 3) {
                var img = new DDTImage();
                var serializer = new SerialReader(new BinaryReader(file));
                SerializeHeader(ref img.Header, serializer);

                int numColors = (img.Palette != null ? img.Palette.Length : 0);
                int[] paletteOffs = new int[5];
                if (img.Header.Format == ImageHeader.FormatE.Palette) {
                    // TODO: paletteOffs[img.Header.AlphaBits] = palette offset
                    serializer.Serialize(ref numColors);
                    int unknown02 = 0;
                    int palette15Off = 0;
                    serializer.Serialize(ref unknown02);
                    serializer.Serialize(ref paletteOffs[0]);       //16
                    serializer.Serialize(ref palette15Off);
                    serializer.Serialize(ref paletteOffs[1]);       //15b
                    serializer.Serialize(ref paletteOffs[4]);       //12
                }

                List<Tuple<int, int>> images = new List<Tuple<int, int>>();
                for (int l = 0; l < img.Header.MipLevels; ++l) {
                    int off = reader.ReadInt32();
                    int len = reader.ReadInt32();
                    images.Add(new Tuple<int,int>(off, len));
                }

                if (img.Header.Format == ImageHeader.FormatE.Palette) {
                    file.Seek(beg + paletteOffs[img.Header.AlphaBits], SeekOrigin.Begin);
                    byte[] paletteData = new byte[numColors * 2];
                    file.Read(paletteData, 0, paletteData.Length);

                    img.Palette = new Pixel[numColors];
                    switch (img.Header.AlphaBits) {
                        case 0: Convert565ToRGB(paletteData, 0, img.Palette); break;
                        case 1: Convert555ToRGB(paletteData, 0, img.Palette); break;
                        case 4: Convert444ToRGB(paletteData, 0, img.Palette); break;
                    }
                }

                file.Seek(beg + images[0].Item1, SeekOrigin.Begin);
                byte[] bytes = new byte[images[0].Item2];
                reader.Read(bytes, 0, bytes.Length);
                img.PixelData = bytes;
                return img;
            }
            return null;
        }

        private static void Convert565ToRGB(byte[] data, int startData, Pixel[] palette) {
            ConvertToRGB(data, startData, palette, 0, 5, 5, 6, 11, 5, 16);
        }
        private static void Convert555ToRGB(byte[] data, int startData, Pixel[] palette) {
            ConvertToRGB(data, startData, palette, 1, 5, 6, 5, 11, 5, 16);
        }
        private static void Convert444ToRGB(byte[] data, int startData, Pixel[] palette) {
            ConvertToRGB(data, startData, palette, 4, 4, 8, 4, 12, 4, 16);
        }
        private static void ConvertToRGB(byte[] data, int startData, Pixel[] palette, int rOff, int rCnt, int gOff, int gCnt, int bOff, int bCnt, int stride) {
            int rMask = ((1 << rCnt) - 1);
            int gMask = ((1 << gCnt) - 1);
            int bMask = ((1 << bCnt) - 1);
            for (int c = 0; c < palette.Length; ++c) {
                int cBit = startData * 8 + c * stride;
                int dByte = cBit / 8;
                // Will be such that the color starts at 24th bit
                Debug.Assert(dByte < data.Length,
                    "Not enough data!");
                int b1 = data[dByte + 0];
                int b2 = (dByte + 1 < data.Length ? data[dByte + 1] : 0);
                int b3 = 0;// (dByte + 2 < data.Length ? data[dByte + 2] : 0);
                if (rCnt == 4) {
                    palette[c] = new Pixel() {
                        R = (byte)((b1 << 4) & 0xf0),
                        G = (byte)((b1 << 0) & 0xf0),
                        B = (byte)((b2 << 4) & 0xf0),
                        A = 255,
                    };
                    Debug.Assert((b2 & 0xfffffff0) == 0);
                } else {
                    int dataV = ((((b3 << 8) + b2) << 8) + b1) << (cBit - dByte * 8);
                    int r = (((dataV << rOff) >> (16 - rCnt)) & rMask) * 255 / rMask;
                    int g = (((dataV << gOff) >> (16 - gCnt)) & gMask) * 255 / gMask;
                    int b = (((dataV << bOff) >> (16 - bCnt)) & bMask) * 255 / bMask;
                    palette[c] = new Pixel() {
                        R = (byte)r,
                        G = (byte)g,
                        B = (byte)b,
                        A = 255,
                    };
                    if (rCnt == 5 && gCnt == 5 && bCnt == 5) {
                        palette[c].A = ((dataV & 0x8000) == 0 ? (byte)0 : (byte)255);
                    }
                }
            }
        }


        public override byte[] Get32BitUncompressed() {
            byte[] pixels = new byte[Width * Height * 4];
            switch (Header.Format) {
                case ImageHeader.FormatE.Uncompressed32: {
                    Debug.Assert(PixelData.Length == pixels.Length,
                        "Uncompressed data is not correct size");
                    Buffer.BlockCopy(PixelData, 0, pixels, 0, pixels.Length);
                } break;
                case ImageHeader.FormatE.Palette: {
                    if (Header.AlphaBits == 4) {
                        int p = 0;
                        for (int b = 0; b < PixelData.Length; b += 6) {
                            for (int o = 0; o < 2; ++o) {
                                int aByte = b + o;
                                int cByte = b + o * 2 + 2;
                                int alphaMask = ((1 << 4) - 1);
                                byte a1 = (byte)((PixelData[aByte] >> 4) & alphaMask);
                                byte a2 = (byte)((PixelData[aByte] >> 0) & alphaMask);
                                int c1 = PixelData[cByte + 0], c2 = PixelData[cByte + 1];
                                pixels[p++] = Palette[c1].R;
                                pixels[p++] = Palette[c1].G;
                                pixels[p++] = Palette[c1].B;
                                pixels[p++] = (byte)(a1 * 255 / alphaMask);
                                pixels[p++] = Palette[c2].R;
                                pixels[p++] = Palette[c2].G;
                                pixels[p++] = Palette[c2].B;
                                pixels[p++] = (byte)(a2 * 255 / alphaMask);
                            }
                        }
                    } else {
                        for (int b = 0; b < PixelData.Length; ++b) {
                            var index = PixelData[b];
                            var col = Palette[index];
                            pixels[b * 4 + 0] = col.B;
                            pixels[b * 4 + 1] = col.G;
                            pixels[b * 4 + 2] = col.R;
                            pixels[b * 4 + 3] = col.A;
                        }
                    }
                } break;
                case ImageHeader.FormatE.Grayscale8: {
                    for (int b = 0; b < PixelData.Length; ++b) {
                        var gray = PixelData[b];
                        pixels[b * 4 + 0] = gray;
                        pixels[b * 4 + 1] = gray;
                        pixels[b * 4 + 2] = gray;
                        pixels[b * 4 + 3] = 255;
                    }
                } break;
                case ImageHeader.FormatE.DXT1:
                case ImageHeader.FormatE.DXT3:
                case ImageHeader.FormatE.DXT5: {
                    Pixel[] blockPalette = new Pixel[2];
                    Pixel[] block4Palette = new Pixel[4];
                    int stride = 8;
                    int colOff = 4;
                    int colStride = 1;
                    int alpOff = 0;
                    int alpBits = 0;
                    int alpStrideBits = 0;
                    Action<byte[], int, Pixel[]> extractor = null;
                    switch (Header.Format) {
                        case ImageHeader.FormatE.DXT1: extractor = Convert565ToRGB; stride = 8; colOff = 4; colStride = 1; break;
                        case ImageHeader.FormatE.DXT3: extractor = Convert555ToRGB; stride = 10; colOff = 6; colStride = 1; alpOff = 4; alpBits = 1; alpStrideBits = 4 * 1; break;
                        case ImageHeader.FormatE.DXT5: extractor = Convert444ToRGB; stride = 16; colOff = 6; colStride = 3; alpOff = 4; alpBits = 4; alpStrideBits = 6 * 4; break;
                    }
                    for (int x = 0; x < Width / 4; ++x) {
                        for (int y = 0; y < Height / 4; ++y) {
                            int pixStart = stride * (y * Width / 4 + x);
                            int palStart = pixStart;
                            int colStart = pixStart + colOff;
                            int alpStart = pixStart + alpOff;
                            extractor(PixelData, palStart, blockPalette);
                            block4Palette[0] = blockPalette[0];
                            block4Palette[2] = blockPalette[0].Lerp(blockPalette[1], 1, 3);
                            block4Palette[3] = blockPalette[0].Lerp(blockPalette[1], 2, 3);
                            block4Palette[1] = blockPalette[1];
                            for (int sy = 0; sy < 4; ++sy) {
                                int py = y * 4 + sy;
                                int imgOff = sy * colStride + colStart;
                                int data = PixelData[imgOff];
                                for (int sx = 0; sx < 4; ++sx) {
                                    int v = (data >> (sx * 2)) & 0x03;
                                    Pixel c = block4Palette[v];
                                    int px = x * 4 + sx;
                                    int p = (px + py * Width) * 4;
                                    pixels[p + 0] = c.B;
                                    pixels[p + 1] = c.G;
                                    pixels[p + 2] = c.R;
                                    pixels[p + 3] = c.A;
                                }
                                if (alpBits > 0) {
                                    int alpMask = (1 << alpBits) - 1;
                                    int alphaStartBit = alpStart * 8 + sy * alpStrideBits;
                                    for (int sx = 0; sx < 4; ++sx) {
                                        int alphaBit = alphaStartBit + sx * alpBits;
                                        int alphaByte = alphaBit / 8;
                                        int alphaBitOff = alphaBit - alphaByte * 8;
                                        int alphaData = ((PixelData[alphaByte + 1]) << 8) + PixelData[alphaByte];
                                        int px = x * 4 + sx;
                                        int p = (px + py * Width) * 4;
                                        pixels[p + 3] = (byte)(((alphaData >> alphaBitOff) & alpMask) * 255 / alpMask);
                                    }
                                }
                            }
                        }
                    }
                } break;
                default: return null;
            }
            // Flip the image
            for (int y = 0; y < Height / 2; ++y) {
                for (int x = 0; x < Width; ++x) {
                    int p0 = x + y * Width;
                    int p1 = x + (Height - y - 1) * Width;
                    for (int c = 0; c < 4; ++c) {
                        byte t = pixels[p0 * 4 + c];
                        pixels[p0 * 4 + c] = pixels[p1 * 4 + c];
                        pixels[p1 * 4 + c] = t;
                    }
                }
            }
            return pixels;
        }
    }
}

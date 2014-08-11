using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RTS4.Data {
    public class XMBFile {

        XDocument document;

        public XDocument GetAsXDocument() {
            return document;
        }

        /*private static string ReadString16(BinaryReader reader) {
            int len = reader.ReadInt32();
            string str = "";
            for (int i = 0; i < len; ++i) str += (char)reader.ReadUInt16();
            return str;
        }
        private static void WriteString16(BinaryWriter writer, string str) {
            writer.Write((int)str.Length);
            for (int i = 0; i < str.Length; ++i) writer.Write((ushort)(char)str[i]);
        }
        private static string[] ReadStringArray(BinaryReader reader) {
            int elementC = reader.ReadInt32();
            List<string> elements = new List<string>();
            for (int e = 0; e < elementC; ++e) elements.Add(ReadString16(reader));
            return elements.ToArray();
        }
        private static void WriteStringArray(BinaryWriter writer, string[] strings) {
            writer.Write((int)strings.Length);
            for (int e = 0; e < strings.Length; ++e) WriteString16(writer, strings[e]);
        }*/

        private static void SerializeStringArray(Serializer serializer, ref string[] strings, int version) {
            int len = (strings != null ? strings.Length : 0);
            if (serializer.Serialize(ref len)) strings = new string[len];
            if (version == 0) {
                short wutshort = 0; serializer.Serialize(ref wutshort);
            }
            for (int s = 0; s < len; ++s) {
                serializer.SerializeU16(ref strings[s]);
            }
        }



        public static XMBFile FromDocument(XDocument xdoc) {
            var xmb = new XMBFile();
            xmb.document = xdoc;
            return xmb;
        }


        private static void Serialize(XElement root, Serializer serializer) {
            using (var scope = serializer.OpenPrefixedLengthScope()) {
                byte[] buffer = new byte[2] { (byte)'X', (byte)'R' };
                serializer.SerializeFixed(buffer);
                int four = 4;
                int seven = 7;
                serializer.Serialize(ref four);
                serializer.Serialize(ref seven);

                if (seven == 0) {
                    scope.Die();
                    throw new NotImplementedException();
                }

                string[] elements = null;
                string[] parameters = null;
                if (serializer.IsWriting) {
                    List<string> elementNames = new List<string>();
                    List<string> paramNames = new List<string>();
                    foreach (var element in new[] { root }.Concat(root.Descendants())) {
                        if (!elementNames.Contains(element.Name.LocalName))
                            elementNames.Add(element.Name.LocalName);
                        foreach (var attribute in element.Attributes()) {
                            if (!paramNames.Contains(attribute.Name.LocalName))
                                paramNames.Add(attribute.Name.LocalName);
                        }
                    }
                    elements = elementNames.ToArray();
                    parameters = paramNames.ToArray();
                }
                if (seven == 0) {
                    char v0 = 'X', v1 = 'N';
                    serializer.Serialize(ref v0);
                    serializer.Serialize(ref v1);
                    byte n174 = 174;
                    byte n249 = 249;
                    serializer.Serialize(ref n174);
                    serializer.Serialize(ref n249);
                }
                /*byte[] data = new byte[128];
                for (var b = 0; b < data.Length; ++b) { serializer.Serialize(ref data[b]); }
                char[] chars = data.Select(d => (char)d).ToArray();*/
                SerializeStringArray(serializer, ref elements, seven);
                SerializeStringArray(serializer, ref parameters, seven);
                Serialize(root, serializer, elements, parameters, seven);
            }
        }
        private static void Serialize(XElement element, Serializer serializer, string[] elements, string[] parameters, int version) {
            byte[] elhead = new byte[2] { (byte)'X', (byte)'N' };
            short dummyShort = 0;
            serializer.SerializeFixed(elhead);
            using (serializer.OpenPrefixedLengthScope()) {
                //int ellen = 0;
                //serializer.Serialize(ref ellen);

                string content = string.Concat(element.Nodes().OfType<XText>().Select(t => t.Value));
                {
                    if (serializer.SerializeU16(ref content)) element.Value = content;
                    int nameId = 0;
                    if (serializer.IsWriting) nameId = elements.TakeWhile(e => e != element.Name).Count();
                    if (serializer.Serialize(ref nameId)) element.Name = elements[nameId];
                    //if (version7) serializer.Serialize(ref dummyShort);
                }

                int elParamC = element.Attributes().Count();
                if (version > 7) {
                    int lineNumber = 0;
                    serializer.Serialize(ref lineNumber);
                }
                serializer.Serialize(ref elParamC);
                var attr = element.FirstAttribute;
                for (int p = 0; p < elParamC; ++p) {
                    string name = (attr != null ? attr.Name.LocalName : "");
                    string value = (attr != null ? attr.Value : "");

                    int nameId = 0;
                    if (serializer.IsWriting) nameId = parameters.TakeWhile(prm => prm != name).Count();
                    if (serializer.Serialize(ref nameId)) name = parameters[nameId];
                    //if (version7) serializer.Serialize(ref dummyShort);

                    if (serializer.SerializeU16(ref value)) {
                        Debug.Assert(attr == null);
                        element.Add(attr = new XAttribute(name, value));
                    }
                    attr = attr.NextAttribute;
                }
                int elC = element.Elements().Count();
                serializer.Serialize(ref elC);
                var node = element.FirstNode;
                for (int e = 0; e < elC; ++e) {
                    while (node != null && !(node is XElement)) node = node.NextNode;
                    if (node == null) element.Add(node = new XElement("tmp"));
                    Serialize(node as XElement, serializer, elements, parameters, version);
                    node = node.NextNode;
                }
            }
        }

        public static Stream DecompressAndSkipHeader(Stream cmpStream) {
            char header1 = (char)cmpStream.ReadByte();
            char header2 = (char)cmpStream.ReadByte();
            int expectedLength = 0;
            MemoryStream newStream = new MemoryStream();
            if (header1 == 'l' && header2 == '3') { // l33t
                var reader = new BinaryReader(cmpStream);
                var bytes = reader.ReadBytes(2);
                expectedLength = reader.ReadInt32();
                var zlibHeader = reader.ReadBytes(2);
                using (var decompStream = new DeflateStream(cmpStream, CompressionMode.Decompress, true)) {
                    decompStream.CopyTo(newStream);
                }
                newStream.Position = 0;
                return newStream;
                header1 = (char)newStream.ReadByte();
                header2 = (char)newStream.ReadByte();
            } else {
                cmpStream.Position = 0;
                cmpStream.CopyTo(newStream);
                newStream.Position = 0;
                return newStream;
            }
            if (header1 != 'X' || header2 != '1') {
                throw new NotImplementedException();
            }
            return newStream;
        }

        public static XMBFile Load(Stream cmpStream) {
            var newStream = DecompressAndSkipHeader(cmpStream);
            string data = new StreamReader(newStream).ReadToEnd();
            var xmb = new XMBFile();
            if(data.Trim().StartsWith("<")) {
                xmb.document = XDocument.Parse(data);
            } else {
                newStream.Position = 0;
                var b0 = newStream.ReadByte();
                var b1 = newStream.ReadByte();
                XElement root = new XElement("tmp");
                var reader = new BinaryReader(newStream);
                var serializer = new SerialReader(reader);
                Serialize(root, serializer);

                /*char[] header = new char[2];
                reader.Read(header, 0, 2);
                Debug.Assert(header[0] == 'X' && header[1] == '1');
                byte[] buffer = new byte[1024];
                reader.Read(buffer, 0, 14);

                var elements = ReadStringArray(reader);
                var parameters = ReadStringArray(reader);

                XElement root = ReadElement(reader, elements, parameters);*/

                xmb.document = new XDocument(root);
            }
            return xmb;
        }
        private static XElement ReadElement(BinaryReader reader, string[] elements, string[] parameters) {
            /*reader.BaseStream.Position += 6;
            //int elhead = reader.ReadInt16();
            //int ellen = reader.ReadInt32();
            string content = ReadString16(reader);
            int nameId = reader.ReadInt32();
            XElement el = new XElement(elements[nameId]);
            el.Value = content;
            int elParamC = reader.ReadInt32();
            for (int p = 0; p < elParamC; ++p) {
                int paramId = reader.ReadInt32();
                XAttribute attr = new XAttribute(parameters[paramId], ReadString16(reader));
                el.Add(attr);
            }
            int elC = reader.ReadInt32();
            for (int e = 0; e < elC; ++e) {
                el.Add(ReadElement(reader, elements, parameters));
            }
            return el;*/
            return null;
        }

        public void Save(Stream stream, bool compress) {
            char[] header = new char[2] { 'X', '1' };
            using (var outStream = new MemoryStream()) {
                outStream.WriteByte((byte)header[0]);
                outStream.WriteByte((byte)header[1]);

                var serializer = new SerialWriter(new BinaryWriter(outStream));
                Serialize(document.Root, serializer);
                outStream.Position = 0;
                string data = new StreamReader(outStream).ReadToEnd();
                outStream.Position = 0;

                if (compress) {
                    var writer = new BinaryWriter(stream);
                    writer.Write(Encoding.UTF8.GetBytes("l33t"));
                    writer.Write((int)outStream.Length);
                    writer.Write((byte)0x78);
                    writer.Write((byte)0x9C);
                    using (var decompStream = new DeflateStream(stream, CompressionMode.Compress, true)) {
                        outStream.CopyTo(decompStream);
                        decompStream.Flush();
                    }
                } else outStream.CopyTo(stream);
            }
        }

    }
}

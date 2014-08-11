using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS4.Data {
    // Info from
    // http://sourceforge.net/p/dragonunpacker/code/631/tree/trunk/DragonUnPACKer/plugins/drivers/default/drv_default.dpr
    public class BARFile {

        public class Entry {
            public string Name;
            public int Offset;
            public int Size;
            public int Size2;

            private MemoryStream streamCache;

            public Stream Ensure(Stream inStream) {
                if (streamCache != null) {
                    streamCache.Position = 0;
                    return streamCache;
                }
                lock (inStream) {
                    var data = new byte[Size];
                    inStream.Seek(Offset, SeekOrigin.Begin);
                    inStream.Read(data, 0, data.Length);
                    return streamCache = new MemoryStream(data);
                }
            }
            public void SetData(byte[] newData) {
                streamCache = new MemoryStream(newData);
                Size = Size2 = newData.Length;
            }

            public override string ToString() {
                return Name;
            }
        }

        public byte[] Id;   // Should always be 7 0x00s?
        public int DirectoryOffset;
        public int DirectorySize;

        public string SourceFilename { get; private set; }

        List<Entry> entries = new List<Entry>();
        Stream stream;

        private bool IsPathDelim(char c) {
            return c == '/' || c == '\\';
        }
        private bool PathsEqual(string n1, string n2) {
            int c1 = 0, c2 = 0;
            while (c1 < n1.Length || c2 < n2.Length) {
                // Skip delimeters
                if (c1 < n1.Length && IsPathDelim(n1[c1])) {
                    // Both strings need to have delimiters, unless the other string has ended
                    if (c2 < n2.Length && !IsPathDelim(n2[c2])) return false;
                    // Skip delims
                    while (c1 < n1.Length && IsPathDelim(n1[c1])) ++c1;
                    while (c2 < n2.Length && IsPathDelim(n2[c2])) ++c2;
                } else if (c2 < n2.Length && IsPathDelim(n2[c2])) return false;
                // Ensure the paths are equal
                if (c1 < n1.Length && c2 < n2.Length && char.ToLower(n1[c1]) == char.ToLower(n2[c2])) {
                    c1++;
                    c2++;
                } else {
                    return false;
                }
            }
            // Didnt make it to the end
            if (c1 != n1.Length || c2 != n2.Length) return false;
            return true;
        }

        public IEnumerable<string> FilesNames {
            get { return entries.Select(e => e.Name); }
        }
        public IEnumerable<Entry> Files {
            get { return entries; }
        }
        public Stream GetFileStream(string name) {
            var entry = entries.FirstOrDefault(e => PathsEqual(e.Name, name));
            if (entry == null) return null;
            return GetFileStream(entry);
        }

        public Stream GetFileStream(Entry entry) {
            return entry.Ensure(stream);
        }

        public static BARFile Load(string filename) {
            if (!File.Exists(filename)) return null;
            var file = File.OpenRead(filename);
            var bar = new BARFile() {
                stream = file,
                SourceFilename = filename,
            };
            var reader = new BinaryReader(file);
            reader.Read(bar.Id = new byte[8], 0, 8);
            int unknown = reader.ReadInt32();
            int entryC = reader.ReadInt32();
            bar.DirectorySize = reader.ReadInt32();
            bar.DirectoryOffset = reader.ReadInt32();
            int unknown2 = reader.ReadInt32();
            file.Seek(bar.DirectoryOffset, SeekOrigin.Begin);
            int[] offsets = new int[entryC];
            for (int o = 0; o < offsets.Length; ++o) offsets[o] = reader.ReadInt32();
            for (int e = 0; e < entryC; ++e) {
                Entry entry = new Entry();
                entry.Offset = reader.ReadInt32();
                entry.Size = reader.ReadInt32();
                entry.Size2 = reader.ReadInt32();
                byte b0 = reader.ReadByte(),
                    b1 = reader.ReadByte(),
                    b2 = reader.ReadByte(),
                    b3 = reader.ReadByte();
                if (b3 != 0) file.Position += 4;
                for (var c = reader.ReadChar(); c != '\0'; c = reader.ReadChar()) {
                    entry.Name += c;
                }
                bar.entries.Add(entry);
            }
            return bar;
        }

        public void CloseStream() {
            if (stream != null) {
                for (int e = 0; e < entries.Count; ++e) {
                    entries[e].Ensure(stream);
                }
                stream.Close();
                stream = null;
            }
        }

        public void Save(Stream outFile) {
            CloseStream();
            bool ownsStream = false;
            if (outFile == null) {
                outFile = File.Create(SourceFilename);
                ownsStream = true;
            }
            Func<Entry, int> getEntryDirectorySize = (entry) => {
                return 4 + 4 + 4 + 4 + 4 + entry.Name.Length + 1;
            };
            var writer = new BinaryWriter(outFile);
            writer.Write(Id);
            writer.Write((int)0);
            writer.Write(entries.Count);
            writer.Write(entries.Sum(e => getEntryDirectorySize(e)));
            writer.Write(28 + entries.Sum(e => e.Size));
            writer.Write((int)0);
            for (int e = 0; e < entries.Count; ++e) {
                var entry = entries[e];
                entry.Offset = (int)outFile.Position;
                var entryData = GetFileStream(entry);
                entryData.CopyTo(outFile);
                Debug.Assert(entry.Offset + entry.Size == outFile.Position);
            }
            for (int e = 0; e < entries.Count; ++e) {
                int numsToGo = entries.Count - e;
                int skippedEntries = entries.Take(e).Sum(en => getEntryDirectorySize(en));
                writer.Write(skippedEntries);
            }
            for (int e = 0; e < entries.Count; ++e) {
                var entry = entries[e];
                writer.Write(entry.Offset);
                writer.Write(entry.Size);
                writer.Write(entry.Size2);
                writer.Write((int)0);
                writer.Write((int)0);
                for (int c = 0; c < entry.Name.Length; ++c) writer.Write((byte)entry.Name[c]);
                writer.Write((byte)'\0');
            }
            if (ownsStream) outFile.Close();
        }

        public void SetFileData(Entry entry, byte[] newData) {
            entry.SetData(newData);
            Save(null);
        }
    }
}

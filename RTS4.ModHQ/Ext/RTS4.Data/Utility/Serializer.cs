using System;
using System.Diagnostics;
using System.IO;

public abstract class Serializer {

    public bool IsWriting { get { return !IsReading; } }
    public abstract bool IsReading { get; }

    public int Version;

    public abstract bool Serialize(ref bool data);
    public abstract bool Serialize(ref char data);
    public abstract bool Serialize(ref short data);
    public abstract bool Serialize(ref int data);
    public abstract bool Serialize(ref long data);
    public abstract bool Serialize(ref float data);
    public abstract bool Serialize(ref string data);
    public abstract bool Serialize(ref byte data);
    public abstract bool Serialize(ref byte[] data);
    public abstract bool Serialize(ref byte[] data, ref int len);

    public bool SerializeFixed(byte[] data) {
        for (int b = 0; b < data.Length; ++b) {
            Serialize(ref data[b]);
        }
        return IsWriting;
    }

    public bool Serialize(ref string[] data) {
        int scount = (data != null ? data.Length : -1);
        bool res = Serialize(ref scount);
        if (scount >= 0) {
            if (scount != (data != null ? data.Length : 0)) data = new string[scount];
            for (int s = 0; s < scount; ++s) {
                res |= Serialize(ref data[s]);
            }
        } else data = null;
        return res;
    }

    public bool SerializeU16(ref string data) {
        int len = (data != null ? data.Length : 0);
        if (Serialize(ref len)) data = "";
        for (int c = 0; c < len; ++c) {
            char chr = (c < data.Length ? data[c] : '\0');
            if (SerializeU16(ref chr)) data += chr;
        }
        return IsReading;
    }
    public bool SerializeU16(ref char chr) {
        short v = (short)chr;
        if (Serialize(ref v)) chr = (char)v;
        return IsReading;
    }

    public bool Serialize(ref DateTime tmpDate) {
        long ticks = tmpDate.Ticks;
        if (Serialize(ref ticks)) {
            tmpDate = new DateTime(ticks);
            return true;
        }
        return false;
    }


    public struct LengthScope : IDisposable {
        internal Action OnEndScope;
        public void Die() { OnEndScope = null; }
        public void Dispose() { if (OnEndScope != null) OnEndScope(); }
    }
    public virtual LengthScope OpenPrefixedLengthScope() {
        throw new NotImplementedException();
    }
}

public class SerialWriter : Serializer {

    BinaryWriter writer;

    public SerialWriter(BinaryWriter _writer) {
        writer = _writer;
    }

    public override bool IsReading { get { return false; } }

    public override bool Serialize(ref bool data) { writer.Write(data); return false; }
    public override bool Serialize(ref char data) { writer.Write(data); return false; }
    public override bool Serialize(ref short data) { writer.Write(data); return false; }
    public override bool Serialize(ref int data) { writer.Write(data); return false; }
    public override bool Serialize(ref long data) { writer.Write(data); return false; }
    public override bool Serialize(ref float data) { writer.Write(data); return false; }
    public override bool Serialize(ref string data) { writer.Write(data != null ? data : ""); return false; }
    public override bool Serialize(ref byte data) { writer.Write(data); return false; }
    public override bool Serialize(ref byte[] data) { if (data == null) writer.Write((Int16)(-1)); else { writer.Write(data.Length); writer.Write(data); } return false; }
    public override bool Serialize(ref byte[] data, ref int len) { if (data == null) writer.Write((Int16)(-1)); else { writer.Write((Int16)len); writer.Write(data, 0, len); } return false; }
    public override Serializer.LengthScope OpenPrefixedLengthScope() {
        var origWriter = writer;
        var memStream = new MemoryStream();
        writer = new BinaryWriter(memStream);
        return new LengthScope() {
            OnEndScope = delegate {
                writer = origWriter;
                writer.Write((int)memStream.Length);
                memStream.Position = 0;
                memStream.CopyTo(writer.BaseStream);
                memStream.Close();
            },
        };
    }

}

public class SerialReader : Serializer {

    BinaryReader reader;

    public SerialReader(BinaryReader _reader) {
        reader = _reader;
    }

    public override bool IsReading { get { return true; } }

    public override bool Serialize(ref bool data) { data = reader.ReadBoolean(); return true; }
    public override bool Serialize(ref char data) { data = reader.ReadChar(); return true; }
    public override bool Serialize(ref short data) { data = reader.ReadInt16(); return true; }
    public override bool Serialize(ref int data) { data = reader.ReadInt32(); return true; }
    public override bool Serialize(ref long data) { data = reader.ReadInt64(); return true; }
    public override bool Serialize(ref float data) { data = reader.ReadSingle(); return true; }
    public override bool Serialize(ref string data) { data = reader.ReadString(); return true; }
    public override bool Serialize(ref byte data) { data = reader.ReadByte(); return true; }
    public override bool Serialize(ref byte[] data) { int len = reader.ReadInt16(); if (len >= 0) data = reader.ReadBytes(len); return true; }
    public override bool Serialize(ref byte[] data, ref int len) { len = reader.ReadInt16(); if (len >= 0) data = reader.ReadBytes(len); return true; }

    public override Serializer.LengthScope OpenPrefixedLengthScope() {
        int expectedLength = reader.ReadInt32();
        int expectedEnd = (int)reader.BaseStream.Position + expectedLength;
        return new LengthScope() {
            OnEndScope = delegate {
                Debug.Assert(expectedEnd == (int)reader.BaseStream.Position,
                    "Scope is incorrect size");
            },
        };
    }
}

using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

// Best serialisation for production will be per object serialisation to a minimum of binary data
// Each new object must have it's own serialise and deserialise methods written specifically for it
public static class ProductionSerialisationTools {
    // Better to use byte stream?
    // Or use photon's system?

    public static readonly short LENGTH_BOOL = 1;
    public static readonly short LENGTH_INT32 = 4;
    public static readonly short LENGTH_FLOAT = 4;

    public class ProductionSerialiser {

        private List<byte[]> serialisedObjects;
        private int totalLength;

        public ProductionSerialiser() {
            serialisedObjects = new List<byte[]>();
            totalLength = 0;
        }

        public void Add(bool b) {
            byte[] bx = BitConverter.GetBytes(b);
            Assert.IsTrue(bx.Length == LENGTH_BOOL);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bx);
            Add(bx);
        }

        public void Add(Int32 i) {
            byte[] bx = BitConverter.GetBytes(i);
            Assert.IsTrue(bx.Length == LENGTH_INT32);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bx);
            Add(bx);
        }

        public void Add(float f) {
            byte[] bx = BitConverter.GetBytes(f);
            Assert.IsTrue(bx.Length == LENGTH_FLOAT);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bx);
            Add(bx);
        }

        public void Add(byte[] serialisedObject) {
            serialisedObjects.Add(serialisedObject);
            totalLength += serialisedObject.Length;
        }

        public byte[] ToByteArray() {
            byte[] bx = new byte[totalLength];
            byte[] t;
            int c = 0;

            for (int i = 0; i < serialisedObjects.Count; i++) {
                t = serialisedObjects[i];
                for (int j = 0; j < t.Length; j++) {
                    bx[c + j] = t[j];
                }
                c += t.Length;
            }

            return bx;
        }

    }

    public class ProductionDeserialiser {

        private byte[] rawData;
        private int pointer;

        public ProductionDeserialiser(byte[] rawData) {
            Assert.IsNotNull(rawData);
            Assert.IsTrue(rawData.Length > 0);
            this.rawData = rawData;
            pointer = 0;
        }

        public bool DeserialiseBool() {
            bool b = BitConverter.ToBoolean(rawData, pointer);
            pointer += LENGTH_BOOL;
            return b;
        }

        public Int32 DeserialiseInt32() {
            Int32 i = BitConverter.ToInt32(rawData, pointer);
            pointer += LENGTH_INT32;
            return i;
        }

        public float DeserialiseFloat() {
            float f = BitConverter.ToSingle(rawData, pointer);
            pointer += LENGTH_FLOAT;
            return f;
        }

        public byte[] ReadOutData(int length) {
            Assert.IsTrue(pointer + length < rawData.Length);

            byte[] bx = new byte[length];
            for (int i = 0; i < length; i++) {
                bx[i] = rawData[pointer + i];
            }
            pointer += length;

            return bx;
        }

    }

    #region Base-type serialisation helper methods
    public static void SerialiseBool(this bool obj, ProductionSerialiser ps) {
        ps.Add(obj);
    }
    public static void SerialiseInt(this Int32 obj, ProductionSerialiser ps) {
        ps.Add(obj);
    }
    public static void SerialiseFloat(this float obj, ProductionSerialiser ps) {
        ps.Add(obj);
    }
    #endregion

    #region Vector2
    public static void SerialiseVector2(this Vector2 obj, ProductionSerialiser ps) {
        ps.Add(obj.x);
        ps.Add(obj.y);
    }

    public static Vector2 DeserialiseVector2(this ProductionDeserialiser pd) {
        float x = pd.DeserialiseFloat(); // Must be read out in the same order as it is added in the serialise method
        float y = pd.DeserialiseFloat();
        return new Vector2(x, y);
    }
    #endregion

    #region Vector3
    public static void SerialiseVector3(this Vector3 obj, ProductionSerialiser ps) {
        ps.Add(obj.x);
        ps.Add(obj.y);
        ps.Add(obj.z);
    }

    public static Vector3 DeserialiseVector3(this ProductionDeserialiser pd) {
        float x = pd.DeserialiseFloat();
        float y = pd.DeserialiseFloat();
        float z = pd.DeserialiseFloat();
        return new Vector3(x, y, z);
    }
    #endregion

    #region Quaternion
    public static void SerialiseQuaternion(this Quaternion obj, ProductionSerialiser ps) {
        ps.Add(obj.x);
        ps.Add(obj.y);
        ps.Add(obj.z);
        ps.Add(obj.w);
    }

    public static Quaternion DeserialiseQuaternion(this ProductionDeserialiser pd) {
        float x = pd.DeserialiseFloat();
        float y = pd.DeserialiseFloat();
        float z = pd.DeserialiseFloat();
        float w = pd.DeserialiseFloat();
        return new Quaternion(x, y, z, w);
    }
    #endregion

    #region Arbitrary Class Example
    public class ArbitraryClass {
        public Vector2 V;
        public bool B;
        public float F;
    }

    public static void SerialiseArbitraryClass(this ArbitraryClass obj, ProductionSerialiser ps) {
        obj.V.SerialiseVector2(ps);
        obj.B.SerialiseBool(ps);
        obj.F.SerialiseFloat(ps);
    }

    public static ArbitraryClass DeserialiseArbitraryClass(this ProductionDeserialiser pd) {
        Vector2 v = pd.DeserialiseVector2();
        bool b = pd.DeserialiseBool();
        float f = pd.DeserialiseFloat();
        return new ArbitraryClass() {
            V = v,
            B = b,
            F = f
        };
    }
    #endregion

}

// Best serialisation for prototyping will be one command fits all
public static class PrototypingSerialisationTools {

    public static byte[] SerialiseToJSON(this System.Object obj) {
        return Encoding.ASCII.GetBytes(JsonUtility.ToJson(obj));
    }

    public static System.Object DeserialiseFromJSON(this byte[] rawData) {
        return Encoding.ASCII.GetString(rawData);
    }

    public static T DeserialiseFromJSON<T>(this byte[] rawData) {
        return (T) DeserialiseFromJSON(rawData);
    }

    public static byte[] SerialiseToXML(this System.Object obj) {
        throw new NotImplementedException();
    }

    public static System.Object DeserialiseFromXML(this byte[] rawData) {
        throw new NotImplementedException();
    }

    public static T DeserialiseFromXML<T>(this byte[] rawData) {
        throw new NotImplementedException();
    }

    public static byte[] SerialiseToBinary(this System.Object obj) {
        if (obj == null) {
            return null;
        }

        using (var memoryStream = new MemoryStream()) {
            var binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(memoryStream, obj);

            var compressed = CompressBinary(memoryStream.ToArray());
            return compressed;
        }
    }

    public static System.Object DeserialiseFromBinary(this byte[] rawData) {
        using (MemoryStream memoryStream = new MemoryStream()) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            byte[] decompressed = DecompressBinary(rawData);

            memoryStream.Write(decompressed, 0, decompressed.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return binaryFormatter.Deserialize(memoryStream);
        }
    }

    public static T DeserialiseFromBinary<T>(this byte[] rawData) {
        return (T) DeserialiseFromBinary(rawData);
    }

    public static byte[] CompressBinary(byte[] input) {
        byte[] compressesData;

        using (var outputStream = new MemoryStream()) {
            using (var zip = new GZipStream(outputStream, CompressionMode.Compress)) {
                zip.Write(input, 0, input.Length);
            }

            compressesData = outputStream.ToArray();
        }

        return compressesData;
    }

    public static byte[] DecompressBinary(byte[] input) {
        byte[] decompressedData;

        using (var outputStream = new MemoryStream()) {
            using (var inputStream = new MemoryStream(input)) {
                using (var zip = new GZipStream(inputStream, CompressionMode.Decompress)) {
                    zip.CopyTo(outputStream);
                }
            }

            decompressedData = outputStream.ToArray();
        }

        return decompressedData;
    }

}

[Serializable]
public struct Vector2Serialiser {
    public float x, y;
    public Vector2Serialiser(float x, float y) { this.x = x; this.y = y; }
    public Vector2Serialiser(Vector2 v) { x = v.x; y = v.y; }
    public Vector2 Vector2 { get { return new Vector2(x, y); } }
    public Vector2Serialiser Add(Vector2Serialiser v) { return new Vector2Serialiser(x + v.x, y + v.y); }
    public Vector2Serialiser Subtract(Vector2Serialiser v) { return new Vector2Serialiser(x - v.x, y - v.y); }
    public override string ToString() { return Vector2.ToString(); }
}

[Serializable]
public struct Vector3Serialiser {
    public float x, y, z;
    public Vector3Serialiser(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    public Vector3Serialiser(Vector3 v) { x = v.x; y = v.y; z = v.z; }
    public Vector3 ToVector3 { get { return new Vector3(x, y, z); } }
    public Vector3Serialiser Add(Vector3Serialiser v) { return new Vector3Serialiser(x + v.x, y + v.y, z + v.z); }
    public Vector3Serialiser Subtract(Vector3Serialiser v) { return new Vector3Serialiser(x - v.x, y - v.y, z - v.z); }
    public override string ToString() { return ToVector3.ToString(); }
}

[Serializable]
public struct QuaternionSerialiser {
    public float x, y, z, w;
    public QuaternionSerialiser(Quaternion q) { x = q.x; y = q.y; z = q.z; w = q.w; }
    public Quaternion ToQuaternion { get { return new Quaternion(x, y, z, w); } }
    public override string ToString() { return ToQuaternion.ToString(); }
}
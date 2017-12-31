using System;
using System.Text;
using UnityEngine;
using ZeroFormatter;

public static class Serialisation {

    public delegate byte[] SerialiseDelegate(object obj);
    public delegate object DeserialiseDelegate(byte[] binary);

    public static SerialiseDelegate ToBinary { get; private set; }
    public static DeserialiseDelegate ToObject { get; private set; }

    public static T To<T>(byte[] binary) {
        return (T) ToObject(binary);
    }

    private static SerialisationMode mode = SerialisationMode.Uninitialised;
    public static SerialisationMode Mode => mode;

    #region Production
    private static SerialiseDelegate ToBinaryProduction = ZeroFormatterSerializer.Serialize;
    private static DeserialiseDelegate ToObjectProduction = ZFDeserialise;

    public static void InitialiseProductionSerialisation(bool tryOtherMethodOnDeserialisationFail = true) {
        mode = SerialisationMode.Production_Fast_and_Compact;

        //ZeroFormatterInitializer.Register();

        ToBinary = ToBinaryProduction;

        if (tryOtherMethodOnDeserialisationFail) {
            ToObject = binary => {
                try {
                    return ToObjectProduction(binary);
                } catch {
                    Debug.LogWarning("Failed to deserialise binary data, trying development deserialisation method...");
                    try {
                        return ToObjectDevelopment(binary);
                    } catch (Exception e) {
                        throw new Exception("Failed to deserialise data using either method: " + e);
                    }
                }
            };
        } else {
            ToObject = ToObjectProduction;
        }

    }

    private static object ZFDeserialise(byte[] binary) {
        return ZeroFormatterSerializer.Deserialize<object>(binary);
    }

    private static T ZFDeserialise<T>(byte[] binary) {
        return ZeroFormatterSerializer.Deserialize<T>(binary);
    }
    #endregion

    #region Development
    private static SerialiseDelegate ToBinaryDevelopment = JSONSerialise;
    private static DeserialiseDelegate ToObjectDevelopment = JSONDeserialise;

    public static void InitialiseDevelopmentSerialisation(bool tryOtherMethodOnDeserialisationFail = true) {
        mode = SerialisationMode.Development_Slow_Bulky_and_Human_Readable;

        ToBinary = ToBinaryDevelopment;

        if (tryOtherMethodOnDeserialisationFail) {
            ToObject = binary => {
                try {
                    return ToObjectDevelopment(binary);
                } catch {
                    Debug.LogWarning("Failed to deserialise binary data, trying production deserialisation method...");
                    try {
                        return ToObjectProduction(binary);
                    } catch (Exception e) {
                        throw new Exception("Failed to deserialise data using either method: " + e);
                    }
                }
            };
        } else {
            ToObject = ToObjectDevelopment;
        }

    }

    private static byte[] JSONSerialise(object obj) {
        return Encoding.ASCII.GetBytes(JsonUtility.ToJson(obj, true));
    }

    private static object JSONDeserialise(byte[] binary) {
        return JsonUtility.FromJson<object>(Encoding.ASCII.GetString(binary));
    }
    private static T JSONDeserialise<T>(byte[] binary) {
        return JsonUtility.FromJson<T>(Encoding.ASCII.GetString(binary));
    }
    #endregion

}


// Best serialisation for production will be per object serialisation to a minimum of binary data
// Each new object must have it's own serialise and deserialise methods written specifically for it
// TODO Replace with ZeroFormatter, does the same thing, but more dynamically, less complicated for inheritance
//public static class ProductionSerialisationTools {

//    public static readonly short LENGTH_BOOL = 1;
//    public static readonly short LENGTH_INT32 = 4;
//    public static readonly short LENGTH_FLOAT = 4;

//    public class Serialiser {

//        private List<byte[]> serialisedObjects;
//        private int totalLength;

//        public Serialiser() {
//            serialisedObjects = new List<byte[]>();
//            totalLength = 0;
//        }

//        public byte[] ExtractByteArray() {
//            byte[] bx = new byte[totalLength];
//            byte[] t;
//            int c = 0;

//            for (int i = 0; i < serialisedObjects.Count; i++) {
//                t = serialisedObjects[i];
//                for (int j = 0; j < t.Length; j++) {
//                    bx[c + j] = t[j];
//                }
//                c += t.Length;
//            }

//            return bx;
//        }

//        public void Append(byte[] preserialisedObjectOrData) {
//            serialisedObjects.Add(preserialisedObjectOrData);
//            totalLength += preserialisedObjectOrData.Length;
//        }

//        public void Append(bool b) {
//            byte[] bx = BitConverter.GetBytes(b);
//            Assert.IsTrue(bx.Length == LENGTH_BOOL);
//            if (BitConverter.IsLittleEndian)
//                Array.Reverse(bx);
//            Append(bx);
//        }

//        public void Append(Int32 i) {
//            byte[] bx = BitConverter.GetBytes(i);
//            Assert.IsTrue(bx.Length == LENGTH_INT32);
//            if (BitConverter.IsLittleEndian)
//                Array.Reverse(bx);
//            Append(bx);
//        }

//        public void Append(float f) {
//            byte[] bx = BitConverter.GetBytes(f);
//            Assert.IsTrue(bx.Length == LENGTH_FLOAT);
//            if (BitConverter.IsLittleEndian)
//                Array.Reverse(bx);
//            Append(bx);
//        }

//        public void Append(Vector2 v) {
//            Append(v.x);
//            Append(v.y);
//        }

//        // Add further serialisation methods like above as needed...

//    }

//    public static void Append(this Serialiser s, PlayerCreatedEvent e) {
//        s.Append(e.PlayerUID);
//    }

//    public static void Append(this Serialiser s, PlayerInputEvent e) {
//        s.Append(e.PlayerUID);
//        s.Append(e.OldPosition);
//        s.Append(e.NewPosition);
//        s.Append((int) e.Direction);
//    }

//    public static void Append(this Serialiser s, PlayerLeftEvent e) {
//        s.Append(e.PlayerUID);
//    }

//    // Or add further static serialisation extensions methods like above as needed...

//    public class Deserialiser {

//        private byte[] rawData;
//        private int pointer;

//        public Deserialiser(byte[] rawData) {
//            Assert.IsNotNull(rawData);
//            Assert.IsTrue(rawData.Length > 0);
//            this.rawData = rawData;
//            pointer = 0;
//        }

//        public byte[] ReadOutData(int length) {
//            Assert.IsTrue(pointer + length < rawData.Length);

//            byte[] bx = new byte[length];
//            for (int i = 0; i < length; i++) {
//                bx[i] = rawData[pointer + i];
//            }
//            pointer += length;

//            return bx;
//        }

//        public bool ExtractBool() {
//            bool b = BitConverter.ToBoolean(rawData, pointer);
//            pointer += LENGTH_BOOL;
//            return b;
//        }

//        public Int32 ExtractInt32() {
//            Int32 i = BitConverter.ToInt32(rawData, pointer);
//            pointer += LENGTH_INT32;
//            return i;
//        }

//        public float ExtractFloat() {
//            float f = BitConverter.ToSingle(rawData, pointer);
//            pointer += LENGTH_FLOAT;
//            return f;
//        }

//        public Vector2 ExtractVector2() {
//            float x = ExtractFloat(); // Must be read out in the same order as it is added in the corresponding serialise method
//            float y = ExtractFloat();
//            return new Vector2(x, y);
//        }

//        // Add further deserialisation methods like above as needed...

//    }

//    public static PlayerCreatedEvent ExtractPlayerCreatedEvent(this Deserialiser s) {
//        PlayerCreatedEvent e = new PlayerCreatedEvent();
//        e.PlayerUID = s.ExtractInt32();
//        return e;
//    }

//    public static PlayerInputEvent ExtractPlayerInputEvent(this Deserialiser s) {
//        PlayerInputEvent e = new PlayerInputEvent();
//        e.PlayerUID = s.ExtractInt32();
//        e.OldPosition = s.ExtractVector2();
//        e.NewPosition = s.ExtractVector2();
//        e.Direction = (Direction) s.ExtractInt32();
//        return e;
//    }

//    public static PlayerLeftEvent ExtractPlayerLeftEvent(this Deserialiser s) {
//        PlayerLeftEvent e = new PlayerLeftEvent();
//        e.PlayerUID = s.ExtractInt32();
//        return e;
//    }

//    // Or add further static deserialisation extensions methods like above as needed...

//}

// Best serialisation for prototyping will be one command fits all
// Very slow, but in readable format and easy to serialise anything
public static class DevelopmentSerialisationTools {

    #region JSON
    public static byte[] SerialiseToJSON(this System.Object obj) {
        return Encoding.ASCII.GetBytes(JsonUtility.ToJson(obj));
    }

    public static string SerialiseToJSONString(this System.Object obj) {
        return JsonUtility.ToJson(obj);
    }

    public static T DeserialiseFromJSON<T>(this byte[] rawData) {
        return JsonUtility.FromJson<T>(Encoding.ASCII.GetString(rawData));
    }

    public static T DeserialiseFromJSONString<T>(this string rawData) {
        return JsonUtility.FromJson<T>(rawData);
    }
    #endregion

    //public static byte[] SerialiseToBinary(this System.Object obj) {
    //    if (obj == null) {
    //        return null;
    //    }

    //    using (var memoryStream = new MemoryStream()) {
    //        var binaryFormatter = new BinaryFormatter();

    //        binaryFormatter.Serialize(memoryStream, obj);

    //        var compressed = CompressBinary(memoryStream.ToArray());
    //        return compressed;
    //    }
    //}

    //public static System.Object DeserialiseFromBinary(this byte[] rawData) {
    //    using (MemoryStream memoryStream = new MemoryStream()) {
    //        BinaryFormatter binaryFormatter = new BinaryFormatter();
    //        byte[] decompressed = DecompressBinary(rawData);

    //        memoryStream.Write(decompressed, 0, decompressed.Length);
    //        memoryStream.Seek(0, SeekOrigin.Begin);

    //        return binaryFormatter.Deserialize(memoryStream);
    //    }
    //}

    //public static T DeserialiseFromBinary<T>(this byte[] rawData) {
    //    return (T) DeserialiseFromBinary(rawData);
    //}

    //public static byte[] CompressBinary(byte[] input) {
    //    byte[] compressesData;

    //    using (var outputStream = new MemoryStream()) {
    //        using (var zip = new GZipStream(outputStream, CompressionMode.Compress)) {
    //            zip.Write(input, 0, input.Length);
    //        }

    //        compressesData = outputStream.ToArray();
    //    }

    //    return compressesData;
    //}

    //public static byte[] DecompressBinary(byte[] input) {
    //    byte[] decompressedData;

    //    using (var outputStream = new MemoryStream()) {
    //        using (var inputStream = new MemoryStream(input)) {
    //            using (var zip = new GZipStream(inputStream, CompressionMode.Decompress)) {
    //                zip.CopyTo(outputStream);
    //            }
    //        }

    //        decompressedData = outputStream.ToArray();
    //    }

    //    return decompressedData;
    //}

}

//[Serializable]
//public struct Vector2Serialiser {
//    public float x, y;
//    public Vector2Serialiser(float x, float y) { this.x = x; this.y = y; }
//    public Vector2Serialiser(Vector2 v) { x = v.x; y = v.y; }
//    public Vector2 Vector2 { get { return new Vector2(x, y); } }
//    public Vector2Serialiser Add(Vector2Serialiser v) { return new Vector2Serialiser(x + v.x, y + v.y); }
//    public Vector2Serialiser Subtract(Vector2Serialiser v) { return new Vector2Serialiser(x - v.x, y - v.y); }
//    public override string ToString() { return Vector2.ToString(); }
//}

//[Serializable]
//public struct Vector3Serialiser {
//    public float x, y, z;
//    public Vector3Serialiser(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
//    public Vector3Serialiser(Vector3 v) { x = v.x; y = v.y; z = v.z; }
//    public Vector3 ToVector3 { get { return new Vector3(x, y, z); } }
//    public Vector3Serialiser Add(Vector3Serialiser v) { return new Vector3Serialiser(x + v.x, y + v.y, z + v.z); }
//    public Vector3Serialiser Subtract(Vector3Serialiser v) { return new Vector3Serialiser(x - v.x, y - v.y, z - v.z); }
//    public override string ToString() { return ToVector3.ToString(); }
//}

//[Serializable]
//public struct QuaternionSerialiser {
//    public float x, y, z, w;
//    public QuaternionSerialiser(Quaternion q) { x = q.x; y = q.y; z = q.z; w = q.w; }
//    public Quaternion ToQuaternion { get { return new Quaternion(x, y, z, w); } }
//    public override string ToString() { return ToQuaternion.ToString(); }
//}
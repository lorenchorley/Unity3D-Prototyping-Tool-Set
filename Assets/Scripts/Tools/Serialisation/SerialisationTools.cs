using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using ZeroFormatter;

public static class Serialisation {

    // See Basic Entity Management example for serialisation examples and patterns

    #region Production
    // Best serialisation for production will be per object serialisation to a minimum of binary data

    private static SerialiseDelegate ToBinaryProduction = ZFSerialise;
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

    private static byte[] ZFSerialise(object obj) {
        return ZeroFormatterSerializer.Serialize(BeforeSerialiseObject(obj));
    }

    private static object ZFDeserialise(byte[] binary) {
        return AfterDeserialiseObject(ZeroFormatterSerializer.Deserialize<object>(binary));
    }

    private static T ZFDeserialise<T>(byte[] binary) {
        return (T) AfterDeserialiseObject(ZeroFormatterSerializer.Deserialize<T>(binary));
    }
    #endregion

    #region Development
    // Best serialisation for prototyping and development will be one command fits all, no extra customisation
    // Very slow, but (in readable format) and easy to serialise anything

    private static SerialiseDelegate ToBinaryDevelopment = BinaryFormatterSerialise;
    private static DeserialiseDelegate ToObjectDevelopment = BinaryFormatterDeserialise;

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

    private static byte[] BinaryFormatterSerialise(System.Object obj) {
        if (obj == null)
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }

    private static System.Object BinaryFormatterDeserialise(byte[] arrBytes) {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        System.Object obj = (System.Object) binForm.Deserialize(memStream);

        return obj;
    }

    private static byte[] JSONSerialise(object obj) {
        object o = BeforeSerialiseObject(obj);
        string json = JsonUtility.ToJson(o);
        return Encoding.ASCII.GetBytes(json);
    }

    private static object JSONDeserialise(byte[] binary) {
        string json = Encoding.ASCII.GetString(binary);
        object o = JsonUtility.FromJson<object>(json);
        return AfterDeserialiseObject(o);
    }
    #endregion

    public delegate byte[] SerialiseDelegate(object obj);
    public delegate object DeserialiseDelegate(byte[] binary);

    public static SerialiseDelegate ToBinary { get; private set; }
    public static DeserialiseDelegate ToObject { get; private set; }

    public static byte[] From(object obj) {
        return ToBinary(obj);
    }

    public static T To<T>(byte[] binary) {
        return (T) ToObject(binary);
    }

    private static SerialisationMode mode = SerialisationMode.Uninitialised;
    public static SerialisationMode Mode => mode;

    public static object BeforeSerialiseObject(object obj, params object[] p) {
        if (obj is ISerialisationAware) {
            (obj as ISerialisationAware).OnBeforeSerialise(p);
        } else if (obj is IEnumerable) {
            foreach (object o in (obj as IEnumerable)) {
                BeforeSerialiseObject(o, p);
            }
        }
        return obj;
    }

    public static object AfterDeserialiseObject(object obj, params object[] p) {
        if (obj is ISerialisationAware) {
            (obj as ISerialisationAware).OnAfterDeserialise(p);
        } else if (obj is IEnumerable) {
            foreach (object o in (obj as IEnumerable)) {
                AfterDeserialiseObject(o, p);
            }
        }
        return obj;
    }

}

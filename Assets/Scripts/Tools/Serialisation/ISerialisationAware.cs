
// Allows an object to prepare itself for a serialisation event, and to know when
// it has been deserialised so it can reconnect itself to the rest of the application
public interface ISerialisationAware {
    void OnAfterDeserialise(params object[] p);
    void OnBeforeSerialise(params object[] p);
}

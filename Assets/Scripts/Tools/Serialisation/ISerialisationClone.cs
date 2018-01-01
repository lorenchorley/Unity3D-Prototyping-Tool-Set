
// Allows the production a new object that contains only serialisable data
// that can later be deserialised
public interface ISerialisationCloning {
    void DeserialiseFromClonedObject(object clone);
    object CloneToSerialisedObject();
}

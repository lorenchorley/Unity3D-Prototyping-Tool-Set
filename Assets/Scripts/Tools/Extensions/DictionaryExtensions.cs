using System;
using System.Collections.Generic;

public static class DictionaryExtensions {
        
    public static Dictionary<K, V> LookupFromList<K, V>(this IEnumerable<V> enumerable, Func<V, K> getKeyForValue) {
        Dictionary<K, V> dict = new Dictionary<K, V>();

        foreach (V value in enumerable) {
            K key = getKeyForValue(value);

            if (dict.ContainsKey(key)) {
                throw new Exception("List lookup has duplicate keys: " + key.ToString());
            } else {
                dict.Add(key, value);
            }

        }

        return dict;
    }

    public static Dictionary<K, V> LookupFromList<K, E, V>(this IEnumerable<E> enumerable, Func<E, K> getKeyForEntry, Func<E, V> getValueForEntry) {
        Dictionary<K, V> dict = new Dictionary<K, V>();

        foreach (E entry in enumerable) {
            K key = getKeyForEntry(entry);
            V value = getValueForEntry(entry);

            if (dict.ContainsKey(key)) {
                throw new Exception("List lookup has duplicate keys: " + key.ToString());
            } else {
                dict.Add(key, value);
            }

        }

        return dict;
    }

}

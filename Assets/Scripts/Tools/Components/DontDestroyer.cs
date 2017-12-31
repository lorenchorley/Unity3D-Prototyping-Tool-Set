using UnityEngine;

public class DontDestroyer : MonoBehaviour {
        
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

}

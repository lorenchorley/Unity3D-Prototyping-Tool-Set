using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ButtonExtensions {

    public static void UpdateListener(this Button button, bool toAdd, UnityAction action) {
        if (toAdd)
            button.onClick.AddListener(action);
        else
            button.onClick.RemoveListener(action);
    }
    
}


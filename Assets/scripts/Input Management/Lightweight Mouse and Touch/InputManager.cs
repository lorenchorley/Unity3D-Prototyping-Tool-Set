using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour { // Try out TouchScript as a replacement for this

    public static InputManager Instance;

    [Header("Info")]
    [SerializeField]
    private List<string> EnabledKeys;
    [SerializeField] private List<string> DisabledKeys;
    [SerializeField] private bool TouchEnabled;
    [SerializeField] private bool MouseEnabled;

    [Header("Options")]
    public float KeyRepeatDelay = 0; // TODO

    private event Action Updates = delegate { };
    private Dictionary<KeyCode, StrictSignal<ButtonState>> EnabledSignalsByKeyCode;
    private Dictionary<KeyCode, StrictSignal<ButtonState>> DisabledSignalsByKeyCode;
    public MouseInputter Mouse { get; private set; }
    public TouchInputter Touch { get; private set; }

    void Awake() {
        if (Instance == null)
            Instance = this;

        EnabledSignalsByKeyCode = new Dictionary<KeyCode, StrictSignal<ButtonState>>(); // Multiple Profile support
        DisabledSignalsByKeyCode = new Dictionary<KeyCode, StrictSignal<ButtonState>>();
        Mouse = new MouseInputter();
        Touch = new TouchInputter();
        EnabledKeys = new List<string>();
        DisabledKeys = new List<string>();

        EnableKeyTracking(true); // Enable keys only by default

    }

    bool cursorInputThisFrame;
    void Update() {
        cursorInputThisFrame = false;
        Updates.Invoke();
    }

    private void MouseUpdate() {
        if (!cursorInputThisFrame)
            cursorInputThisFrame = Mouse.Update();
    }

    private void TouchUpdate() {
        if (!cursorInputThisFrame)
            cursorInputThisFrame = Touch.Update();
    }

    private void KeyUpdate() {
        foreach (KeyCode key in EnabledSignalsByKeyCode.Keys) {
            if (Input.GetKeyDown(key)) {
                EnabledSignalsByKeyCode[key].Dispatch(ButtonState.Down);
            } else if (Input.GetKey(key)) {
                EnabledSignalsByKeyCode[key].Dispatch(ButtonState.Held);
            } else if (Input.GetKeyUp(key)) {
                EnabledSignalsByKeyCode[key].Dispatch(ButtonState.Up);
            }
        }
    }

    // First one to get registered has priority over the other
    public void EnableMouseTracking(bool enable) { 
        MouseEnabled = enable;
        if (enable) {
            Updates += MouseUpdate;
            gameObject.SetActive(true);
        } else {
            Updates -= MouseUpdate;
            if (Updates.GetInvocationList().Length == 1)
                gameObject.SetActive(false);
        }
    }

    public void EnableTouchTracking(bool enable) {
        TouchEnabled = enable;
        if (enable) {
            Updates += TouchUpdate;
            gameObject.SetActive(true);
        } else {
            Updates -= TouchUpdate;
            if (Updates.GetInvocationList().Length == 1)
                gameObject.SetActive(false);
        }
    }

    public void EnableKeyTracking(bool enable) {
        if (enable) {
            Updates += KeyUpdate;
            gameObject.SetActive(true);
        } else {
            Updates -= KeyUpdate;
            if (Updates.GetInvocationList().Length == 0)
                gameObject.SetActive(false);
        }
    }

    public StrictSignal<ButtonState> GetSignalForKey(KeyCode key) {
        StrictSignal<ButtonState> signal;
        if (EnabledSignalsByKeyCode.TryGetValue(key, out signal) || DisabledSignalsByKeyCode.TryGetValue(key, out signal))
            return signal;
        else
            throw new Exception("Key " + key + " is not registered");
    }

    public void Register(KeyCode[] keys, Action<ButtonState>[] actions) {
        for (int i = 0; i < Math.Min(keys.Length, actions.Length); i++)
            Register(keys[i], actions[i]);
    }

    public void Register(KeyCode[] keys, Action<ButtonState> action) {
        for (int i = 0; i < keys.Length; i++)
            Register(keys[i], action);
    }

    public void Register(KeyCode key, Action<ButtonState> action) {
        StrictSignal<ButtonState> signal;
        if (EnabledSignalsByKeyCode.TryGetValue(key, out signal) || DisabledSignalsByKeyCode.TryGetValue(key, out signal))
            signal.AddListener(action);
        else
            throw new Exception("Key " + key + " is not registered");
    }

    public void Deregister(KeyCode[] keys, Action<ButtonState>[] actions) {
        for (int i = 0; i < Math.Min(keys.Length, actions.Length); i++)
            Deregister(keys[i], actions[i]);
    }

    public void Deregister(KeyCode[] keys, Action<ButtonState> action) {
        for (int i = 0; i < keys.Length; i++)
            Deregister(keys[i], action);
    }

    public void Deregister(KeyCode key, Action<ButtonState> action) {
        StrictSignal<ButtonState> signal;
        if (EnabledSignalsByKeyCode.TryGetValue(key, out signal) || DisabledSignalsByKeyCode.TryGetValue(key, out signal))
            signal.RemoveListener(action);
        else
            throw new Exception("Key " + key + " is not registered");
    }

    public bool IsKeyRegistered(KeyCode key) {
        return EnabledSignalsByKeyCode.ContainsKey(key) || DisabledSignalsByKeyCode.ContainsKey(key);
    }

    public bool IsKeyEnabled(KeyCode key) {
        return EnabledSignalsByKeyCode.ContainsKey(key);
    }

    public void EnableKeys(KeyCode[] keys) {
        for (int i = 0; i < keys.Length; i++)
            EnableKey(keys[i]);
    }

    public void EnableKey(KeyCode key) {

        // Remove from disabled
        StrictSignal<ButtonState> signal = null;
        if (DisabledSignalsByKeyCode.TryGetValue(key, out signal)) {
            DisabledSignalsByKeyCode.Remove(key);
            DisabledKeys.Remove(key.ToString());
        } else
            signal = new StrictSignal<ButtonState>();

        // Add to enabled
        if (!EnabledSignalsByKeyCode.ContainsKey(key)) {
            EnabledSignalsByKeyCode.Add(key, signal);
            EnabledKeys.Add(key.ToString());
        }

    }

    public void DisableKeys(KeyCode[] keys) {
        for (int i = 0; i < keys.Length; i++)
            DisableKey(keys[i]);
    }

    public void DisableKey(KeyCode key) {

        // Remove from enabled
        StrictSignal<ButtonState> signal = null;
        if (EnabledSignalsByKeyCode.TryGetValue(key, out signal)) {
            EnabledSignalsByKeyCode.Remove(key);
            EnabledKeys.Remove(key.ToString());
        } else
            signal = new StrictSignal<ButtonState>();

        // Add to disabled
        if (!DisabledSignalsByKeyCode.ContainsKey(key)) {
            DisabledSignalsByKeyCode.Add(key, signal);
            DisabledKeys.Add(key.ToString());
        }

    }

}

using System;
using UniRx;
using UnityEngine;

public class InputManagerTester : MonoBehaviour {

    public bool MouseDragging = false;
    public Vector2 MouseDragPosition;

    private InputManager IM;
    private KeyCode[] LeftSide = new KeyCode[] { KeyCode.Q, KeyCode.Z, KeyCode.S, KeyCode.D };
    private KeyCode[] RightSide = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow };
    private KeyCode[] Left = new KeyCode[] { KeyCode.Q , KeyCode.LeftArrow };
    private KeyCode[] Right = new KeyCode[] { KeyCode.D , KeyCode.RightArrow };
    private KeyCode[] Up = new KeyCode[] { KeyCode.Z , KeyCode.UpArrow };
    private KeyCode[] Down = new KeyCode[] { KeyCode.S , KeyCode.DownArrow };

    void Start() {
        IM = GetComponent<InputManager>() ?? FindObjectOfType<InputManager>();
        IM.EnableKeys(LeftSide);
        IM.EnableKeys(RightSide);
        IM.EnableMouseTracking(true);

        IM.Register(Left, b => { if (b == ButtonState.Down) RegisterDirectionInput(Vector2Int.left); });
        IM.Register(Right, b => { if (b == ButtonState.Down) RegisterDirectionInput(Vector2Int.right); });
        IM.Register(Up, b => { if (b == ButtonState.Down) RegisterDirectionInput(Vector2Int.up); });
        IM.Register(Down, b => { if (b == ButtonState.Down) RegisterDirectionInput(Vector2Int.down); });

        bool state = false;
        Observable.Interval(TimeSpan.FromSeconds(4))
                  .Select(_ => (state = !state)) // Alternate value
                  .Subscribe(b => {
                      // Swap key sets every 4 seconds, because we can
                      IM.EnableKeys(b ? LeftSide : RightSide);
                      IM.DisableKeys(b ?  RightSide : LeftSide);
                  });

        IM.Mouse.OnPrimaryAction.AddListener(pos => Debug.Log("Primary click at " + pos));
        // TODO Primary and secondary actions:
        // Mouse defaults to left and right buttons
        // Touch defaults to tap and long tap
        // Drag options Tap and drag, long tap and drag, left click drag, right click drag
        // How to deal with two finger drag, three finger drag, right mouse button drag?
        // Two finger rotate?
        // Mouse extra button click
        // Touch swipes, left right up down, tolerances, enable disable, one two three finger?
        // Listen for

        IM.Mouse.OnStartDrag.AddListener(RegisterMouseStartedDragging);
        IM.Mouse.OnDrag.AddListener(RegisterMouseDragging);
        IM.Mouse.OnEndDrag.AddListener(RegisterMouseStoppedDragging);

        IM.Mouse.OnZoom.AddListener(f => Debug.Log("Zooming: " + f));

    }

    public void RegisterDirectionInput(Vector2Int direction) {
        Debug.Log("Input for direction: " + direction);
    }

    public void RegisterMouseStartedDragging(Vector2 pos) {
        MouseDragging = true;
        MouseDragPosition = pos;
    }

    public void RegisterMouseDragging(Vector2 pos) {
        MouseDragPosition = pos;
    }

    public void RegisterMouseStoppedDragging(Vector2 pos) {
        MouseDragPosition = pos;
        MouseDragging = false;
    }

}

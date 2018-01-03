using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputterTester : MonoBehaviour {

    public Inputter inputter;

    public Text MainMessage;
    public Text Touches;

    public ScrollRect ScrollRect;
    public RectTransform Console;
    public GameObject ConsoleItemTemplate;

    private bool IsPrimaryActionOverUI() {
        if (Input.touchCount > 0) {

            for (int i = 0; i < Input.touchCount; i++) {
                Touch t = Input.touches[i];

                if (!inputter.IsPositionOverUI(t.position))
                    return false;
            }

            return true;
        } else {
            return inputter.IsPositionOverUI(Input.mousePosition);
        }
    }

    private string previousItemContent = null;
    private Text previousItemText = null;
    void Start() {

        Application.logMessageReceived +=
            (string condition, string stackTrace, LogType type) => {
                if (IsPrimaryActionOverUI())
                    return;

                if (previousItemText != null && previousItemContent == condition) {
                    previousItemText.text += "|";
                    return;
                }

                GameObject newItem = GameObject.Instantiate<GameObject>(ConsoleItemTemplate);
                Text newItemText = newItem.GetComponent<Text>();
                newItemText.text = condition + " ";
                newItem.transform.SetParent(Console);

                previousItemContent = condition;
                previousItemText = newItemText;

                switch (type) {
                case LogType.Assert:
                    newItemText.color = Color.magenta;
                    break;
                case LogType.Warning:
                    newItemText.color = Color.yellow;
                    break;
                case LogType.Error:
                    newItemText.color = Color.red;
                    break;
                case LogType.Exception:
                    newItemText.color = Color.cyan;
                    break;
                }

                if (type != LogType.Log) {
                    newItemText.text += stackTrace;
                }

                ScrollRect.ScrollToBottom();

                LayoutRebuilder.ForceRebuildLayoutImmediate(Console);
            };

        inputter = Inputter.GetPlatformAppropriateInputter();

        inputter.OnPrimaryAction.AddListener((pos) => {
            MainMessage.text = (AddType(inputter, "primary action event: " + pos));
        });
        inputter.OnStartDrag.AddListener((pos) => {
            MainMessage.text = (AddType(inputter, "start drag event: " + pos));
        });
        inputter.OnDrag.AddListener((pos) => {
            MainMessage.text = (AddType(inputter, "drag event: " + pos));
        });
        inputter.OnEndDrag.AddListener((pos) => {
            MainMessage.text = (AddType(inputter, "end drag event: " + pos));
        });
        inputter.OnZoom.AddListener((delta) => {
            MainMessage.text = (AddType(inputter, "zoom event: " + delta.ToString("0.00000")));
        });

    }

    void Update() {
        inputter.Update();
        Touches.text = "Touches: " + Input.touchCount;
    }

    private string AddType(Inputter i, string s) {
        if (i is MouseInputter) {
            return "Mouse " + s;
        } else if (i is TouchInputter) {
            return "Touch " + s;
        } else
            throw new Exception("TODO");
    }

}


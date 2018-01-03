using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.context.impl;
using strange.extensions.context.api;
using strange.framework.api;
using System;
using strange.extensions.signal.impl;
using strange.extensions.injector.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.command.api;
using strange.extensions.mediation.api;
using strange.extensions.sequencer.api;
using UnityEngine.Assertions;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "DebuggingOptions", menuName = "Context/DebuggingOptions", order = 1)]
public class ContextDebuggingOptions : ScriptableObject {

    [Header("Logging")]
    public bool LogEventDispatches = false;
    public bool LogSignalDispatches = false;
    public bool LogCommandExecutions = false;
    public bool LogBinderCreation = false; // TODO

    [Header("Naming conventions")]
    public bool IgnoreNamingConventions = false;
    public List<string> SignalNamingConventions = new List<string>(new string[] { "Returning", "Command", "Event" });
    public List<string> CommandNamingConventions = new List<string>(new string[] { "Start" });

}

#if UNITY_EDITOR
[CustomEditor(typeof(ContextDebuggingOptions))]
public class ContextDebuggingOptionsEditor : Editor {

    public bool displayBindings = false;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (displayBindings) {
            if (GUILayout.Button("Hide bindings")) {
                displayBindings = false;
            } else {
                PrintBindings();
            }
        } else {
            if (GUILayout.Button("Display bindings")) {
                displayBindings = true;
            }
        }

    }

    private void PrintBindings() {
        IContext first = Context.firstContext;

        if (first is CrossContext) {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            DisplayCrossContextBindings((first as CrossContext).injectionBinder.CrossContextBinder);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            DisplayContextBindings(first as CrossContext);

            var contexts = (first as ICrossContextCapable).Contexts;
            if (contexts != null) {
                foreach (ICrossContextCapable context in contexts) {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    DisplayContextBindings(context);
                }
            }

        }

    }

    private GUIStyle HeaderStyle {
        get {
            return new GUIStyle {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16
            };
        }
    }

    private void DisplayCrossContextBindings(IInjectionBinder binder) {
        EditorGUILayout.LabelField("Cross Context Bindings", HeaderStyle);
        ListBindingsFor(binder, null);
    }

    private void DisplayContextBindings(ICrossContextCapable context) {
        string contextName = (context as CrossContext).GetBinderName();
        EditorGUILayout.LabelField(contextName, HeaderStyle);
        ListBindingsFor(context.injectionBinder, (context as MVCSContext).commandBinder);
    }

    private void ListBindingsFor(IInjectionBinder binder, ICommandBinder commandBinder) {
        EditorGUI.indentLevel++;
        List<object> printedCommandBindings = new List<object>();
        binder.IterateAllBindings(
            (key, name, binding) => {
                List<object> boundValues = new List<object>();
                if (binding.value != null)
                    boundValues.Add(binding.value);

                IBinding commandBinding = null;
                if (commandBinder != null) {
                    Assert.IsTrue(key is Type);
                    object boundValue = binder.GetInstance(key as Type, name);
                    if (boundValue != null) {
                        commandBinding = commandBinder.GetBinding(boundValue, name);
                        if (commandBinding != null && commandBinding.value != null) {
                            Assert.IsTrue(commandBinding.value is object[]);
                            boundValues.AddRange(commandBinding.value as object[]);
                            printedCommandBindings.AddRange(commandBinding.value as object[]);
                        }
                    }
                }

                Print(key, name, boundValues);

            }
        );
        if (commandBinder != null) {
            commandBinder.IterateAllBindings(
                (key, name, binding) => {
                    if (!printedCommandBindings.Contains(binding.value)) {
                        Print(key, name, new List<object>(binding.value as object[]));
                    }
                }
            );
        }
        EditorGUI.indentLevel--;
    }

    private void Print(object key, object name, List<object> boundValues) {

        EditorGUILayout.BeginHorizontal();

        Label(KeyToString(key), KeyColor(key));

        if (HasName(name))
            Label("(" + NameToString(name) + ")", NameColor(name));

        Label("->", Color.black);

        if (boundValues.Count == 0) {
            Label(BindingToString(null), BindingColor(null));
            EditorGUILayout.EndHorizontal();
        } else if (boundValues.Count == 1) {
            Label(BindingToString(boundValues[0]), BindingColor(boundValues[0]));
            EditorGUILayout.EndHorizontal();
        } else if (boundValues.Count > 1) {
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < boundValues.Count; i++) {
                Label(BindingToString(boundValues[i]), BindingColor(boundValues[i]));
            }

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;

        }

    }

    private void Label(string name, Color color) {
        GUIStyle s = new GUIStyle();
        s.normal.textColor = color;
        //s.fixedWidth = 20;
        GUIContent label = new GUIContent(name);
        EditorGUILayout.LabelField(label, s, GUILayout.Width(GUI.skin.label.CalcSize(label).x + 5 * EditorGUI.indentLevel));
    }

    private void Label(string name) {
        EditorGUILayout.LabelField(name);
    }

    private string KeyToString(object key) {
        Type keyType = (key is Type) ? key as Type : key.GetType();

        return keyType.Name;
    }

    private Color KeyColor(object key) {
        Type keyType = (key is Type) ? key as Type : key.GetType();

        if (keyType == typeof(IEventDispatcher) ||
            keyType == typeof(IInjectionBinder) ||
            keyType == typeof(IContext) ||
            keyType == typeof(ICommandBinder) ||
            keyType == typeof(IMediationBinder) ||
            keyType == typeof(ISequencer) ||
            keyType == typeof(GameObject))
            return Color.grey;
        if (keyType == typeof(BaseSignal) || keyType.IsSubclassOf(typeof(BaseSignal)))
            return Color.blue;
        else
            return Color.black;
    }

    private bool HasName(object name) {
        return name != null;
    }

    private string NameToString(object name) {
        if (name == null)
            return "";

        if (name is Type)
            return (name as Type).Name;

        return name.ToString();
    }

    private Color NameColor(object name) {
        return Color.yellow;
    }

    private string BindingToString(object boundValue) {
        if (boundValue == null)
            return "<null binding>";

        bool bindingIsType = boundValue is Type;
        Type bindingType = bindingIsType ? boundValue as Type : boundValue.GetType();

        if (bindingIsType)
            return bindingType.Name;

        return bindingType.Name + " (Instance)";
    }

    private Color BindingColor(object boundValue) {
        if (boundValue == null)
            return Color.red;
        else
            return Color.black;
    }

}
#endif

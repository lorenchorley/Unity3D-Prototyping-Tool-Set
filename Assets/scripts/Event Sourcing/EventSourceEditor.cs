#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UniRx;
using System;
using static ProductionSerialisationTools;

namespace eventsourcing {

    [CustomEditor(typeof(EventSource))]
    public class EventSourceEditor : Editor {

        IDisposable d = null;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (Application.isPlaying && d == null && GUILayout.Button("Print events")) {
                EventSource UnityEventSource = (EventSource) target;
                PrintEventQueue(UnityEventSource);
            }

            if (d != null && GUILayout.Button("Stop printing events")) {
                d.Dispose();
                d = null;
            }

        }

        private void PrintEventQueue(EventSource ES) {
            d = ES.AllEventObservable
                .Subscribe(e => {
                    string sx = e.SerialiseToJSONString();
                    Debug.Log(e.ToString() + "\n\n" + sx + "\n\n");
                });
        }

    }
}
#endif

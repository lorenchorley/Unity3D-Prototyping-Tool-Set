#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UniRx;
using System;
using System.Text;
using eventsourcing.examples.network;

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
            ES.AllEventObservable.Dump().Subscribe();
            d = ES.NewEventObservable.Dump().Subscribe();
        }

    }
}
#endif

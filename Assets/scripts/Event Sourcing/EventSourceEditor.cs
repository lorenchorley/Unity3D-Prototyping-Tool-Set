#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UniRx;
using ZeroFormatter;

namespace eventsource {

    [CustomEditor(typeof(EventSource))]
    public class EventSourceEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (Application.isPlaying && GUILayout.Button("Print events")) {
                EventSource UnityEventSource = (EventSource) target;
                PrintEventQueue(UnityEventSource);
            }

        }

        private void PrintEventQueue(EventSource ES) {
            ES.AllEventObserable
                .Subscribe(e => {
                    byte[] bx = ZeroFormatterSerializer.Serialize(e);
                    Debug.Log(e.ToString() + "\n\n" + bx.Length);
                 });
        }

    }
}
#endif

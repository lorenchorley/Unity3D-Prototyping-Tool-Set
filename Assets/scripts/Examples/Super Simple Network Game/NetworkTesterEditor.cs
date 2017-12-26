#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace eventsource.examples.network {

    [CustomEditor(typeof(NetworkTester))]
    public class NetworkTesterEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (Application.isPlaying && GUILayout.Button("Check final positions")) {
                NetworkTester PlayerTester = (NetworkTester) target;
                PrintEventQueue(PlayerTester);
            }

            if (Application.isPlaying && GUILayout.Button("Print hashcode")) {
                NetworkTester PlayerTester = (NetworkTester) target;
                PrintHash(PlayerTester);
            }

        }
        
        private void PrintEventQueue(NetworkTester p) {
            PositionCheckProjection proj = new PositionCheckProjection();
            PlayerRegistry PlayerRegistry = p.ES.GetRegistry<PlayerRegistry>();
            proj.PlayerCount = PlayerRegistry.EntityCount;
            p.ES.ApplyProjection(proj);

            PlayerPositionQuery q = new PlayerPositionQuery();

            foreach (PlayerEntity player in PlayerRegistry.Entities) { 
                p.ES.Query(player, q);
                Debug.Log("Player " + player.UID + " position: real " + q.Position + " projected " + proj.PlayerPositionsByUID[player.UID]);
            }

        }

        private void PrintHash(NetworkTester p) {
            HashProjection proj = new HashProjection(p.ES);
            p.ES.ApplyProjection(proj);

            Debug.Log(proj.GetHashCode());

        }

    }
}
#endif

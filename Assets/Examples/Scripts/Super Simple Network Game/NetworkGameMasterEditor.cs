#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace eventsourcing.examples.network {

    [CustomEditor(typeof(NetworkGameMaster))]
    public class NetworkGameMasterEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (Application.isPlaying && GUILayout.Button("Check final positions")) {
                NetworkGameMaster PlayerTester = (NetworkGameMaster) target;
                PrintEventQueue(PlayerTester);
            }

            if (Application.isPlaying && GUILayout.Button("Print hashcode")) {
                NetworkGameMaster PlayerTester = (NetworkGameMaster) target;
                PrintHash(PlayerTester);
            }

        }
        
        private void PrintEventQueue(NetworkGameMaster p) {
            PositionCheckProjection proj = new PositionCheckProjection();
            PlayerRegistry PlayerRegistry = p.EM.GetRegistry<PlayerRegistry>();
            proj.PlayerCount = PlayerRegistry.EntityCount;
            p.ES.ApplyProjection(proj, EventStream.AllExistingEvents);

            PlayerPositionQuery q = new PlayerPositionQuery();

            foreach (PlayerEntity player in PlayerRegistry.Entities) { 
                p.EM.Query(player, q);
                Debug.Log("Player " + player.UID + " position: real " + q.Position + " projected " + proj.PlayerPositionsByUID[player.UID]);
            }

        }

        private void PrintHash(NetworkGameMaster p) {
            HashProjection proj = new HashProjection(p.ES);
            p.ES.ApplyProjection(proj, EventStream.AllExistingEvents);

            Debug.Log(proj.GetHashCode());

        }

    }
}
#endif

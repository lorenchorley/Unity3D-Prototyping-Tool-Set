using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace eventsourcing.examples.network {

    public class CatchUpProjection : IProjection {

        public List<PlayerComponent> Players;
        public Vector3[] PlayerPositions;

        public void Reset() {
            PlayerPositions = new Vector3[Players.Count];
        }
        
        public bool Process(IEvent e) {
            if (e is PlayerInputEvent) {

            } else if (e is PlayerCreatedEvent) {

            }
            return true;
        }

        public void OnFinish() {
        }

    }

}
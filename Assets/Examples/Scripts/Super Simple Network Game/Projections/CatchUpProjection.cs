using UnityEngine;
using System;
using System.Collections.Generic;

namespace eventsourcing.examples.network {

    public class CatchUpProjection : IProjection {

        public List<PlayerComponent> Players;
        public Vector3[] PlayerPositions;

        public IDisposable CancelToken { get; set; }

        public void Reset() {
            PlayerPositions = new Vector3[Players.Count];
        }
        
        public bool Process(IEvent e) {
            if (e is PlayerInputEvent) {
                throw new NotImplementedException();
            } else if (e is PlayerCreatedEvent) {
                throw new NotImplementedException();
            }
            return true;
        }

        public void OnFinish() {
        }

    }

}
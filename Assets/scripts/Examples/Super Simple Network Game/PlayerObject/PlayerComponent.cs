using UnityEngine;
using System;

namespace eventsource.examples.network {

    public class PlayerComponent : MonoBehaviour {

        public int UID;

        public EventSource ES;

        void Start() {
            ES = GameObject.FindObjectOfType<EventSource>();
        }
        
        public void RefreshPosition() {
            PlayerPositionQuery q = new PlayerPositionQuery();
            ES.Query<PlayerEntity, PlayerPositionQuery>(UID, q);
            transform.position = q.Position;
        }

    }

}
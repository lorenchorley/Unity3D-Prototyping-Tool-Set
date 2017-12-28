using UnityEngine;
using System;

namespace eventsourcing.examples.network {

    public class PlayerComponent : MonoBehaviour {

        public int UID;

        public EntityManager EM;

        void Start() {
            EM = GameObject.FindObjectOfType<EntityManager>();
        }
        
        public void RefreshPosition() {
            PlayerPositionQuery q = new PlayerPositionQuery();
            EM.Query<PlayerEntity, PlayerPositionQuery>(UID, q);
            transform.position = q.Position;
        }

    }

}
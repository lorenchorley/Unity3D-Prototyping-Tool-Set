using UnityEngine;
using System.Collections;
using entitymanagement;

namespace eventsourcing.examples.network {

    public class PlayerPositionQuery : IQuery {
        public Vector2 Position;
    }

}
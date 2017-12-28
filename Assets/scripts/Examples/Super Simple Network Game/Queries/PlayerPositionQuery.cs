using UnityEngine;
using System.Collections;

namespace eventsourcing.examples.network {

    public class PlayerPositionQuery : IQuery {
        public Vector2 Position;
    }

}
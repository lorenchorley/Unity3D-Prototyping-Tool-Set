using UnityEngine;
using System;
using ZeroFormatter;

namespace eventsource.examples.network {

    [Serializable]
    [ZeroFormattable]
    public class PlayerInputEvent : ESEvent {

        [Index(0)]
        public Direction Direction;

        [Index(1)]
        public int PlayerUID;

        [Index(2)]
        public Vector2 OldPosition;

        [Index(3)]
        public Vector2 NewPosition;

        public override string ToString() {
            return "Moved player " + PlayerUID + " in direction " + Direction.ToString() + " from " + OldPosition + " to " + NewPosition;
        }

        public override int GetHashCode() {
            return ((OldPosition + NewPosition).GetHashCode() + PlayerUID + ((int) Direction)).GetHashCode();
        }

        public override ESCommand NewDoCommand() {
            throw new NotImplementedException();
        }

        public override ESCommand NewUndoCommand() {
            throw new NotImplementedException();
        }

    }

}

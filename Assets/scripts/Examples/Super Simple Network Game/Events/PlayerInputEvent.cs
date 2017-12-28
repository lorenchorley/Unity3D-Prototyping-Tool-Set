using UnityEngine;
using System;
using ZeroFormatter;
using strange.extensions.command.api;

namespace eventsourcing.examples.network {

    [Serializable]
    public class PlayerInputEvent : IActionableEvent {

        public Direction Direction;
        public int PlayerUID;
        public Vector2 OldPosition;
        public Vector2 NewPosition;

        public override string ToString() {
            return "Moved player " + PlayerUID + " in direction " + Direction.ToString() + " from " + OldPosition + " to " + NewPosition;
        }

        public override int GetHashCode() {
            return ((OldPosition + NewPosition).GetHashCode() + PlayerUID + ((int) Direction)).GetHashCode();
        }

        public IBaseCommand NewDoCommand() {
            throw new NotImplementedException();
        }

        public IBaseCommand NewUndoCommand() {
            throw new NotImplementedException();
        }

    }

}

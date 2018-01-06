using UnityEngine;
using System;
using ZeroFormatter;
using strange.extensions.command.api;

namespace eventsourcing.examples.network {

    [Serializable]
    public class PlayerCreatedEvent : IActionableEvent {

        public int PlayerUID;

        public override string ToString() {
            return "Player created " + PlayerUID;
        }

        public override int GetHashCode() {
            return PlayerUID.GetHashCode();
        }

        public IBaseCommand NewDoCommand() {
            throw new NotImplementedException();
        }

        public IBaseCommand NewUndoCommand() {
            throw new NotImplementedException();
        }

    }

}
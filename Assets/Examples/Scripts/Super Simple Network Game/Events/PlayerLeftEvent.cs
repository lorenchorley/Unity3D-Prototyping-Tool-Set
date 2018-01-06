using UnityEngine;
using System;
using ZeroFormatter;
using strange.extensions.command.api;

namespace eventsourcing.examples.network {

    [Serializable]
    public class PlayerLeftEvent : IActionableEvent {

        public int PlayerUID;

        public override string ToString() {
            return "Player left " + PlayerUID;
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
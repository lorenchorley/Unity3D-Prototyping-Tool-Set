using UnityEngine;
using System;
using ZeroFormatter;
using strange.extensions.command.api;

namespace eventsourcing.examples.network {

    [Serializable]
    public class PlayerCreatedEvent : IActionableEvent {

        public int PlayerUID;
        public int PlayerPhotonID;

        public long CreationTime { get; set; }

        public override string ToString() {
            return "Player created " + PlayerUID;
        }

        public int GenerateHashCode() {
            return (PlayerUID + CreationTime).GetHashCode();
        }

        public IBaseCommand NewDoCommand() {
            throw new NotImplementedException();
        }

        public IBaseCommand NewUndoCommand() {
            throw new NotImplementedException();
        }

    }

}
using UnityEngine;
using System;
using ZeroFormatter;

namespace eventsource.examples.network {

    [Serializable]
    [ZeroFormattable]
    public class PlayerLeftEvent : ESEvent {

        [Index(0)]
        public int PlayerUID;

        public override string ToString() {
            return "Player left " + PlayerUID;
        }

        public override int GetHashCode() {
            return PlayerUID.GetHashCode();
        }

        public override ESCommand NewDoCommand() {
            throw new NotImplementedException();
        }

        public override ESCommand NewUndoCommand() {
            throw new NotImplementedException();
        }

    }

}
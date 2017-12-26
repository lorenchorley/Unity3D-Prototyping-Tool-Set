using UnityEngine;
using System;

namespace eventsource.examples.basic {

    [Serializable]
    public class PersonAgeChangedEvent : ESEvent {
        public int NewAge;
        public int OldAge;

        public override int GetHashCode() {
            return (OldAge + NewAge).GetHashCode();
        }

        public override ESCommand NewDoCommand() {
            throw new NotImplementedException();
        }

        public override ESCommand NewUndoCommand() {
            throw new NotImplementedException();
        }

    }

}
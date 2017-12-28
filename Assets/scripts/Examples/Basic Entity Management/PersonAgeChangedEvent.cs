using UnityEngine;
using System;
using strange.extensions.command.api;

namespace eventsourcing.examples.basic {

    [Serializable]
    public class PersonAgeChangedEvent : IEvent {
        public int NewAge;
        public int OldAge;

        public override int GetHashCode() {
            return (OldAge + NewAge).GetHashCode();
        }

        public IBaseCommand NewDoCommand() {
            throw new NotImplementedException();
        }

        public IBaseCommand NewUndoCommand() {
            throw new NotImplementedException();
        }

    }

}
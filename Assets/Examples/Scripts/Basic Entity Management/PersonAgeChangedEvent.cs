using UnityEngine;
using System;
using strange.extensions.command.api;
using eventsourcing;

namespace entitymanagement.examples.basic {

    [Serializable]
    public class PersonAgeChangedEvent : IEvent {

        public int NewAge;
        public int OldAge;

        public long CreationTime { get; set; }

        public int GenerateHashCode() {
            return (OldAge + NewAge + CreationTime).GetHashCode();
        }

        public IBaseCommand NewDoCommand() {
            return new ChangePersonAgeMod() {
                NewAge = NewAge
            };
        }

        public IBaseCommand NewUndoCommand() {
            return new ChangePersonAgeMod() {
                NewAge = OldAge
            };
        }
        
        public override string ToString() {
            return "Person Age Changed from " + OldAge + " to " + NewAge;
        }

    }

}
using eventsourcing;
using System;

namespace photon.essynchronisation {

    [Serializable]
    public class PauseEvent : IEvent {

        public long CreationTime { get; set; }

        public int GenerateHashCode() {
            return (CreationTime).GetHashCode();
        }

    }

}
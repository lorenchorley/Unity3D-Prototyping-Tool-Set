using System;

namespace photon.essynchronisation {

    [Serializable]
    public class IntervalChangedEvent : IIntervalEvent {

        public long OldInterval;
        public long NewInterval;

        public long CreationTime { get; set; }

        public int GenerateHashCode() {
            return (OldInterval + NewInterval + CreationTime).GetHashCode();
        }

    }

}
using System;

namespace photon.essynchronisation {

    [Serializable]
    public class IntervalStartedEvent : IIntervalEvent {

        public long TimeStarted;
        public long Interval;

        public long CreationTime { get; set; }

        public int GenerateHashCode() {
            return (TimeStarted + Interval + CreationTime).GetHashCode();
        }

    }

}
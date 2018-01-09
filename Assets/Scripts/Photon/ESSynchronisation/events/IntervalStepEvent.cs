using System;

namespace photon.essynchronisation {

    [Serializable]
    public class IntervalStepEvent : IIntervalEvent {

        public long Step;
        public long exactTicks; // For error estimate

        public long CreationTime { get; set; }

        public int GenerateHashCode() {
            return (Step + exactTicks + CreationTime).GetHashCode();
        }

    }

}
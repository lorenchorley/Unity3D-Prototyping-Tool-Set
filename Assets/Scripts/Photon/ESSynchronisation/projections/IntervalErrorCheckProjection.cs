using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using eventsourcing;

namespace photon.essynchronisation {

    public class IntervalErrorCheckProjection : IProjection {

        public long AverageError;
        public long GreatestError;

        private long start = -1;
        private long period;
        private List<long> errors;

        public IDisposable CancelToken { get; set; }

        public void Reset() {
            errors = new List<long>();
        }

        public bool Process(IEvent e) {
            if (e is IntervalStepEvent) {
                if (start == -1)
                    throw new Exception("ES registered start of interval before interval was started");

                IntervalStepEvent interval = e as IntervalStepEvent;

                errors.Add(Math.Abs(start + period * interval.Step - interval.exactTicks));

            } else if (e is IntervalChangedEvent) {
                IntervalChangedEvent intervalChanged = e as IntervalChangedEvent;

                period = intervalChanged.NewInterval;

            } else if (e is IntervalStartedEvent) {
                IntervalStartedEvent intervalStarted = e as IntervalStartedEvent;

                start = intervalStarted.TimeStarted;
                period = intervalStarted.Interval;

            }

            return true; // Check all events
        }

        public void OnFinish() {
            GreatestError = 0;
            AverageError = 0;

            for (int i = 0; i < errors.Count; i++) {
                long error = errors[i];

                if (error > GreatestError)
                    GreatestError = error;

                AverageError += error;

            }

            AverageError = AverageError / errors.Count;

        }

    }

}
using UniRx;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using eventsourcing;
using entitymanagement;

namespace photon.essynchronisation {

    public class IntervalTimer : MonoBehaviour {

        private class ModComparer : Comparer<IModifier> {
            public override int Compare(IModifier x, IModifier y) {
                return (int) (x.CreationTime - y.CreationTime);
            }
        }

        public event Action<long> OnStep;

        public float MarginForErrorPercentage = 0.2f;

        private EventSource ES;
        private EntityManager EM;
        private Action<IModifier, Action<IModifier>> DistributeBetweenPlayersAsyncFilter;

        private Action onInterval;
        private bool hasStarted;
        private long startTicks; // TODO Need to be able to rewind steps somehow
        private long intervalTicks;
        public float intervalMS;
        private long step;
        private TimeSpan newIntervalSpan;

        private List<IModifier> QueuedModsForThisStep;
        private List<IModifier> QueuedModsForTheNextStep;
        private List<IModifier> tmpQueueRef;
        private Comparer<IModifier> modComparer;

        public void QueueMod(IModifier mod, long targetStep) {
            if (targetStep == step) {
                QueuedModsForThisStep.Add(mod);
            } else if (targetStep == step + 1) {
                QueuedModsForTheNextStep.Add(mod);
            } else
                throw new Exception("Cannot queue mod for step with offset " + ((targetStep - step > 0) ? "+" : "") + (targetStep - step));
        }

        public void Init(EventSource ES, EntityManager EM, Action<IModifier, Action<IModifier>> DistributeBetweenPlayersAsyncFilter) {
            this.ES = ES;
            this.EM = EM;
            this.DistributeBetweenPlayersAsyncFilter = DistributeBetweenPlayersAsyncFilter;
            hasStarted = false;
            QueuedModsForThisStep = new List<IModifier>();
            QueuedModsForTheNextStep = new List<IModifier>();
            modComparer = new ModComparer();
        }

        public void StartTimer(IntervalStartedEvent started, IntervalStepEvent lastStep, IntervalChangedEvent lastIntervalChange) {
            // Determine start time
            DateTime start = new DateTime(started.TimeStarted);

            // Determine interval
            TimeSpan intervalSpan;
            if (lastIntervalChange == null)
                intervalSpan = new TimeSpan(started.Interval);
            else
                intervalSpan = new TimeSpan(lastIntervalChange.NewInterval);

            // Determine curret step
            step = lastStep.Step;
            if (step > 0)
                hasStarted = true;

            StartTimer(start, intervalSpan);
        }

        public void StartTimer(DateTime start, TimeSpan intervalSpan) {
            startTicks = start.Ticks;
            intervalTicks = intervalSpan.Ticks;
            intervalMS = intervalTicks / TimeSpan.TicksPerMillisecond;
            onInterval = standardInterval;
            StartCoroutine("IntervalLoop");
        }
        
        public void ChangeInterval(TimeSpan newIntervalSpan) {
            this.newIntervalSpan = newIntervalSpan;
            onInterval = changeInterval;
        }

        public long NextPossibleStepToTarget(float pingMS) {
            float leftMS = startTicks + step * intervalTicks; // Before end of step
            if (leftMS - pingMS > MarginForErrorPercentage * intervalMS) {
                return step;
            } else
                return step + 1;
        }

        private void changeInterval() {
            Debug.Log("Interval changed: " + intervalMS + " -> " + (newIntervalSpan.Ticks / TimeSpan.TicksPerMillisecond));

            // Register event for interval change
            ES.RegisterEvent(new IntervalChangedEvent() {
                OldInterval = intervalTicks,
                NewInterval = newIntervalSpan.Ticks
            });

            // Update interval variables
            intervalTicks = newIntervalSpan.Ticks;
            intervalMS = intervalTicks / TimeSpan.TicksPerMillisecond;

            newIntervalSpan = default(TimeSpan);

            onInterval = standardInterval;
            standardInterval();
        }

        private void standardInterval() {
            if (!ES.IsLastEventOfType<IntervalStepEvent>()) { // Don't record successive end of interval events
                ES.RegisterEvent(new IntervalStepEvent() {
                    Step = step,
                    exactTicks = DateTime.Now.Ticks
                });

                // Sort mods for this step
                QueuedModsForThisStep.Sort(modComparer);

                // Apply mods for this step, and clear the queue
                for (int i = 0; i < QueuedModsForThisStep.Count; i++) {
                    EM.ApplyMod(QueuedModsForThisStep[i], DistributeBetweenPlayersAsyncFilter);
                }
                QueuedModsForThisStep.Clear();

                // Swap queue references, bringing next step's mod queue to the current queue
                tmpQueueRef = QueuedModsForThisStep;
                QueuedModsForThisStep = QueuedModsForTheNextStep;
                QueuedModsForTheNextStep = tmpQueueRef;

            }
            OnStep.Invoke(step);
        } 

        IEnumerator IntervalLoop() {

            if (!hasStarted) {
                yield return new WaitUntil(IsOnOrAfterStartTime);

                ES.RegisterEvent(new IntervalStartedEvent() {
                    TimeStarted = startTicks,
                    Interval = intervalTicks
                });

                step = 0;
                onInterval.Invoke();
            }

            while (true) {
                yield return new WaitUntil(IsOnOrAfterStepTime);
                step++;
                onInterval.Invoke();
            }

        }

        private bool IsOnOrAfterStartTime() {
            return DateTime.Now.Ticks >= startTicks;
        }

        private bool IsOnOrAfterStepTime() {
            long nextStepInTicks = startTicks + intervalTicks * step;
            return DateTime.Now.Ticks >= nextStepInTicks;
        }

    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using UniRx;
using ZeroFormatter;

namespace eventsourcing {

    public class EventSource : MonoBehaviour {

        private ReplaySubject<IEvent> allEventObserable;
        public UniRx.IObservable<IEvent> AllEventObservable {
            get {
                return allEventObserable;
            }
        }

        private Subject<IEvent> newEventObserable;
        public UniRx.IObservable<IEvent> NewEventObserable {
            get {
                return newEventObserable;
            }
        }

        private int eventCount;
        public int EventCount {
            get {
                return eventCount;
            }
        }

        protected void Awake() {
            Reset();
        }

        public void ResetWithByteData(byte[] data) {
            IEvent[] array = ZeroFormatterSerializer.Deserialize<IEvent[]>(data);
            Reset();

            for (int i = 0; i < array.Length; i++) { // Ignore interval data for now
                RegisterEvent(array[i]);
            }

        }

        public void ExtractByteData(Action<byte[]> callback) {
            List<IEvent> list = new List<IEvent>();

            allEventObserable
                .DoOnCompleted(() => callback.Invoke(ZeroFormatterSerializer.Serialize(list.ToArray())))
                .Subscribe(list.Add);

        }

        private void Reset() {
            newEventObserable = new Subject<IEvent>();
            allEventObserable = new ReplaySubject<IEvent>();
            eventCount = 0;
        }

        public void RegisterEvent(IEvent e) {
            Assert.IsNotNull(e);
            eventCount++;
            newEventObserable.OnNext(e);
            allEventObserable.OnNext(e);
        }

        public void ApplyProjection(IProjection projection) {

            IDisposable disposable = null;
            disposable = AllEventObservable
                .DoOnSubscribe(projection.Reset)
                .DoOnCompleted(() => {
                    projection.OnFinish();
                    disposable.Dispose();
                    disposable = null;
                })
                .Subscribe(e => {
                    if (!projection.Process(e)) {
                        disposable.Dispose();
                        disposable = null;
                    }
                });

        }

    }

}
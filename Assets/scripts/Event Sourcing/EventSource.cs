using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using UniRx;
using ZeroFormatter;
using strange.extensions.command.api;
using strange.extensions.command.impl;

namespace eventsourcing {

    public class EventSource : MonoBehaviour {

        private EntityManager EM;
        private CustomStack<IEvent> allEvents;
        private Subject<IEvent> newEventObservable;

        public UniRx.IObservable<IEvent> NewEventObservable => newEventObservable;
        public UniRx.IObservable<IEvent> AllEventObservable => Observable.ToObservable(allEvents);
        public int EventCount => allEvents.Count;

        protected void Awake() {
            Reset();
        }

        protected void Start() {
            CheckForEM();
        }

        public void ResetWithByteData(byte[] data) {
            IEvent[] array = Serialisation.To<IEvent[]>(data);

            Reset();

            for (int i = 0; i < array.Length; i++) { // Ignore interval data for now
                RegisterEvent(array[i]);
            }

        }

        public void ExtractByteData(Action<byte[]> callback) {
            callback.Invoke(ZeroFormatterSerializer.Serialize(allEvents.ToArray()));
        }

        private void CheckForEM() {
            if (EM == null) {
                EM = GetComponent<EntityManager>() ?? GameObject.FindObjectOfType<EntityManager>();
                if (EM == null)
                    throw new Exception("Cannot undo when no Entity Manager is present");
            }
        }

        private void Reset() {
            newEventObservable = new Subject<IEvent>();
            allEvents = new CustomStack<IEvent>();
        }

        public void RegisterEvent(IEvent e) {
            Assert.IsNotNull(e);

            newEventObservable.OnNext(e);
            allEvents.Push(e);
        }

        public void ApplyProjection(IProjection projection, EventStream eventStream) {
            UniRx.IObservable<IEvent> stream = null;

            switch (eventStream) {
            case EventStream.AllExistingEvents:
                stream = AllEventObservable; // Terminating since it comes from a list
                break;
            case EventStream.NewEvents:
                stream = NewEventObservable; // Unterminating, needs to be cancelled using the CancelToken when desired
                break;
            case EventStream.ActionableEvents:
                stream = AllEventObservable.Where(e => e is IActionableEvent);
                break;
            case EventStream.NewActionableEvents:
                stream = NewEventObservable.Where(e => e is IActionableEvent);
                break;
            case EventStream.NonActionableEvents:
                stream = AllEventObservable.Where(e => !(e is IActionableEvent));
                break;
            case EventStream.NewNonActionableEvents:
                stream = NewEventObservable.Where(e => !(e is IActionableEvent));
                break;
            default:
                throw new NotImplementedException();
            }

            projection.Reset();

            projection.CancelToken = stream
                .Finally(() => {
                    projection.OnFinish();
                    if (projection.CancelToken != null) {
                        projection.CancelToken.Dispose();
                        projection.CancelToken = null;
                    }
                })
                .Subscribe(e => {
                    if (!projection.Process(e)) {
                        projection.CancelToken.Dispose();
                        projection.CancelToken = null;
                    }
                });

        }

        private void _Do(IEvent e) {
            if (e is IActionableEvent) {
                IBaseCommand doCommand = (e as IActionableEvent).NewDoCommand();
                if (doCommand is IModifier) {
                    IModifier m = (doCommand as IModifier);
                    Assert.IsTrue(m is IEventProducing);
                    EM.Mod(m);
                } else if (doCommand is Command) {
                    throw new NotImplementedException();
                } else
                    throw new Exception();
            }
        }

        public void Do(IEvent e) {
            CheckForEM();

            if (e == null)
                throw new NullReferenceException("IEvent");

            _Do(e);
        }

        public void Do(List<IEvent> events) {
            CheckForEM();

            for (int i = 0; i < events.Count; i++) {
                IEvent e = events[i];

                if (e == null)
                    throw new NullReferenceException("IEvent");

                _Do(e);
            }
        }

        private IEvent _Undo(bool countNonActionable = true) { // TODO
            IEvent e = allEvents.Pop(); // Make method for this

            if (e is IActionableEvent) {
                IBaseCommand undoCommand = (e as IActionableEvent).NewUndoCommand();
                if (undoCommand is IModifier) {
                    IModifier m = (undoCommand as IModifier);
                    Assert.IsTrue(m is IEventProducing);
                    (m as IEventProducing).DontRecordEvent = true;
                    EM.Mod(m);
                } else if (undoCommand is Command) {
                    throw new NotImplementedException();
                } else
                    throw new Exception();
            }

            return e;
        }

        public IEvent Undo(bool countNonActionable = true) {
            CheckForEM();

            if (allEvents.Count < 0)
                throw new Exception("No events to undo");

            return _Undo(countNonActionable);
        }

        public List<IEvent> Undo(int count, bool countNonActionable = true) {
            CheckForEM();

            if (count < 0)
                throw new Exception("Count must be greater than or equally to 0");

            if (allEvents.Count < count)
                count = allEvents.Count;

            List<IEvent> undoneEvents = new List<IEvent>();

            for (int i = 0; i < count; i++) {
                undoneEvents.Add(_Undo(countNonActionable));
            }

            undoneEvents.Reverse();
            return undoneEvents;
        }

        public List<IEvent> UndoWhile(Func<IEvent, bool> predicate, bool countNonActionable = true) {
            CheckForEM();

            List<IEvent> undoneEvents = new List<IEvent>();
            while (allEvents.Count > 0 && predicate.Invoke(allEvents.Peek())) {
                undoneEvents.Add(_Undo(countNonActionable));
            }
            undoneEvents.Reverse();
            return undoneEvents;
        }

        public List<IEvent> UndoUntil(IEvent e, int max = int.MaxValue, bool countNonActionable = true) {
            CheckForEM();

            List<IEvent> undoneEvents = new List<IEvent>();
            while (allEvents.Count > 0 && undoneEvents.Count < max && allEvents.Peek() != e) {
                undoneEvents.Add(_Undo(countNonActionable));
            }
            undoneEvents.Reverse();
            return undoneEvents;
        }

    }

}
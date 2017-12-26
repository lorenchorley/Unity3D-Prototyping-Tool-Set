using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using UniRx;
using ZeroFormatter;

namespace eventsource {

    public class EventSource : MonoBehaviour {

        private ReplaySubject<ESEvent> allEventObserable;
        public UniRx.IObservable<ESEvent> AllEventObserable {
            get {
                return allEventObserable;
            }
        }

        private Subject<ESEvent> newEventObserable;
        public UniRx.IObservable<ESEvent> NewEventObserable {
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

        //public List<ESEvent> Events;
        private Dictionary<Type, IESEntityRegistry> Registries;

        public EventSource() {
            //Events = new List<ESEvent>();
            Registries = new Dictionary<Type, IESEntityRegistry>();
            Reset();
        }

        private void Reset() {
            newEventObserable = new Subject<ESEvent>();
            allEventObserable = new ReplaySubject<ESEvent>();
            eventCount = 0;
        }

        public void ResetWithByteData(byte[] data) {
            //ESEvent[] array = (ESEvent[]) data.DeserializeFromBinary();
            ESEvent[] array = ZeroFormatterSerializer.Deserialize<ESEvent[]>(data);
            //Events = new List<ESEvent>(array);
            Reset();

            for (int i = 0; i < array.Length; i++) { // Ignore interval data for now
                RegisterEvent(array[i]);
            }

        }

        public void ExtractByteData(Action<byte[]> callback) {
            List<ESEvent> list = new List<ESEvent>();

            allEventObserable
                .DoOnCompleted(() => callback.Invoke(ZeroFormatterSerializer.Serialize(list.ToArray())))
                .Subscribe(list.Add);

        }

        public void RegisterRegistry<E>(IESEntityRegistry<E> registry) where E : IESEntity {
            Registries.Add(typeof(E), registry);
            Registries.Add(registry.GetType(), registry);
        }

        public R GetRegistry<R>() where R : IESEntityRegistry {
            return (R) Registries[typeof(R)];
        }

        public R GetRegistryByEntityType<R>() where R : IESEntity {
            return (R) Registries[typeof(R)];
        }

        public void Command<E, C>(int uid, C command) where C : ESCommand where E : IESEntity, IESCommandable<C> {
            IESEntityRegistry<E> registry = (IESEntityRegistry<E>) Registries[typeof(E)];
            var p = registry.GetEntityByUID(uid);
            Command(p, command);
        }

        public void Command<E, C>(int uid, IESEntityRegistry<E> registry, C command) where C : ESCommand where E : IESEntity, IESCommandable<C> {
            var p = registry.GetEntityByUID(uid);
            Command(p, command);
        }

        public void Command<E, C>(E entity, C command) where C : ESCommand where E : IESEntity, IESCommandable<C> {
            Assert.IsFalse(command.Complete);
            command.Complete = true;
            RegisterEvent(entity.ESUSEONLYCOMMAND(command));
        }

        public void Command<C>(C command) where C : ESIndependentCommand {
            Assert.IsFalse(command.Complete);
            command.Complete = true;
            RegisterEvent(command.Execute(this));
        }

        public void RegisterEvent(ESEvent e) {
            Assert.IsFalse(e.registered);
            e.registered = true;
            //Events.Add(e);
            eventCount++;
            newEventObserable.OnNext(e);
            allEventObserable.OnNext(e);
        }

        public void Query<E, Q>(int uid, Q query) where Q : IESQuery where E : IESEntity, IESQueriable<Q> {
            IESEntityRegistry<E> registry = (IESEntityRegistry<E>) Registries[typeof(E)];
            Query<E, Q>(registry.GetEntityByUID(uid), query);
        }

        public void Query<E, Q>(int uid, IESEntityRegistry<E> registry, Q query) where Q : IESQuery where E : IESEntity, IESQueriable<Q> {
            Query<E, Q>(registry.GetEntityByUID(uid), query);
        }

        public void Query<E, Q>(E entity, Q query) where Q : IESQuery where E : IESEntity, IESQueriable<Q> {
            entity.ESUSEONLYQUERY(query);
        }

        public void ApplyProjection(IESProjection projection) {
            projection.Reset();

            IDisposable disposable = null;
            Action<ESEvent> sub = e => {
                if (!projection.Process(e)) {
                    disposable.Dispose();
                    disposable = null;
                }
            };
            Action fin = () => {
                projection.OnFinish();
                disposable.Dispose();
                disposable = null;
            };

            disposable =
                AllEventObserable
                    .DoOnCompleted(fin)
                    .Subscribe(sub);

            //int length;
            //int id;
            //if (projection is IESProjectionAbsolute) {
            //    IESProjectionAbsolute absolute = projection as IESProjectionAbsolute;

            //while (absolute.NextRequest(out id, out length)) {
            //    if (id + length > Events.Count) {
            //        if (id > Events.Count) {
            //            break;
            //        } else {
            //            length = Events.Count - id;
            //        }
            //    }

            //    for (int i = 0; i < length; i++) {
            //        projection.Process(id, Events[id]);
            //        id++;
            //    }

            //}

            //} else if (projection is IESProjectionRelativeContinuing) {
            //    IESProjectionRelativeContinuing relative = projection as IESProjectionRelativeContinuing;

            //    id = 0;
            //    while (relative.NextRequest(out length)) {
            //        if (id + length > Events.Count) {
            //            if (id >= Events.Count) {
            //                break;
            //            } else {
            //                length = Events.Count - id;
            //            }
            //        }

            //        for (int i = 0; i < length; i++) {
            //            projection.Process(id, Events[id]);
            //            id++;
            //        }

            //    }

            //}

            //projection.OnFinish();

        }

    }

}
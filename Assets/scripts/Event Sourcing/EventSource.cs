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

        private Dictionary<Type, IEntityRegistry> Registries;

        public EventSource() {
            Registries = new Dictionary<Type, IEntityRegistry>();
            Reset();
        }

        private void Reset() {
            newEventObserable = new Subject<IEvent>();
            allEventObserable = new ReplaySubject<IEvent>();
            eventCount = 0;
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

        public void RegisterRegistry<E>(IEntityRegistry<E> registry) where E : IEntity {
            Registries.Add(typeof(E), registry);
            Registries.Add(registry.GetType(), registry);
        }

        public R GetRegistry<R>() where R : IEntityRegistry {
            return (R) Registries[typeof(R)];
        }

        public R GetRegistryByEntityType<R>() where R : IEntity {
            return (R) Registries[typeof(R)];
        }

        public void Command<E, C>(int uid, C command) where C : IModifier where E : IEntity, IModifiable<C> {
            IEntityRegistry<E> registry = (IEntityRegistry<E>) Registries[typeof(E)];
            var p = registry.GetEntityByUID(uid);
            Command(p, command);
        }

        public void Command<E, C>(int uid, IEntityRegistry<E> registry, C command) where C : IModifier where E : IEntity, IModifiable<C> {
            var p = registry.GetEntityByUID(uid);
            Command(p, command);
        }

        public void Command<E, C>(E entity, C command) where C : IModifier where E : IModifiable<C> {
            RegisterEvent(entity.ESUSEONLYMODIFY(command));
        }

        public void Command<C>(C command) where C : IndependentModifier {
            command.ES = this;
            command.Execute();

            if (command is IEventCreator) {
                RegisterEvent((command as IEventCreator).Event);
            }

        }

        public void RegisterEvent(IEvent e) {
            Assert.IsNotNull(e);
            eventCount++;
            newEventObserable.OnNext(e);
            allEventObserable.OnNext(e);
        }

        public void Query<E, Q>(int uid, Q query) where Q : IQuery where E : IEntity, IQueriable<Q> {
            IEntityRegistry<E> registry = (IEntityRegistry<E>) Registries[typeof(E)];
            Query<E, Q>(registry.GetEntityByUID(uid), query);
        }

        public void Query<E, Q>(int uid, IEntityRegistry<E> registry, Q query) where Q : IQuery where E : IEntity, IQueriable<Q> {
            Query<E, Q>(registry.GetEntityByUID(uid), query);
        }

        public void Query<E, Q>(E entity, Q query) where Q : IQuery where E : IEntity, IQueriable<Q> {
            entity.ESUSEONLYQUERY(query);
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
                .Merge()
                .Subscribe(e => {
                    if (!projection.Process(e)) {
                        disposable.Dispose();
                        disposable = null;
                    }
                });
            
        }

    }
    
}
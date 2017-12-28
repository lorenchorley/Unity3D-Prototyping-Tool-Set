using UnityEngine;
using System.Collections.Generic;
using System;

namespace eventsourcing {

    public class EntityManager : MonoBehaviour {

        private EventSource ES;
        private Dictionary<Type, IEntityRegistry> Registries;

        protected void Awake() {
            ES = GetComponent<EventSource>() ?? FindObjectOfType<EventSource>();
            if (ES == null)
                throw new Exception("Unable to find active EventSource in scene");

            Reset();
        }

        private void Reset() {
            Registries = new Dictionary<Type, IEntityRegistry>();
        }

        public void ResetWithByteData(byte[] data) {
            throw new NotImplementedException();
        }

        public void ExtractByteData(Action<byte[]> callback) {
            throw new NotImplementedException();
        }

        public void Register<E>(IEntityRegistry<E> r) where E : IEntity {
            Registries.Add(typeof(E), r);
            Registries.Add(r.GetType(), r);
        }

        public R GetRegistry<R>() where R : IEntityRegistry {
            return (R) Registries[typeof(R)];
        }

        public R GetRegistryByEntityType<R>() where R : IEntity {
            return (R) Registries[typeof(R)];
        }

        public void Mod<E, M>(int uid, M m) where M : IModifier where E : IEntity, IModifiable<M> {
            IEntityRegistry<E> registry = (IEntityRegistry<E>) Registries[typeof(E)];
            var p = registry.GetEntityByUID(uid);
            Mod(p, m);
        }

        public void Mod<E, M>(int uid, IEntityRegistry<E> r, M m) where M : IModifier where E : IEntity, IModifiable<M> {
            var p = r.GetEntityByUID(uid);
            Mod(p, m);
        }

        public void Mod<E, M>(E e, M m) where M : IModifier where E : IModifiable<M> {
            ApplyMod(m, e.ApplyMod);
        }

        public void Mod<M>(M m) where M : IModifier {
            ApplyMod(m, null);
        }

        private void ApplyMod<M>(M m, Func<M, IEvent> function) where M : IModifier {

            if (m is IESInjected)
                (m as IESInjected).ES = ES;

            if (m is IEMInjected)
                (m as IEMInjected).EM = this;

            IEvent producedEvent = null;

            if (function == null) {
                m.Execute();

                if (m is IEventProducing && (m as IEventProducing).RecordEvent) {
                    producedEvent = (m as IEventProducing).Event;
                    ES.RegisterEvent(producedEvent);
                }

            } else {
                producedEvent = function.Invoke(m);
                ES.RegisterEvent(producedEvent);
            }

        }

        public void Query<E, Q>(int uid, Q q) where Q : IQuery where E : IEntity, IQueriable<Q> {
            IEntityRegistry<E> registry = (IEntityRegistry<E>) Registries[typeof(E)];
            Query<E, Q>(registry.GetEntityByUID(uid), q);
        }

        public void Query<E, Q>(int uid, IEntityRegistry<E> r, Q q) where Q : IQuery where E : IEntity, IQueriable<Q> {
            Query(r.GetEntityByUID(uid), q);
        }

        public void Query<E, Q>(E e, Q q) where Q : IQuery where E : IEntity, IQueriable<Q> {
            e.Query(q);
        }

    }

}
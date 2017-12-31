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
            E e = registry.GetEntityByUID(uid);
            ApplyMod(m, e.ApplyMod);
        }

        public void Mod<E, M>(int uid, IEntityRegistry<E> r, M m) where M : IModifier where E : IEntity, IModifiable<M> {
            E e = r.GetEntityByUID(uid);
            ApplyMod(m, e.ApplyMod);
        }

        public void Mod<E, M>(E e, M m) where M : IModifier where E : IEntity, IModifiable<M> {
            if (e.GetType().IsValueType) // If the entity is a struct, get the up to date version from the registry first before passing its associated function
                ApplyMod(m, (e.Key.Registry as IEntityRegistry<E>).GetEntityByKey(e.Key).ApplyMod);
            else
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

                if (m is IEventProducing && !(m as IEventProducing).DontRecordEvent) {
                    producedEvent = (m as IEventProducing).Event;
                    ES.RegisterEvent(producedEvent);
                }

            } else {
                producedEvent = function.Invoke(m);
                ES.RegisterEvent(producedEvent);
            }

        }

        public void Query<E, Q>(int uid, Q q) where Q : IQuery where E : IEntity, IQueriable<Q> {
            IEntityRegistry<E> r = (IEntityRegistry<E>) Registries[typeof(E)];
            Query<E, Q>(r.GetEntityByUID(uid), q);
        }

        public void Query<E, Q>(int uid, IEntityRegistry<E> r, Q q) where Q : IQuery where E : IEntity, IQueriable<Q> {
            Query(r.GetEntityByUID(uid), q);
        }

        // Better method to use for struct entities, since the entity value never has to be retrived
        public void Query<E, Q>(EntityKey key, Q q) where Q : IQuery where E : IEntity, IQueriable<Q> {
            Query((key.Registry as IEntityRegistry<E>).GetEntityByKey(key), q);
        }

        public void Query<E, Q>(E e, Q q) where Q : IQuery where E : IEntity, IQueriable<Q> {
            if (e.GetType().IsValueType) // If the entity is a struct, get the up to date version from the registry
                (e.Key.Registry as IEntityRegistry<E>).ApplyQuery<E, Q>(e.Key, q); // Apply query directly to value in registry
            else
                e.Query(q);
        }
        
    }

}
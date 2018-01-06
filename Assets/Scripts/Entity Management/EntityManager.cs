using UnityEngine;
using System.Collections.Generic;
using System;
using eventsourcing;

namespace entitymanagement {

    public class EntityManager : MonoBehaviour {

        private EventSource ES;
        private Dictionary<Type, IEntityRegistry> Registries;
        public List<Func<IModifier, IModifier>> Filters;
        public List<Action<IModifier, Action<IModifier>>> AsyncFilters;

        protected void Awake() {
            ES = GetComponent<EventSource>() ?? FindObjectOfType<EventSource>();
            if (ES == null)
                throw new Exception("Unable to find active EventSource in scene");

            Reset();

            Filters = new List<Func<IModifier, IModifier>>();
            AsyncFilters = new List<Action<IModifier, Action<IModifier>>>();
        }

        private void Reset() {
            Registries = new Dictionary<Type, IEntityRegistry>();
        }

        public void ResetWithByteData(byte[] data) {
            Registries = Serialisation.To<Dictionary<Type, IEntityRegistry>>(data);
        }

        public void ExtractByteData(Action<byte[]> callback) {
            callback.Invoke(Serialisation.ToBinary(Registries));
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

        public void ApplyEntityMod<E, M>(int uid, M m) where M : IEntityModifier where E : IEntity, IModifiable<M> {
            IEntityRegistry<E> r = (IEntityRegistry<E>) Registries[typeof(E)];
            E e = r.GetEntityByUID(uid);
            m.e = e;
            ApplyEntityMod<M>(m);
        }

        public void ApplyEntityMod<E, M>(int uid, IEntityRegistry<E> r, M m) where M : IEntityModifier where E : IEntity, IModifiable<M> {
            E e = r.GetEntityByUID(uid);
            m.e = e;
            ApplyEntityMod<M>(m);
        }

        public void ApplyEntityMod<M>(IEntity e, M m) where M : IEntityModifier {
            m.e = e;
            ApplyEntityMod<M>(m);
        }

        public void ApplyEntityMod<M>(EntityKey key, M m) where M : IEntityModifier {
            if (m.e == null)
                m.e = new EmptyEntity();

            m.e = (key.Registry as IEntityRegistry).GetUncastEntityByKey(key);

            ApplyEntityMod<M>(m);
        }

        public void ApplyEntityModWhere<E, M>(M m, Predicate<EntityKey> pkey) where M : IEntityModifier where E : IEntity, IModifiable<M> {
            IEntityRegistry<E> r = (IEntityRegistry<E>) Registries[typeof(E)];
            foreach (E e in r.Entities) {
                if (pkey.Invoke(e.Key))
                    ApplyEntityMod(e, m);
            }
        }

        public void ApplyEntityModWhere<E, M>(M m, Predicate<E> pkey) where M : IEntityModifier where E : IEntity, IModifiable<M> {
            IEntityRegistry<E> r = (IEntityRegistry<E>) Registries[typeof(E)];
            foreach (E e in r.Entities) {
                if (pkey.Invoke(e))
                    ApplyEntityMod(e, m);
            }
        }
        
        public void ApplyMod<M>(M unfilteredMod) where M : IIndependentModifier {
            if (unfilteredMod is IEntityModifier)
                throw new Exception("Use ApplyEntityMod instead for classes that implement IEntityModifier: " + unfilteredMod.GetType().Name);

            ApplyModFilters(unfilteredMod, m => {

                if (m is IESInjected)
                    (m as IESInjected).ES = ES;

                if (m is IEMInjected)
                    (m as IEMInjected).EM = this;

                m.Execute();

                if (m is IEventProducing && !(m as IEventProducing).DontRecordEvent) {
                    IEvent producedEvent = (m as IEventProducing).Event;
                    ES.RegisterEvent(producedEvent);
                }

            });
        }

        public void ApplyEntityMod<M>(M unfilteredMod) where M : IEntityModifier {
            ApplyModFilters(unfilteredMod, m => {

                if (m is IESInjected)
                    (m as IESInjected).ES = ES;

                if (m is IEMInjected)
                    (m as IEMInjected).EM = this;

                IEvent producedEvent = null;
                Func<M, IEvent> function = GetEntityModFunction(m);

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

            });
        }

        private void ApplyModFilters<M>(M m, Action<M> callback) where M : IModifier {
            for (int i = 0; i < Filters.Count; i++)
                m = (M) Filters[i].Invoke(m);

            Action<int, M> AsyncRun = null;
            AsyncRun = (i, n) => {
                if (i < AsyncFilters.Count)
                    AsyncFilters[i].Invoke(n, o => AsyncRun(i + 1, (M) o));
                else
                    callback.Invoke(n);
            };
            AsyncRun(0, m);

        }

        private Func<M, IEvent> GetEntityModFunction<M>(M m) where M : IEntityModifier {
            if (!(m.e is IModifiable<M>))
                return null; // Default to the execute method
                //throw new Exception(m.e.GetType().Name + " must extend IModifiable<" + typeof(M).Name + "> to be able to use " + m.GetType().Name + " on it.");

            if (m.e.GetType().IsValueType) // If the entity is a struct, get the up to date version from the registry first before passing its associated function
                m.e = (m.e.Key.Registry as IEntityRegistry).GetUncastEntityByKey(m.e.Key);

            return (m.e as IModifiable<M>).ApplyMod;
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
using UnityEngine;
using System.Collections.Generic;
using System;
using eventsourcing;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.injector.api;

namespace entitymanagement {

    public class EntityManager : MonoBehaviour {

        private IInjectionBinder injectionBinder;
        private EventSource ES;
        private Dictionary<Type, IEntityRegistry> Registries;
        public List<Action<IModifier, Action<IModifier>>> Filters;

        protected void Awake() {

            ContextView contextView = GetComponentInParent<ContextView>();
            IContext context = contextView?.context ?? Context.firstContext;
            if (context != null && context is CrossContext) { 
                injectionBinder = (contextView.context as CrossContext).injectionBinder;
                injectionBinder.Bind<EntityManager>().ToValue(this).ToSingleton().CrossContext();
            }
             
            ES = GetComponent<EventSource>() ?? FindObjectOfType<EventSource>();
            if (ES == null)
                throw new Exception("Unable to find active EventSource in scene");

            Reset();

            Filters = new List<Action<IModifier, Action<IModifier>>>();
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

        public void RefreshEntityOnModifierUsingUID(IModifier m) {
            if (m is IEntityModifier) {
                IEntityModifier em = m as IEntityModifier;
                IEntityRegistry r = GetRegistryByEntityType(em.IntendedEntityType);
                em.e = r.GetUncastEntityByUID(em.e.UID);
            }
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

        public IEntityRegistry GetRegistryByEntityType(Type type) {
            return Registries[type];
        }

        public void ApplyMod<E, M>(int uid, M m) where M : IEntityModifier where E : IEntity, IModifiable<M> {
            IEntityRegistry<E> r = (IEntityRegistry<E>) Registries[typeof(E)];
            E e = r.GetEntityByUID(uid);
            m.e = e;
            ApplyMod<M>(m);
        }

        public void ApplyMod<E, M>(int uid, IEntityRegistry<E> r, M m) where M : IEntityModifier where E : IEntity, IModifiable<M> {
            E e = r.GetEntityByUID(uid);
            m.e = e;
            ApplyMod<M>(m);
        }

        public void ApplyMod<M>(IEntity e, M m) where M : IEntityModifier {
            m.e = e;
            ApplyMod<M>(m);
        }

        public void ApplyEntityMod<M>(EntityKey key, M m) where M : IEntityModifier {
            if (m.e == null)
                m.e = new EmptyEntity();

            m.e = (key.Registry as IEntityRegistry).GetUncastEntityByKey(key);

            ApplyMod<M>(m);
        }

        public void ApplyModWhere<E, M>(M m, Predicate<EntityKey> pkey) where M : IEntityModifier where E : IEntity, IModifiable<M> {
            IEntityRegistry<E> r = (IEntityRegistry<E>) Registries[typeof(E)];
            foreach (E e in r.Entities) {
                if (pkey.Invoke(e.Key))
                    ApplyMod(e, m);
            }
        }

        public void ApplyModWhere<E, M>(M m, Predicate<E> pkey) where M : IEntityModifier where E : IEntity, IModifiable<M> {
            IEntityRegistry<E> r = (IEntityRegistry<E>) Registries[typeof(E)];
            foreach (E e in r.Entities) {
                if (pkey.Invoke(e))
                    ApplyMod(e, m);
            }
        }
        
        public void ApplyMod<M>(M unfilteredMod, Action<IModifier, Action<IModifier>> excludedAsyncFilter = null) where M : IModifier {
            ApplyModFilters(unfilteredMod, excludedAsyncFilter, m => {
                if (m == null) // If null, don't process
                    return;

                if (m is IIoCInjected && injectionBinder != null)
                    injectionBinder.injector.Inject(m);

                if (m is IESInjected)
                    (m as IESInjected).ES = ES;

                if (m is IEMInjected)
                    (m as IEMInjected).EM = this;

                m.Execute();

                if (m is IEventProducing && !(m as IEventProducing).DontRecordEvent) {
                    ES.RegisterEvent((m as IEventProducing).Event);
                }

            });
        }
        
        private void ApplyModFilters<M>(M m, Action<IModifier, Action<IModifier>> excludedAsyncFilter, Action<M> callback) where M : IModifier {
            Action<int, M> AsyncRun = null;

            AsyncRun = (i, n) => {
                if (i < Filters.Count) {
                    if (excludedAsyncFilter != Filters[i])
                        Filters[i].Invoke(n, o => AsyncRun(i + 1, (M) o));
                    else
                        AsyncRun(i + 1, (M) n);
                } else
                    callback.Invoke(n);
            };

            AsyncRun(0, m);
        }

        //private Func<M, IEvent> GetEntityModFunction<M>(M m) where M : IEntityModifier {
        //    IModifiable<M> e = m.e as IModifiable<M>;
        //    if (e == null)
        //        return null; // Default to the execute method
        //        //throw new Exception(m.e.GetType().Name + " must extend IModifiable<" + typeof(M).Name + "> to be able to use " + m.GetType().Name + " on it.");

        //    // TODO Will not work with empty entity! gettype will return typeof(EmptyEntity), not the intended struct type
        //    // Put bool in registry instead
        //    if (e.GetType().IsValueType) // If the entity is a struct, get the up to date version from the registry first before passing its associated function
        //        m.e = (m.e.Key.Registry as IEntityRegistry).GetUncastEntityByKey(m.e.Key);

        //    return e.ApplyMod;
        //}

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
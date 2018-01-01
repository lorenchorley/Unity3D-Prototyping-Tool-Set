using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using System;

namespace eventsourcing.examples.basic {

    public class PersonRegistry : IValueEntityRegistry<PersonEntity> {

        private int newUID = 0;
        private List<int> uids;
        private List<PersonEntity> entities; // TODO Need to replace this with an array that can produce references to values in the list
        private Dictionary<int, EntityKey> keyByUID; // You can implement any number of ways to index or key your entities like this
        private int averageCount;

        public IEnumerable<PersonEntity> Entities => entities;
        public IEnumerable<int> UIDs => uids;

        public PersonRegistry(EntityManager EM, int averageCount) {
            this.averageCount = averageCount;
            EM.Register(this);
            Reset();
        }

        private void Reset() {
            if (keyByUID == null)
                keyByUID = new Dictionary<int, EntityKey>();
            else
                keyByUID.Clear();

            if (uids == null)
                uids = new List<int>(averageCount);
            else
                uids.Clear();

            if (entities == null)
                entities = new List<PersonEntity>(averageCount);
            else
                entities.Clear();
        }

        public int EntityCount {
            get {
                return entities.Count;
            }
        }
        
        public PersonEntity GetEntityByUID(int uid) {
            return entities[keyByUID[uid].Index];
        }

        public PersonEntity GetEntityByKey(EntityKey key) {
            return entities[key.Index];
        }

        public void SetEntity(ref PersonEntity e) {
            EntityKey k;
            if (keyByUID.TryGetValue(e.UID, out k) && e.Key.Equals(k)) {// Update registered entity
                Assert.AreEqual(this, e.Key.Registry);

                entities[e.Key.Index] = e;

            } else {// Set new entity

                // Set the uid seed to be greater than the largest uid
                if (e.UID >= newUID)
                    newUID = e.UID + 1;

                // Generate new key for this registry
                e.Key = new EntityKey() {
                    Index = entities.Count,
                    Registry = this
                };

                keyByUID.Add(e.UID, e.Key);
                uids.Add(e.UID);
                entities.Add(e);

            }

        }

        public PersonEntity NewEntity() {
            PersonEntity e = new PersonEntity(newUID++, new EntityKey { Index = entities.Count, Registry = this });

            keyByUID[e.UID] = e.Key;
            uids.Add(e.UID);
            entities.Add(e);

            return e;
        }

        void IEntityRegistry<PersonEntity>.ApplyQuery<F, Q>(EntityKey key, Q q) {
            IEntity e = entities[key.Index]; // TODO apply directly to value in list, need a way to access the value directly, list that supports ref
            ((F) e).Query(q);
        }

        // Private serialisation pattern
        public object SerialisableInformation;
        [Serializable] class Serialisable {
            public IEnumerable<object> entities;
        }

        public void OnAfterDeserialise(params object[] p) {
            // Reregister with the EntityManager
            Assert.IsTrue(p.Length > 0 && p[0] is EntityManager);
            (p[0] as EntityManager).Register(this);

            // Make sure serialised info is available, deserialise...
            Assert.IsNotNull(SerialisableInformation);
            Serialisable s = SerialisableInformation as Serialisable;
            PersonEntity e;

            Reset();
            foreach (object o in s.entities) {
                e = new PersonEntity();
                e.DeserialiseFromClonedObject(o);
                SetEntity(ref e);
            }

            SerialisableInformation = null;
        }

        public void OnBeforeSerialise(params object[] p) {

            // Fill serialisable info with serialisable-wrapped list of cloned person entities
            SerialisableInformation = new Serialisable {
                entities = entities.Select(e => e.CloneToSerialisedObject())
            };

        }

    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;

namespace eventsourcing.examples.basic {

    public class PersonRegistry : IEntityRegistry<PersonEntity> {

        private int newUID = 0;
        private List<int> uids;
        private List<PersonEntity> entities; // TODO Need to replace this with an array that can produce references to values in the list
        private Dictionary<int, EntityKey> keyByUID;

        public IEnumerable<PersonEntity> Entities => entities;
        public IEnumerable<int> UIDs => uids;

        public PersonRegistry(EntityManager EM, int averageCount) {
            EM.Register(this);

            keyByUID = new Dictionary<int, EntityKey>();
            uids = new List<int>(averageCount);
            entities = new List<PersonEntity>(averageCount);

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

        public void SetEntity(PersonEntity e) {
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

    }

}
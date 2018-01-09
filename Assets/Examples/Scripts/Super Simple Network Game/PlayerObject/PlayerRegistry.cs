using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using entitymanagement;

namespace eventsourcing.examples.network {

    public class PlayerRegistry : IReferenceEntityRegistry<PlayerEntity> {

        private int newUID = 0;
        private List<int> uids;
        private List<PlayerEntity> entities;
        private Dictionary<int, int> entityIndexByUID;

        public IEnumerable<PlayerEntity> Entities => entities;
        public IEnumerable<int> UIDs => uids;

        public PlayerRegistry(EntityManager EM, int averageQuantity) {
            EM.Register(this);

            entityIndexByUID = new Dictionary<int, int>();
            uids = new List<int>(averageQuantity);
            entities = new List<PlayerEntity>(averageQuantity);
        }

        public int EntityCount {
            get { 
                return entities.Count;
            }
        }

        public IEntity GetUncastEntityByUID(int uid) {
            return entities[entityIndexByUID[uid]];
        }

        public PlayerEntity GetEntityByUID(int uid) {
            return entities[entityIndexByUID[uid]];
        }

        public IEntity GetUncastEntityByKey(EntityKey key) {
            return entities[key.Index];
        }

        public PlayerEntity GetEntityByKey(EntityKey key) {
            return entities[key.Index];
        }

        public void SetEntity(IEntity e) {
            if (!(e is PlayerEntity))
                throw new Exception("Incorrect entity type");

            SetEntity((PlayerEntity) e);
        }

        public void SetEntity(int uid, PlayerEntity e) {
            if (uid >= newUID)
                newUID = uid + 1;

            entityIndexByUID[uid] = entities.Count;
            uids.Add(uid);
            entities.Add(e);

        }

        public void SetEntity(PlayerEntity e) {
            entities[e.Key.Index] = e;
        }

        public PlayerEntity NewEntity() {
            EntityKey key = new EntityKey() {
                Index = entities.Count,
                Registry = this
            };
            PlayerEntity e = new PlayerEntity(newUID++, key);

            entityIndexByUID[e.UID] = key.Index;
            uids.Add(e.UID);
            entities.Add(e);

            return e;
        }

        public void ApplyQuery<F, Q>(EntityKey key, Q q) where F : PlayerEntity, IQueriable<Q> where Q : IQuery {
            ((F) entities[key.Index]).Query(q);
        }

        public void OnAfterDeserialise(params object[] p) {
            throw new NotImplementedException();
        }

        public void OnBeforeSerialise(params object[] p) {
            throw new NotImplementedException();
        }

    }

}
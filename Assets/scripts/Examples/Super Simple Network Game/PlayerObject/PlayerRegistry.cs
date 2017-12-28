using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace eventsourcing.examples.network {

    public class PlayerRegistry : IEntityRegistry<PlayerEntity> {

        private int newUID = 0;
        private List<int> uids;
        private List<PlayerEntity> entities;
        private Dictionary<int, int> entityIndexByUID;

        public IList<PlayerEntity> Entities => entities;
        public IList<int> UIDs => uids;

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

        public PlayerEntity GetEntityByUID(int uid) {
            return entities[entityIndexByUID[uid]];
        }

        public void SetEntity(int uid, PlayerEntity e) {
            if (uid >= newUID)
                newUID = uid + 1;

            entityIndexByUID[uid] = entities.Count;
            uids.Add(uid);
            entities.Add(e);

        }

        public PlayerEntity NewEntity() {
            int uid = newUID++;
            PlayerEntity e = new PlayerEntity(uid);

            entityIndexByUID[uid] = entities.Count;
            uids.Add(uid);
            entities.Add(e);

            return e;
        }

    }

}
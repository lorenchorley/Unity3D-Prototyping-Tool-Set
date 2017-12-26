using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace eventsource.examples.network {

    public class PlayerRegistry : IESEntityRegistry<PlayerEntity> {

        private int newUID = 0;
        private List<int> uids;
        private List<PlayerEntity> entities;
        private Dictionary<int, int> entityIndexByUID;
        private EventSource ES;

        public IList<PlayerEntity> Entities => entities;
        public IList<int> UIDs => uids;

        public PlayerRegistry(EventSource ES) {
            this.ES = ES;
            ES.RegisterRegistry(this);
            entityIndexByUID = new Dictionary<int, int>();
            uids = new List<int>(5);
            entities = new List<PlayerEntity>(5);
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
            PlayerEntity e = new PlayerEntity(ES, uid);

            entityIndexByUID[uid] = entities.Count;
            uids.Add(uid);
            entities.Add(e);

            return e;
        }

    }

}
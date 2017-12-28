using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace eventsourcing.examples.basic {

    public class PersonRegistry : IEntityRegistry<PersonEntity> {

        private int newUID = 0;
        private List<int> uids;
        private List<PersonEntity> entities;
        private Dictionary<int, int> entityIndexByUID;
        private EventSource ES;

        public IList<PersonEntity> Entities => entities;
        public IList<int> UIDs => uids;

        public PersonRegistry(EventSource ES) {
            this.ES = ES;
            ES.RegisterRegistry(this);
            entityIndexByUID = new Dictionary<int, int>();
            uids = new List<int>(500);
            entities = new List<PersonEntity>(500);
        }

        public int EntityCount {
            get {
                return entities.Count;
            }
        }

        public PersonEntity GetEntityByUID(int uid) {
            return entities[entityIndexByUID[uid]];
        }

        public void SetEntity(int uid, PersonEntity e) {
            if (uid >= newUID)
                newUID = uid + 1;

            entityIndexByUID[uid] = entities.Count;
            uids.Add(uid);
            entities.Add(e);

        }

        public PersonEntity NewEntity() {
            int uid = newUID++;
            PersonEntity e = new PersonEntity(ES, uid);

            entityIndexByUID[uid] = entities.Count;
            uids.Add(uid);
            entities.Add(e);

            return e;
        }

    }

}
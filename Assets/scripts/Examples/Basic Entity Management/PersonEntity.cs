using UnityEngine;
using System;

namespace eventsourcing.examples.basic {

    public struct PersonEntity : IEntity, IQueriable<PersonAgeQuery>, IModifiable<ChangePersonAgeMod> {

        private int uid;
        public int UID { get { return uid; } }
        
        public EntityKey Key { get; set; }

        private int age;

        public PersonEntity(int uid, EntityKey Key) {
            this.uid = uid;
            this.Key = Key;
            age = 0;
        }
        
        private void UpdateRegisteredValue() {
            (Key.Registry as IEntityRegistry<PersonEntity>).SetEntity(this);
        }

        public IEvent ApplyMod(ChangePersonAgeMod m) {
            // Record old value
            PersonAgeChangedEvent e = new PersonAgeChangedEvent {
                OldAge = age
            };

            // Apply command
            age = m.NewAge;

            // Record new value
            e.NewAge = age;

            UpdateRegisteredValue();

            return e;
        }
        
        public void Query(PersonAgeQuery q) {
            q.Age = age;
        }
    }

}
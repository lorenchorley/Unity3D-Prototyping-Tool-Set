using UnityEngine;
using System;

namespace eventsourcing.examples.basic {

    public class PersonEntity : IEntity, IQueriable<PersonAgeQuery>, IModifiable<ChangePersonAgeMod> {

        private int uid;
        public int UID { get { return uid; } }
        
        public int Index { get; set; }

        private int age;

        public PersonEntity(int uid) {
            this.uid = uid;
            age = 0;
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

            return e;
        }
        
        public void Query(PersonAgeQuery q) {
            q.Age = age;
        }
    }

}
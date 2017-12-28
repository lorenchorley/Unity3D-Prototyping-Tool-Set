using UnityEngine;
using System;

namespace eventsourcing.examples.basic {

    public class PersonEntity : IEntity, IQueriable<PersonAgeQuery>, IModifiable<ChangePersonAgeCommand> {

        private int uid;
        public int UID { get { return uid; } }

        protected EventSource es;
        public EventSource ES {
            get {
                return es;
            }
        }

        public int Index { get; set; }

        private int age;

        public PersonEntity(EventSource es, int uid) {
            this.es = es;
            this.uid = uid;
            age = 0;
        }
        
        public IEvent ESUSEONLYMODIFY(ChangePersonAgeCommand c) {
            // Record old value
            PersonAgeChangedEvent e = new PersonAgeChangedEvent {
                OldAge = age
            };

            // Apply command
            age = c.NewAge;

            // Record new value
            e.NewAge = age;

            return e;
        }
        
        public void ESUSEONLYQUERY(PersonAgeQuery q) {
            q.Age = age;
        }
    }

}
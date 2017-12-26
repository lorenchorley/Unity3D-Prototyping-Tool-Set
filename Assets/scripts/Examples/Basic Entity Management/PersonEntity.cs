using UnityEngine;
using System;

namespace eventsource.examples.basic {

    public class PersonEntity : IESEntity, IESQueriable<PersonAgeQuery>, IESCommandable<ChangePersonAgeCommand> {

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
        
        public ESEvent ESUSEONLYCOMMAND(ChangePersonAgeCommand c) {
            // Record old value
            PersonAgeChangedEvent e = new PersonAgeChangedEvent {
                OldAge = age
            };

            // Apply command
            age = c.NewAge;
            c.Complete = true;

            // Record new value
            e.NewAge = age;

            return e;
        }
        
        public void ESUSEONLYQUERY(PersonAgeQuery q) {
            q.Age = age;
        }
    }

}
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace eventsourcing.examples.basic {

    [Serializable]
    public struct PersonEntity : IEntity, IQueriable<PersonAgeQuery>, IModifiable<ChangePersonAgeMod>, ISerialisationCloning {

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
            (Key.Registry as PersonRegistry).SetEntity(ref this);
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

        // Private serialisation pattern
        [Serializable] class Serialisable {
            public int uid;
            public int age;
        }

        public void DeserialiseFromClonedObject(object clone) {
            Serialisable s = clone as Serialisable;
            Assert.IsNotNull(s);

            uid = s.uid;
            age = s.age;

        }

        public object CloneToSerialisedObject() {
            return new Serialisable {
                uid = uid,
                age = age
            };
        }

    }

}
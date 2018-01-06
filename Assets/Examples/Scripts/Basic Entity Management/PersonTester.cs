using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine.Assertions;
using eventsourcing;

namespace entitymanagement.examples.basic {

    public class PersonTester : MonoBehaviour {

        private EventSource ES;
        private EntityManager EM;

        void Start() {
            Serialisation.InitialiseDevelopmentSerialisation();

            ES = GetComponent<EventSource>() ?? gameObject.AddComponent<EventSource>();
            EM = GetComponent<EntityManager>() ?? gameObject.AddComponent<EntityManager>();
            PersonRegistry r = new PersonRegistry(EM, 10);

            int personUID = r.NewEntity().UID;
            PersonEntity p = r.GetEntityByUID(personUID);
            EntityKey pkey = p.Key;

            PersonAgeQuery q = new PersonAgeQuery();
            EM.Query(p, q);
            Assert.IsTrue(q.Age == 0, "New person");

            Debug.Log("Applying age change to 20");
            ChangePersonAgeMod c = new ChangePersonAgeMod { NewAge = 20 };
            EM.ApplyEntityMod(personUID, r, c);

            q = new PersonAgeQuery();
            EM.Query(personUID, r, q);
            Assert.IsTrue(q.Age == 20);

            Debug.Log("Applying age change to 21");
            c = new ChangePersonAgeMod { NewAge = 21 };
            EM.ApplyEntityMod(p, c);

            q = new PersonAgeQuery();
            EM.Query(p, q);
            Assert.IsTrue(q.Age == 21);

            Debug.Log("Projecting all events until end:");
            IProjection proj = new PersonProjection(Colors.cyan);
            ES.ApplyProjection(proj, EventStream.AllExistingEvents);

            Debug.Log("Projecting new events");
            IProjection proj2 = new PersonProjection(Colors.blue);
            ES.ApplyProjection(proj2, EventStream.NewEvents);

            Debug.Log("Applying age change to 22");
            c = new ChangePersonAgeMod { NewAge = 22 };
            EM.ApplyEntityMod(pkey, c);
            
            Debug.Log("Applying age change to 23");
            c = new ChangePersonAgeMod { NewAge = 23 };
            EM.ApplyEntityMod(p, c);

            Debug.Log("Applying age change to 24");
            c = new ChangePersonAgeMod { NewAge = 24 };
            EM.ApplyEntityMod(p, c);

            Debug.Log("Undoing 2");
            ES.Undo(2);

            Debug.Log("Projecting all events until end:");
            proj = new PersonProjection(Colors.cyan);
            ES.ApplyProjection(proj, EventStream.AllExistingEvents);
            
            Debug.Log("Serialising...");
            ES.ExtractByteData(bx => {
                Debug.Log("Serialised to " + bx.Length + " bytes");

                Debug.Log("Deserialising...");
                ES.ResetWithByteData(bx);

                Debug.Log("Projecting all events until end:");
                proj = new PersonProjection(Colors.green);
                ES.ApplyProjection(proj, EventStream.AllExistingEvents);

            });

        }

    }

}
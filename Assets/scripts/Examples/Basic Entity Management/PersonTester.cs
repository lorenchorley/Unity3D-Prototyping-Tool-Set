using UnityEngine;
using System.Collections;

namespace eventsourcing.examples.basic {

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

            PersonAgeQuery q = new PersonAgeQuery();
            EM.Query(p, q);
            Debug.Log("Age: " + q.Age + " Expecting 0 - new person");

            Debug.Log("Applying age change to 20");
            ChangePersonAgeMod c = new ChangePersonAgeMod { NewAge = 20 };
            EM.Mod(personUID, r, c);

            q = new PersonAgeQuery();
            EM.Query(personUID, r, q);
            Debug.Log("Age: " + q.Age + " Expecting 20");

            Debug.Log("Applying age change to 21");
            c = new ChangePersonAgeMod { NewAge = 21 };
            EM.Mod(p, c);

            q = new PersonAgeQuery();
            EM.Query(p, q);
            Debug.Log("Age: " + q.Age + " Expecting 21");

            Debug.Log("Projecting all events until end:");
            IProjection proj = new PersonProjection(Colors.cyan);
            ES.ApplyProjection(proj, EventStream.AllExistingEvents);

            Debug.Log("Projecting new events");
            IProjection proj2 = new PersonProjection(Colors.blue);
            ES.ApplyProjection(proj2, EventStream.NewEvents);

            Debug.Log("Applying age change to 22");
            c = new ChangePersonAgeMod { NewAge = 22 };
            EM.Mod(p, c);
            
            Debug.Log("Applying age change to 23");
            c = new ChangePersonAgeMod { NewAge = 23 };
            EM.Mod(p, c);

            Debug.Log("Applying age change to 24");
            c = new ChangePersonAgeMod { NewAge = 24 };
            EM.Mod(p, c);

            Debug.Log("Undoing 2");
            ES.Undo(2);


            Debug.Log("Projecting all events until end:");
             proj = new PersonProjection(Colors.cyan);
            ES.ApplyProjection(proj, EventStream.AllExistingEvents);


        }

    }

}
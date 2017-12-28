using UnityEngine;
using System.Collections;

namespace eventsourcing.examples.basic {

    public class PersonTester : MonoBehaviour {

        private EventSource ES;
        private EntityManager EM;

        void Start() {
            ES = GetComponent<EventSource>() ?? gameObject.AddComponent<EventSource>();
            EM = GetComponent<EntityManager>() ?? gameObject.AddComponent<EntityManager>();
            PersonRegistry r = new PersonRegistry(EM, 10);

            int personUID = r.NewEntity().UID;
            PersonEntity p = r.GetEntityByUID(personUID);

            PersonAgeQuery q = new PersonAgeQuery();
            EM.Query(p, q);
            Debug.Log("Age: " + q.Age);

            ChangePersonAgeMod c = new ChangePersonAgeMod { NewAge = 20 };
            EM.Mod(personUID, r, c);

            q = new PersonAgeQuery();
            EM.Query(personUID, r, q);
            Debug.Log("Age: " + q.Age);

            c = new ChangePersonAgeMod { NewAge = 21 };
            EM.Mod(p, c);

            q = new PersonAgeQuery();
            EM.Query(p, q);
            Debug.Log("Age: " + q.Age);

            IProjection proj = new PersonProjection(Colors.cyan);
            ES.ApplyProjection(proj);

            c = new ChangePersonAgeMod { NewAge = 22 };
            EM.Mod(p, c);

            proj = new PersonProjection(Colors.blue);
            ES.ApplyProjection(proj);

        }

    }

}
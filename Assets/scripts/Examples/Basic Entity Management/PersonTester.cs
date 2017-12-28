using UnityEngine;
using System.Collections;

namespace eventsourcing.examples.basic {

    [RequireComponent(typeof(EventSource))]
    public class PersonTester : MonoBehaviour {

        private EventSource ES;

        void Start() {
            ES = GetComponent<EventSource>();
            PersonRegistry r = new PersonRegistry(ES);

            int personUID = r.NewEntity().UID;
            PersonEntity p = r.GetEntityByUID(personUID);

            PersonAgeQuery q = new PersonAgeQuery();
            ES.Query(p, q);
            Debug.Log("Age: " + q.Age);

            ChangePersonAgeCommand c = new ChangePersonAgeCommand { NewAge = 20 };
            ES.Command(personUID, r, c);

            q = new PersonAgeQuery();
            ES.Query(personUID, r, q);
            Debug.Log("Age: " + q.Age);

            c = new ChangePersonAgeCommand { NewAge = 21 };
            ES.Command(p, c);

            q = new PersonAgeQuery();
            ES.Query(p, q);
            Debug.Log("Age: " + q.Age);

            IProjection proj = new PersonProjection(Colors.cyan);
            ES.ApplyProjection(proj);

            c = new ChangePersonAgeCommand { NewAge = 22 };
            ES.Command(p, c);

            proj = new PersonProjection(Colors.blue);
            ES.ApplyProjection(proj);

        }

    }

}
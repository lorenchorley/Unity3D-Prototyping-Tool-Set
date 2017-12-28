using UnityEngine;
using System.Collections;

namespace eventsourcing.examples.basic {

    public class PersonProjection : IProjection {

        public Colors c;

        public PersonProjection(Colors c) {
            this.c = c;
        }

        public void Reset() {
        }

        public bool Process(IEvent e) {
            Debug.Log((e.ToString()).Colored(c));
            return true; // Keep going
        }

        public void OnFinish() {
        }
        
    }

}
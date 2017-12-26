using UnityEngine;
using System.Collections;

namespace eventsource.examples.network {

    public class HashProjection : IESProjection {

        private int hashcode;
        private EventSource ES;

        public HashProjection(EventSource ES) {
            this.ES = ES;
        }

        public void Reset() {
            hashcode = 0;
        }
        
        public bool Process(ESEvent e) {
            hashcode += e.GetHashCode();
            return true;
        }

        public void OnFinish() {
            hashcode = hashcode.GetHashCode();

            Debug.Log("Hash results: " + hashcode + " from " + ES.EventCount + " events");

        }

        public override int GetHashCode() {
            return hashcode;
        }

    }

}
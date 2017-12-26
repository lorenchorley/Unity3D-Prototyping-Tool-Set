using UnityEngine;
using System.Collections;

namespace eventsource.examples.basic {

    public class PersonProjection : IESProjection {

        public const int ChunkSize = 2;

        public Colors c;

        public PersonProjection(Colors c) {
            this.c = c;
        }

        public void Reset() {
            
        }

        public bool NextRequest(out int length) {
            length = ChunkSize;
            return true;
        }

        public bool Process(ESEvent e) {
            Debug.Log((e.ToString()).Colored(c));
            return true; // Keep going
        }

        public void OnFinish() {
        }
        
    }

}
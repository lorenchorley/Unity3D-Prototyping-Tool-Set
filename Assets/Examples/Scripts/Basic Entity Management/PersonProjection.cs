using UnityEngine;
using System.Collections;
using System;
using eventsourcing;

namespace entitymanagement.examples.basic {

    public class PersonProjection : IProjection {

        public Colors c;

        public IDisposable CancelToken { get; set; }

        public PersonProjection(Colors c) {
            this.c = c;
        }

        public void Reset() {
            // Nothing to do
        }

        public bool Process(IEvent e) {
            Debug.Log((e.ToString()).Colored(c));
            return true; // Keep going
        }

        public void OnFinish() {
            // Nothing to do
            Debug.Log("Finish".Colored(c));
        }
        
    }

}
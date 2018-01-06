using UnityEngine;
using System.Collections;
using System;

namespace eventsourcing {

    public class ForEachProjection : IProjection {

        //private EventSource ES;
        private Action<IEvent> callback;

        public IDisposable CancelToken { get; set; }

        public ForEachProjection(EventSource ES, Action<IEvent> callback) {
            //this.ES = ES;
            this.callback = callback;
        }

        public void Reset() {
        }
        
        public bool Process(IEvent e) {
            callback.Invoke(e);
            return true;
        }

        public void OnFinish() {
        }

    }

}
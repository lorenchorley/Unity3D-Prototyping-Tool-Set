using UnityEngine;
using System.Collections;
using System;

namespace eventsourcing {

    public interface IProjection {
        IDisposable CancelToken { get; set; }
        void Reset();
        bool Process(IEvent e);
        void OnFinish();
    }
    
}
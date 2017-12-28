using UnityEngine;
using System.Collections;

namespace eventsourcing {

    public interface IProjection {
        void Reset();
        bool Process(IEvent e);
        void OnFinish();
    }
    
}
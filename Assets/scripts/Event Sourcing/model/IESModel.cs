using UnityEngine;
using System.Collections;
using System;

namespace eventsourcing {

    public interface IESModel {
        EventSource ES { get; }
        void InitES();
    }

}
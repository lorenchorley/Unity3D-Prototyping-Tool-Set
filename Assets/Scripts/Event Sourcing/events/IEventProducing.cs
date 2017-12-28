using UnityEngine;
using System.Collections;
using System;
using ZeroFormatter;
using eventsourcing.examples.network;

namespace eventsourcing {

    public interface IEventProducing {
        
        IEvent Event { get; }
        bool RecordEvent { get; set; }

    }

}
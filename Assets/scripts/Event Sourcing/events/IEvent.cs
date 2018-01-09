using UnityEngine;
using System.Collections;
using System;
using ZeroFormatter;
using eventsourcing.examples.network;

namespace eventsourcing {

    public interface IEvent {

        long CreationTime { get; } // In ticks
        int GenerateHashCode();

    }

}
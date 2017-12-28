using UnityEngine;
using System.Collections;
using System;
using ZeroFormatter;
using eventsourcing.examples.network;

namespace eventsourcing {

    public interface IEventCreator {

        IEvent Event { get; }

    }

}
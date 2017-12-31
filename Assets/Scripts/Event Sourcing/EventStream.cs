using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using UniRx;
using ZeroFormatter;

namespace eventsourcing {

    public enum EventStream {
        NewEvents,
        AllExistingEvents,
        ActionableEvents,
        NewActionableEvents,
        NonActionableEvents,
        NewNonActionableEvents,
    }

}
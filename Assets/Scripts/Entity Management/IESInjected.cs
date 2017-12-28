using UnityEngine;
using System.Collections;
using System;

namespace eventsourcing {

    public interface IESInjected {

        EventSource ES { set; }

    }

}
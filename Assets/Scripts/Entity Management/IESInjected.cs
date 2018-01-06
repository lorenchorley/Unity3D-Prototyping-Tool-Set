using UnityEngine;
using System.Collections;
using System;
using eventsourcing;

namespace entitymanagement {

    public interface IESInjected {

        EventSource ES { set; }

    }

}
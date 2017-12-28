using UnityEngine;
using System.Collections;
using System;

namespace eventsourcing {

    public interface IEMInjected {

        EntityManager EM { set; }

    }

}
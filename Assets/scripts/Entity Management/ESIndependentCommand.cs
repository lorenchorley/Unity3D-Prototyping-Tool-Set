using UnityEngine;
using System.Collections;
using System;

namespace eventsourcing {

    [Serializable]
    public abstract class IndependentModifier : IModifier {

        public EventSource ES;

        public abstract void Execute();

    }

}
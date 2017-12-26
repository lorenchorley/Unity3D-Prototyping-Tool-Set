using UnityEngine;
using System.Collections;
using System;

namespace eventsource {

    [Serializable]
    public abstract class ESIndependentCommand : ESCommand {

        public abstract ESEvent Execute(EventSource ES);

    }

}
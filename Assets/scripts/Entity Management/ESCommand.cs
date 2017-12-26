using UnityEngine;
using System.Collections;
using System;

namespace eventsource {

    [Serializable]
    public abstract class ESCommand {

        public bool Complete = false;

    }
}
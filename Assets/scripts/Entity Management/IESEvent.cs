using UnityEngine;
using System.Collections;
using System;
using ZeroFormatter;
using eventsource.examples.network;

namespace eventsource {

    [Serializable]
    [ZeroFormattable]
    public abstract class ESEvent {

        [NonSerialized]
        public bool registered = false;

        public abstract new int GetHashCode();

        // Do, Undo somehow
        public abstract ESCommand NewDoCommand();
        public abstract ESCommand NewUndoCommand();

    }

}
using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace eventsourcing {

    public class EventSourceResetWithNewData : Signal { }
    public class EventSourceSavedToFile : Signal { }

}
using UnityEngine;
using System;

namespace eventsource.examples.basic {

    [Serializable]
    public class ChangePersonAgeCommand : ESCommand {
        public int NewAge;
    }

}
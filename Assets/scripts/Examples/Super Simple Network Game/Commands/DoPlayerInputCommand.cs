using UnityEngine;
using System;

namespace eventsource.examples.network {

    [Serializable]
    public class DoPlayerInputCommand : ESCommand {
        public Direction direction;
    }

}
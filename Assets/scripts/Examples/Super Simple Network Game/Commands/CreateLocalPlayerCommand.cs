using UnityEngine;
using System;

namespace eventsource.examples.network {

    [Serializable]
    public class CreateLocalPlayerCommand : ESIndependentCommand {

        public override ESEvent Execute(EventSource ES) {
            return PlayerEntity.ESUSEONLYCOMMAND(ES, this);
        }

    }

}
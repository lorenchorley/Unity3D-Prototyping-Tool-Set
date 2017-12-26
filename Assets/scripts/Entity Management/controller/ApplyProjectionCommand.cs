using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace eventsource.examples {

    public class ApplyProjectionCommand : Command {

        [Inject] public EventSource ES { get; set; }
        [Inject] public IESProjection Projection { get; set; }

        public override void Execute() {
            ES.ApplyProjection(Projection);
        }

    }

}
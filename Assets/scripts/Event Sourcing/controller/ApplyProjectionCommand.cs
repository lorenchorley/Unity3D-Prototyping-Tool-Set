using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

namespace eventsourcing.examples {

    public class ApplyProjectionCommand : Command {

        [Inject] public EventSource ES { get; set; }
        [Inject] public IProjection Projection { get; set; }

        public override void Execute() {
            //ES.ApplyProjection(Projection);
        }

    }

}
using UnityEngine;
using System;

namespace eventsourcing.examples.network {

    [Serializable]
    public class CreateLocalPlayerCommand : IndependentModifier, IEventCreator {

        private IEvent _Event;
        public IEvent Event { get { return _Event; } }

        public override void Execute() {
            _Event = PlayerEntity.ESUSEONLYCOMMAND(ES, this);
        }

    }

}
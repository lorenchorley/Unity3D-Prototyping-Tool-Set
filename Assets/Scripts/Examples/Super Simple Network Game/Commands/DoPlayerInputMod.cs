using UnityEngine;
using System;

namespace eventsourcing.examples.network {

    [Serializable]
    public class DoPlayerInputMod : IModifier, IEventProducing {

        private IEvent _Event;
        public IEvent Event { get { return _Event; } }

        public bool RecordEvent { get; set; }

        public PlayerEntity player;
        public Direction direction;

        public void Execute() {
            // Ignored
        }

    }

}
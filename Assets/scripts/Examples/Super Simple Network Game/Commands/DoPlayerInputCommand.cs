using UnityEngine;
using System;

namespace eventsourcing.examples.network {

    [Serializable]
    public class DoPlayerInputCommand : IModifier, IEventCreator {

        private IEvent _Event;
        public IEvent Event { get { return _Event; } }

        public Direction direction;

        public void Execute() {
            throw new NotImplementedException();
        }

    }

}
using UnityEngine;
using System;
using entitymanagement;
using FullSerializer;

namespace eventsourcing.examples.network {

    [Serializable]
    public class DisablePlayerMod : IEntityModifier, IEventProducing {

        private IEvent _Event;
        [fsIgnore] public IEvent Event => _Event;
        [fsIgnore] public bool DontRecordEvent { get; set; }

        [fsIgnore] public IEntity e { get; set; }
        [fsIgnore] public Type IntendedEntityType => typeof(PlayerEntity);

        public long CreationTime { get; set; }

        public void Execute() {
            _Event = (e as PlayerEntity).ApplyMod(this);
        }

    }

}
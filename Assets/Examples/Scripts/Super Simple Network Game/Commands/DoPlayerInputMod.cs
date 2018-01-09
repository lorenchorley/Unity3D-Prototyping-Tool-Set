using UnityEngine;
using System;
using entitymanagement;
using UnityEngine.Assertions;
using FullSerializer;

namespace eventsourcing.examples.network {

    [Serializable]
    public class DoPlayerInputMod : IEntityModifier, IEventProducing {

        public Direction direction;

        private IEvent _Event;
        [fsIgnore] public IEvent Event => _Event;
        [fsIgnore] public bool DontRecordEvent { get; set; }

        [fsIgnore] public IEntity e { get; set; }
        [fsIgnore] public Type IntendedEntityType => typeof(PlayerEntity);

        public long CreationTime { get; set; }

        public void Execute() {
            Assert.IsNotNull(e);
            // TODO Will not work with empty entity! gettype will return typeof(EmptyEntity), not the intended struct type
            // Put bool in registry instead

            // TODO Remove
            PlayerEntity f = (e.Key.Registry as PlayerRegistry).GetEntityByKey(e.Key);
            Assert.IsTrue(e == f);

            _Event = (e as PlayerEntity).ApplyMod(this);
        }

    }

}
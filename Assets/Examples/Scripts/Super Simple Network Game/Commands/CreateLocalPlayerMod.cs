using UnityEngine;
using System;
using entitymanagement;
using FullSerializer;

namespace eventsourcing.examples.network {

    [Serializable]
    public class CreateLocalPlayerMod : IEntityModifier, IEventProducing, IEMInjected {

        private IEvent _Event;
        [fsIgnore] public IEvent Event { get { return _Event; } }

        private EntityManager _EM;
        public EntityManager EM { set { _EM = value; } }

        [fsIgnore] public bool DontRecordEvent { get; set; }

        [fsIgnore] public IEntity e { get; set; }

        public void Execute() {
            _Event = PlayerEntity.ApplyMod(_EM, this);
        }

    }

}
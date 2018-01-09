using UnityEngine;
using System;
using entitymanagement;
using FullSerializer;
using UnityEngine.Assertions;

namespace eventsourcing.examples.network {

    [Serializable]
    public class CreatePlayerMod : IIndependentModifier, IEventProducing, IEMInjected {

        public int PlayerPhotonID;

        private EntityManager _EM;
        public EntityManager EM { set { _EM = value; } }

        private IEvent _Event;
        [fsIgnore] public IEvent Event => _Event;
        [fsIgnore] public bool DontRecordEvent { get; set; }

        public long CreationTime { get; set; }

        public void Execute() {
            _Event = PlayerEntity.ApplyMod(_EM, this);
        }

    }

}
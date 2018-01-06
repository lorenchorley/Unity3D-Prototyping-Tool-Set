using UnityEngine;
using System;
using entitymanagement;

namespace eventsourcing.examples.network {

    [Serializable]
    public class DoPlayerInputMod : IEntityModifier {

        public PlayerEntity player;
        public Direction direction;

        public IEntity e { get; set; }

        public void Execute() {
            // Ignored
        }

    }

}
using UnityEngine;
using System;

namespace eventsourcing.examples.network {

    [Serializable]
    public class DoPlayerInputMod : IModifier {

        public PlayerEntity player;
        public Direction direction;

        public void Execute() {
            // Ignored
        }

    }

}
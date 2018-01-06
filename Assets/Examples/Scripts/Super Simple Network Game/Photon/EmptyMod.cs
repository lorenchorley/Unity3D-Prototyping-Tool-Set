using UnityEngine;
using System;
using entitymanagement;

namespace eventsourcing.examples.network {

    [Serializable]
    public class EmptyMod : IEntityModifier {
        public IEntity e { get; set; }

        public void Execute() {
            // Ignored
        }

    }

}
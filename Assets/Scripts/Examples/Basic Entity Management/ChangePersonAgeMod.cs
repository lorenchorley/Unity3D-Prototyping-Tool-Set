using UnityEngine;
using System;

namespace eventsourcing.examples.basic {

    [Serializable]
    public class ChangePersonAgeMod : IModifier {
        public int NewAge;

        public void Execute() {
            throw new NotImplementedException();
        }

    }

}
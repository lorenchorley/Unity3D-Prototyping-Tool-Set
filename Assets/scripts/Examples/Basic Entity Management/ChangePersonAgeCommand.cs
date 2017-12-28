using UnityEngine;
using System;

namespace eventsourcing.examples.basic {

    [Serializable]
    public class ChangePersonAgeCommand : IModifier {
        public int NewAge;

        public void Execute() {
            throw new NotImplementedException();
        }

    }

}
using System;
using entitymanagement;
using eventsourcing;

namespace photon.essynchronisation {

    // TODO Remove if not used
    [Serializable]
    public class EmptyMod : IIndependentModifier {

        public long CreationTime { get; set; }

        public void Execute() {
        }

    }

}
using System;
using entitymanagement;
using eventsourcing;

namespace photon.essynchronisation {

    [Serializable]
    public class UnpauseMod : IIoCInjected, IIndependentModifier, IEventProducing {

        [Inject] public OnUnpauseSignal OnUnpauseSignal { get; set; }

        private IEvent _Event;
        public IEvent Event { get { return _Event; } }
        public bool DontRecordEvent { get; set; }

        public long CreationTime { get; set; }

        public void Execute() {
            _Event = new UnpauseEvent();

            PUNGamePauser.Unpause();

            if (OnUnpauseSignal != null)
                OnUnpauseSignal.Dispatch();

        }

    }

}
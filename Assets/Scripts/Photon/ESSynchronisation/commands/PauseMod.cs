using System;
using entitymanagement;
using eventsourcing;

namespace photon.essynchronisation {

    [Serializable]
    public class PauseMod : IIoCInjected, IIndependentModifier, IEventProducing {

        [Inject] public OnPauseSignal OnPauseSignal { get; set; }

        private IEvent _Event;
        public IEvent Event { get { return _Event; } }
        public bool DontRecordEvent { get; set; }

        public long CreationTime { get; set; }

        public void Execute() {
            _Event = new PauseEvent();

            PUNGamePauser.Pause();

            if (OnPauseSignal != null)
                OnPauseSignal.Dispatch();

        }

    }

}
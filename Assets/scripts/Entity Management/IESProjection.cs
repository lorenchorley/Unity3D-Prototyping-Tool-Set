using UnityEngine;
using System.Collections;

namespace eventsource {

    public interface IESProjection { // Must inherit from IESCommandable<All appropriate commands...>
        void Reset();
        bool Process(ESEvent e);
        void OnFinish();
    }

    //public interface IESProjectionAbsolute : IESProjection {
    //    bool NextRequest(out int startingEventID, out int length);
    //}

    //public interface IESProjectionRelativeContinuing : IESProjection {
    //    bool NextRequest(out int length);
    //}

}
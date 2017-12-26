using UnityEngine;
using System.Collections;

namespace eventsource {

    public interface IESEntity {

        EventSource ES { get; }
        int Index { get; set; }

    }

}
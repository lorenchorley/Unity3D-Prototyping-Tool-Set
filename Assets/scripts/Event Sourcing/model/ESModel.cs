using UnityEngine;
using System.Collections;
using System;

namespace eventsourcing {

    public class ESModel : IESModel {

        public EventSource ES { get; private set; }

        public void InitES() {
            if (ES == null)
                ES = new EventSource();
        }

    }

}
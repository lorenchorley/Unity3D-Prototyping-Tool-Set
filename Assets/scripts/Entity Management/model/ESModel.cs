using UnityEngine;
using System.Collections;
using System;

namespace eventsource {

    public class ESModel : IESModel {

        public EventSource ES { get; private set; }

        public void InitES() {
            if (ES == null)
                ES = new EventSource();
        }

        public void LoadESFrom(byte[] dataSource) {

        }

        public byte[] SerialiseES() {
            return null;
        }

        public void SynchroniseESToFile(string path) {

        }

    }

}
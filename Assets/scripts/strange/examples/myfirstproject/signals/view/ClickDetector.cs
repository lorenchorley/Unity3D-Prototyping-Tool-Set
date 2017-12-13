/// Just a simple MonoBehaviour Click Detector

using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strange.examples.myfirstproject.signals {
    public class ClickDetector : View {

        public Signal CLICK_EVENT;

        protected override void Awake() {
            base.Awake();
            CLICK_EVENT = new Signal();
        }

        void OnMouseDown() {
            CLICK_EVENT.Dispatch();
        }

    }
}


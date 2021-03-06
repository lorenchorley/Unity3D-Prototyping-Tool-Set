/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace strange.examples.multiplecontexts.signals.game {
    public class EnemySignalView : View {

        public Signal CLICK_EVENT;

        private float theta = 0f;
        private Vector3 basePosition;

        //Publicly settable from Unity3D
        public float edx_WobbleForce = .4f;
        public float edx_WobbleIncrement = .1f;

        internal void init() {
            CLICK_EVENT = new Signal();

            ClickDetector clicker = gameObject.GetComponent<ClickDetector>() ?? gameObject.AddComponent<ClickDetector>();
            clicker.CLICK_EVENT.AddListener(CLICK_EVENT.Dispatch);
        }

        internal void updatePosition() {
            wobble();
        }

        void wobble() {
            theta += edx_WobbleIncrement;
#pragma warning disable CS0618 // Type or member is obsolete
            gameObject.transform.RotateAround(Vector3.forward, edx_WobbleForce * Mathf.Sin(theta));
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}


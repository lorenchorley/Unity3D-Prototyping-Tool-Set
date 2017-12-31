using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LorenChorley.PostAvian.Helpers {

    public class TouchInputter : Inputter {

        private float zoomAmount;

        public float MinimumTimeBeforeNextClickAllowed = 0.1f;
        public float LastClickTime;
        public Vector2 LastClickPlace;

        public TouchInputter() {
            type = "touch: ";
            LastClickTime = Time.time;
            LastClickPlace = -10000000 * Vector2.one;
        }

        protected override Vector2 GetScreenPosition() {
            Assert.IsTrue(Input.touchCount > 0, "GetScreenPosition");

            return Input.touches[0].position;
        }

        protected override bool CanTriggerPrimaryAction() {
            if (Time.time - LastClickTime > MinimumTimeBeforeNextClickAllowed && (LastClickPlace - LastDragPosition).magnitude >= DragThresholdInPixels) {
                LastClickTime = Time.time;
                LastClickPlace = LastDragPosition;
                return true;
            } else {
                LastClickPlace = LastDragPosition;
                return false;
            }
        }

        protected override bool CanStartClickOrTap() {
            return Input.touchCount == 1;
        }

        protected override bool CanEndClickOrTap() {
            return Input.touchCount == 0;
        }

        protected override bool CanStartDrag() {
            return Input.touchCount == 1;
        }

        protected override bool CanEndDrag() {
            return Input.touchCount == 0;
        }

        protected override bool CanStartZoom() {
            return Input.touchCount >= 2;
        }

        protected override bool CanMaintainZoomWithoutZooming() {
            return Input.touchCount == 1;
        }

        protected override bool CanEndZoom() {
            return Input.touchCount == 0;
        }

        protected override float GetZoomDelta() {
            Assert.IsFalse(Input.touchCount <= 1, "GetZoomDelta");

            float newAmount = (Input.touches[0].position - Input.touches[1].position).magnitude;
            float delta = newAmount - zoomAmount;
            zoomAmount = newAmount;

            return delta;
        }

        protected override void RegisterStartOfZoom() {
            Assert.IsTrue(Input.touchCount > 1, "RegisterStartOfZoom");

            zoomAmount = (Input.touches[0].position - Input.touches[1].position).magnitude;

        }

        protected override bool CanZoomAndDragSimultaneously() {
            return false;
        }

    }

}
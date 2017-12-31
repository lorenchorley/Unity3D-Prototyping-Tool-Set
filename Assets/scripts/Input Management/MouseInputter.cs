using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LorenChorley.PostAvian.Helpers {

    public class MouseInputter : Inputter {

        public MouseInputter() {
            type = "mouse: ";
        }

        protected override Vector2 GetScreenPosition() {
            return Input.mousePosition;
        }

        protected override bool CanTriggerPrimaryAction() {
            return true;
        }

        protected override bool CanEndClickOrTap() {
            return Input.GetMouseButtonUp(0);
        }

        protected override bool CanStartClickOrTap() {
            return Input.GetMouseButtonDown(0);
        }

        protected override bool CanEndDrag() {
            return Input.GetMouseButtonUp(0);
        }

        protected override bool CanStartDrag() {
            return Input.GetMouseButtonDown(0);
        }

        protected override bool CanStartZoom() {
            return Input.mouseScrollDelta != Vector2.zero;
        }

        protected override bool CanEndZoom() {
            return Input.mouseScrollDelta == Vector2.zero;
        }

        protected override float GetZoomDelta() {
            return Input.mouseScrollDelta.y;
        }

        protected override void RegisterStartOfZoom() { }

        protected override bool CanMaintainZoomWithoutZooming() {
            return false;
        }

        protected override bool CanZoomAndDragSimultaneously() {
            return true;
        }

    }

}
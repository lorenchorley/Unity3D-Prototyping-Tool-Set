using strange.extensions.dispatcher.eventdispatcher.api;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace menusystem.example {

    public class AwesomeMenuMeditator : MenuMediator<AwesomeMenu> {

        protected AwesomeMenuMeditator() {
        }

        protected override void Init() {
        }

        protected override void UpdateListeners(bool value) {
            //dispatcher.UpdateListener(MenuEvent.OpenMenu, OnOpen);
        }

        private void OnOpen(IEvent evt) {
            if (typeof(AwesomeMenu) == (Type) evt.data) {
                //view.Show();
            }
        }

    }
}
using strange.extensions.dispatcher.eventdispatcher.api;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace menusystem.example {

    public class AwesomeMenuMeditator : MenuMediator<AwesomeMenu> {

        [Inject] public IMenuModel MenuModel { get; set; }
        [Inject] public OpenMenuSignal OpenMenuSignal { get; set; }
        [Inject] public MenuBackSignal MenuBackSignal { get; set; }

        protected override void Init() {
        }

        protected override void UpdateListeners(bool value) {
            view.BackPressedSignal.UpdateListener(value, MenuBackSignal.Dispatch);
            view.BackButton.UpdateListener(value, MenuBackSignal.Dispatch);
        }
        
    }
}
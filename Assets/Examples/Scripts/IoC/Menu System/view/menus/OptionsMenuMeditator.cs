using UnityEngine;
using UnityEngine.UI;

namespace menusystem.example {

    public class OptionsMenuMeditator : MenuMediator<OptionsMenu> {

        [Inject] public IMenuModel MenuModel { get; set; }
        [Inject] public OpenMenuSignal OpenMenuSignal { get; set; }
        [Inject] public MenuBackSignal MenuBackSignal { get; set; }

        protected override void Init() {
        }

        protected override void UpdateListeners(bool value) {
            view.BackPressedSignal.UpdateListener(value, MenuBackSignal.Dispatch);
            view.BackButton.UpdateListener(value, MenuBackSignal.Dispatch);
            view.MagicButton.UpdateListener(value, OnMagicButtonPressed);
        }

        void OnMagicButtonPressed() {
            MenuModel.OpenMenu<AwesomeMenu>(m => m.Show(view.Slider.value));
        }

    }
}
using UnityEngine;
using UnityEngine.UI;

namespace menusystem.example {

    public class MainMenuMeditator : MenuMediator<MainMenu> {

        [Inject] public IMenuModel MenuModel { get; set; }
        [Inject] public OpenMenuSignal OpenMenuSignal { get; set; }
        [Inject] public MenuBackSignal MenuBackSignal { get; set; }

        protected override void Init() {
        }

        protected override void UpdateListeners(bool value) {
            view.OptionsButton.UpdateListener(value, OptionsButtonPressed);
            view.PlayButton.UpdateListener(value, PlayButtonPressed);
            view.BackPressedSignal.UpdateListener(value, MenuBackSignal.Dispatch);
        }

        void OptionsButtonPressed() {
            MenuModel.OpenMenu<OptionsMenu>();
        }

        void PlayButtonPressed() {
            MenuModel.OpenMenu<GameMenu>();
        }

    }

}
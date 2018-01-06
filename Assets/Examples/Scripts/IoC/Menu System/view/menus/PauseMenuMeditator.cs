using UnityEngine;
using UnityEngine.UI;

namespace menusystem.example {

    public class PauseMenuMeditator : MenuMediator<PauseMenu> {

        [Inject] public IMenuModel MenuModel { get; set; }
        [Inject] public OpenMenuSignal OpenMenuSignal { get; set; }
        [Inject] public MenuBackSignal MenuBackSignal { get; set; }

        protected override void Init() {
        }

        protected override void UpdateListeners(bool value) {
            view.QuitButton.UpdateListener(value, OnQuitPressed);
            view.ResumeButton.UpdateListener(value, MenuBackSignal.Dispatch);
        }

        void OnQuitPressed() {
            view.Hide();
            Destroy(view.gameObject); // This menu does not automatically destroy itself

            //GameMenu.Hide();
        }

    }
}
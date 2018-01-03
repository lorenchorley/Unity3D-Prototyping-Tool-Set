using UnityEngine.UI;

namespace menusystem.example {

    public class OptionsMenu : SimpleMenu<OptionsMenu> {
        public Slider Slider;

        public override void Init() {
        }

        public void OnMagicButtonPressed() {
            //AwesomeMenu.Show(Slider.value);
        }
    }

}
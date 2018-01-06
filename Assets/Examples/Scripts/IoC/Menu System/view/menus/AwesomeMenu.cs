using UnityEngine;
using UnityEngine.UI;

namespace menusystem.example {

    public class AwesomeMenu : MenuView<AwesomeMenu> {

        public Image Background;
        public Text Title;
        public Button BackButton;

        public override void Init() {
        }

        public void Show(float awesomeness) {
            Background.color = new Color32((byte) (129 * awesomeness), (byte) (197 * awesomeness), (byte) (34 * awesomeness), 255);
            Title.text = string.Format("This menu is {0:P} awesome", awesomeness);
        }

    }
}
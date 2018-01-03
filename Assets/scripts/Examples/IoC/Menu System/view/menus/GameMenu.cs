namespace menusystem.example {

    public class GameMenu : SimpleMenu<GameMenu> {
        public override void Init() {
        }

        public override void OnBackPressed() {
            PauseMenu.Show();
        }
    }

}
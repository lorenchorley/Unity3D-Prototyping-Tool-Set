namespace menusystem.example {

    public class PauseMenu : SimpleMenu<PauseMenu> {

        public override void Init() {
        }

        public void OnQuitPressed() {
            Hide();
            Destroy(this.gameObject); // This menu does not automatically destroy itself

            GameMenu.Hide();
        }

    }
}
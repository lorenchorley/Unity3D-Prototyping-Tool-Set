using strange.extensions.command.impl;

namespace menusystem.example {

    public class StartExampleCommand : Command {

        [Inject] public IMenuModel MenuModel { get; set; }

        public override void Execute() {
            MenuModel.OpenMenu<MainMenu>();
        }
        
    }

}


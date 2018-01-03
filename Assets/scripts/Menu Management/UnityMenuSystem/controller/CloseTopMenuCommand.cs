using strange.extensions.command.impl;

namespace menusystem {

    public class CloseTopMenuCommand : Command {

        [Inject] public IMenuModel MenuModel { get; set; }

        public override void Execute() {
            MenuModel.CloseTopMenu();
        }

    }

}


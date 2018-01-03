using strange.extensions.command.impl;

namespace menusystem {

    public class BackMenuCommand : Command {

        [Inject] public IMenuModel MenuModel { get; set; }

        public override void Execute() {
            if (MenuModel.IsMenuOpen()) {
                MenuModel.GetTopMenu().OnBackPressed();
            }
        }
        
    }

}


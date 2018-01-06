using strange.extensions.command.impl;
using UnityEngine;

namespace menusystem {

    public class BackMenuCommand : Command {

        [Inject] public IMenuModel MenuModel { get; set; }

        public override void Execute() {
            if (MenuModel.IsMenuOpen()) {
                MenuModel.CloseTopMenu();
            }
        }

    }

}


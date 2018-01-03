using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace menusystem {

    public class CloseMenuCommand : Command {

        [Inject] public IMenuModel MenuModel { get; set; }

        public override void Execute() {
        }

        public void CloseMenu(MenuView menu) {
            
            if (!MenuModel.IsMenuOpen()) {
                Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
                return;
            }
            
            if (MenuModel.GetTopMenu() != menu) {
                Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
                return;
            }

            MenuModel.CloseTopMenu();
        }

    }

}


using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace menusystem {

    public class StartMenuCommand : Command {

        [Inject] public IMenuModel MenuModel { get; set; }

        public override void Execute() {
            MenuModel.Init();
            MenuModel.OnMenuReadySignal.Dispatch();
        }

    }

}


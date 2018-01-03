using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.command.impl;
using System;
using menusystem;

namespace unitymenusystem {

    public class OpenMenuCommand : Command {

        [Inject] public Type MenuType { get; set; }
        [Inject] public IMenuModel MenuModel { get; set; }

        public override void Execute() {
            MenuModel.OpenMenu(MenuType);
        }

    }

}


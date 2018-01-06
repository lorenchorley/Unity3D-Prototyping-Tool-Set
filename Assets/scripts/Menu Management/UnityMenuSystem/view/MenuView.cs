using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace menusystem {

    public abstract class MenuView : EventView {

        public abstract void Init();

        public StrictSignal BackPressedSignal;

        [Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
        public bool DestroyWhenClosed = true;

        [Tooltip("Disable menus that are under this one in the stack")]
        public bool DisableMenusUnderneath = true;

    }

    public abstract class MenuView<T> : MenuView where T : MenuView<T> {
        
    }

}


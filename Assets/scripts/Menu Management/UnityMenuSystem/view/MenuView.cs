using System.Collections;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace menusystem {

    public abstract class MenuView : EventView {

        public abstract void Init();
        public abstract void OnBackPressed();

        [Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
        public bool DestroyWhenClosed = true;

        [Tooltip("Disable menus that are under this one in the stack")]
        public bool DisableMenusUnderneath = true;

    }

    public abstract class MenuView<T> : MenuView where T : MenuView<T> {

        public static T Instance { get; private set; }

        public void UpdateStaticInstance() {
            Instance = (T) this;
        }

        protected override void OnDestroy() {
            Instance = null;
        }

        protected static void Open() {
            //if (Instance == null)
            //    MenuManager.Instance.CreateInstance<T>();
            //else
            //    Instance.gameObject.SetActive(true);

            //MenuManager.Instance.OpenMenu(Instance);
        }

        protected static void Close() {
            if (Instance == null) {
                Debug.LogErrorFormat("Trying to close menu {0} but Instance is null", typeof(T));
                return;
            }

            //MenuManager.Instance.CloseMenu(Instance);
        }

        public override void OnBackPressed() {
            Close();
        }
    }

}


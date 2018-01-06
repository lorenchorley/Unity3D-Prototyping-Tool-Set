using strange.extensions.context.api;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace menusystem {

    public class MenuModel : IMenuModel {

        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject contextView { get; set; }
        
        private MenuBinder menuBinder;

        private Stack<MenuView> menuStack = new Stack<MenuView>();

        [Inject] public MenuSystemReadySignal OnMenuReadySignal { get; set; }
        [Inject] public OpenMenuSignal OpenMenuSignal { get; set; }
        [Inject] public CloseMenuSignal CloseMenuSignal { get; set; }
        [Inject] public CloseTopMenuSignal CloseTopMenuSignal { get; set; }
        [Inject] public MenuBackSignal MenuBackSignal { get; set; }

        public void Init() {
            menuStack = new Stack<MenuView>();

            menuBinder = new MenuBinder();
            menuBinder.Context = contextView;
            menuBinder.SetBinderName("Menu Binder");

            MenuManager manager = GameObject.FindObjectOfType<MenuManager>();

            if (manager == null)
                throw new Exception("Could not find MenuManager in scene");

            MenuOptions options = manager.Options;

            for (int i = 0; i < options.MenuPrefabs.Length; i++) {
                GameObject prefab = options.MenuPrefabs[i];
                MenuView menuView = prefab.GetComponent<MenuView>();

                if (menuView == null)
                    throw new Exception("TODO");

                Type menuViewType = menuView.GetType();

                menuBinder.Bind(menuViewType).To(prefab).ToName("Prefab");

            }

        }

        public void OpenMenu<T>(Action<T> menuReady) where T : MenuView {
            OpenMenu(typeof(T), t => menuReady((T) t));
        }

        public void OpenMenu(Type type, Action<MenuView> menuReady) {
            MenuView instance = menuBinder.GetInstance(type);

            if (instance == null)
                throw new Exception("Menu prefab not registered: " + type.Name);

            // De-activate top menu
            if (menuStack.Count > 0) {
                if (instance.DisableMenusUnderneath) {
                    foreach (var menu in menuStack) {
                        menu.gameObject.SetActive(false);

                        if (menu.DisableMenusUnderneath)
                            break;
                    }
                }

                var topCanvas = instance.GetComponent<Canvas>();
                var previousCanvas = menuStack.Peek().GetComponent<Canvas>();
                topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;
            }

            menuStack.Push(instance);

            if (menuReady != null)
                menuReady.Invoke(instance); // TODO Make sure this is done after OnRegister
        }

        public void OpenMenu(Type type) {
            OpenMenu(type, null);
        }

        public void OpenMenu<T>() where T : MenuView {
            OpenMenu(typeof(T), null);
        }

        public void CloseTopMenu() {
            MenuView instance = menuStack.Pop();

            if (instance.DestroyWhenClosed) { 
                GameObject.Destroy(instance.gameObject);
                menuBinder.Unbind(instance.GetType(), "Instance");
            }  else
                instance.gameObject.SetActive(false);

            // Re-activate top menu
            // If a re-activated menu is an overlay we need to activate the menu under it
            foreach (var menu in menuStack) {
                menu.gameObject.SetActive(true);

                if (menu.DisableMenusUnderneath)
                    break;
            }
        }

        public bool IsMenuOpen() {
            return menuStack.Count > 0;
        }

        public MenuView GetTopMenu() {
            return menuStack.Peek();
        }

    }

}


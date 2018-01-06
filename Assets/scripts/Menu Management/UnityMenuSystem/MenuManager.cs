using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;
using UnityEngine.Assertions;
using unitymenusystem;

namespace menusystem {

    public class MenuManager : MonoBehaviour {

        public static MenuManager Instance;

        public MenuOptions Options;

        void Awake() {
            if (Instance == null)
                Instance = this;

            Assert.IsNotNull(Options);
        }

        //public void MapEventBindingsOnto(MVCSContext c) {
        //    UsingSignals = false;

        //    c.injectionBinder.Bind<IMenuModel>().To<MenuModel>().ToSingleton();

        //    c.commandBinder.Bind(ContextEvent.START).To<StartMenuCommand>().Once();
        //    c.commandBinder.Bind(MenuEvent.OpenMenu).To<OpenMenuCommand>();
        //    c.commandBinder.Bind(MenuEvent.CloseMenu).To<CloseMenuCommand>();
        //    c.commandBinder.Bind(MenuEvent.CloseTopMenu).To<CloseTopMenuCommand>();
        //    c.commandBinder.Bind(MenuEvent.Back).To<BackMenuCommand>();

        //}

        public void MapBindingsOnto(MVCSContext c) {
            c.injectionBinder.Bind<IMenuModel>().To<MenuModel>().ToSingleton();

            c.commandBinder.Bind(ContextEvent.START).To<StartMenuCommand>().Once();
            c.commandBinder.Bind<OpenMenuSignal>().To<OpenMenuCommand>();
            c.commandBinder.Bind<CloseMenuSignal>().To<CloseMenuCommand>();
            c.commandBinder.Bind<CloseTopMenuSignal>().To<CloseTopMenuCommand>();
            c.commandBinder.Bind<MenuBackSignal>().To<BackMenuCommand>();

            c.injectionBinder.Rebind<MenuSystemReadySignal>().ToSingleton();

        }

    }

}


using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;
using unitymenusystem;

namespace menusystem.example {

    public class ExampleMenuContext : MVCSContext {

        public ExampleMenuContext() : base() {
        }

        public ExampleMenuContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup, true) {
        }

        protected override void mapBindings() {
            MenuManager manager = Object.FindObjectOfType<MenuManager>();
            manager.MapBindingsOnto(this);

            mediationBinder.Bind<AwesomeMenu>().To<AwesomeMenuMeditator>();
            mediationBinder.Bind<GameMenu>().To<GameMenuMeditator>();
            mediationBinder.Bind<MainMenu>().To<MainMenuMeditator>();
            mediationBinder.Bind<OptionsMenu>().To<OptionsMenuMeditator>();
            mediationBinder.Bind<PauseMenu>().To<PauseMenuMeditator>();

            commandBinder.Bind<ContextStartSignal>().To<StartMenuCommand>();
            commandBinder.Bind<OnMenuReadySignal>().To<StartExampleCommand>();

        }
    }

}


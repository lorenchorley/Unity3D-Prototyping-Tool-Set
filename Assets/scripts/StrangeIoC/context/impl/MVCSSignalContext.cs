
using UnityEngine;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using strange.extensions.sequencer.api;
using strange.extensions.sequencer.impl;
using strange.framework.api;
using strange.framework.impl;
using strange.extensions.injector.impl;

namespace strange.extensions.context.impl {

    public class MVCSSignalContext : MVCSContext {

        public MVCSSignalContext() {
        }

        public MVCSSignalContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup) {
        }

        // Unbind the default EventCommandBinder and rebind the SignalCommandBinder
        protected override void addCoreComponents() {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        // Override Start so that we can fire the StartSignal 
        override public IContext Start() {
            base.Start();
             
            ContextStartSignal startSignal = null;

            try { 
                startSignal = (ContextStartSignal) injectionBinder.GetInstance<ContextStartSignal>();
            } catch (InjectionException) { }

            if (startSignal != null)
                startSignal.Dispatch();

            return this;
        }
    }

}


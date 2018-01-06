/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

/// MainContext maps the Context for the top-level component.
/// ===========
/// I'm assuming here that you've already gone through myfirstproject, or that
/// you're experienced with strange.

using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.examples.multiplecontexts.signals.main;
using strange.extensions.command.impl;
using strange.extensions.command.api;

namespace strange.examples.multiplecontexts.signals.main {
    public class MainSignalContext : MVCSSignalContext {

        public MainSignalContext() {
        }

        public MainSignalContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup) {
        }

        // Unbind the default EventCommandBinder and rebind the SignalCommandBinder
        //protected override void addCoreComponents() {
        //    base.addCoreComponents();
        //    injectionBinder.Unbind<ICommandBinder>();
        //    injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        //}

        //// Override Start so that we can fire the StartSignal 
        //override public IContext Start() {
        //    base.Start();
        //    MAIN_START_SIGNAL startSignal = (MAIN_START_SIGNAL) injectionBinder.GetInstance<MAIN_START_SIGNAL>();
        //    startSignal.Dispatch();
        //    return this;
        //}

        protected override void mapBindings() {

            commandBinder.Bind<ContextStartSignal>().To<StartCommand>().Once();
            commandBinder.Bind<LOAD_SCENE>().To<LoadSceneCommand>();
            commandBinder.Bind<GAME_COMPLETE>().To<GameCompleteCommand>();
            commandBinder.Bind<MAIN_REMOVE_SOCIAL_CONTEXT>().To<ReceiveCompleteCommand>();

            injectionBinder.Bind<SCENE_LOADED>().ToSingleton();
            injectionBinder.Bind<FULFILL_SERVICE_REQUEST>().ToSingleton();
            //injectionBinder.Bind<MAIN_REMOVE_SOCIAL_CONTEXT>().ToSingleton().CrossContext();
            injectionBinder.Bind<REQUEST_WEB_SERVICE>().ToSingleton();

            BindSignalCrossContext<GAME_COMPLETE>();
            //injectionBinder.GetBinding<GAME_COMPLETE>().CrossContext();

            BindSignalCrossContext<MAIN_REMOVE_SOCIAL_CONTEXT>();
        }
    }
}


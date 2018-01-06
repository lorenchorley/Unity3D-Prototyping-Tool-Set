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
using strange.examples.multiplecontexts.signals.game;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.examples.multiplecontexts.signals.main;

namespace strange.examples.multiplecontexts.signals.game {
    public class GameSignalContext : MVCSSignalContext {

        public GameSignalContext() {
        }

        public GameSignalContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup) {
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
        //    GAME_START_SIGNAL startSignal = (GAME_START_SIGNAL) injectionBinder.GetInstance<GAME_START_SIGNAL>();
        //    startSignal.Dispatch();
        //    return this;
        //}

        protected override void mapBindings() {
            injectionBinder.Bind<IScore>().To<ScoreModel>().ToSingleton();

            mediationBinder.Bind<ShipSignalView>().To<ShipSignalMediator>();
            mediationBinder.Bind<EnemySignalView>().To<EnemySignalMediator>();
            mediationBinder.Bind<ScoreboardSignalView>().To<ScoreboardSignalMediator>();

            commandBinder.Bind<ContextStartSignal>().To<StartAppCommand>().To<StartGameCommand>().Once().InSequence();

            commandBinder.Bind<ADD_TO_SCORE>().To<UpdateScoreCommand>();
            commandBinder.Bind<SHIP_DESTROYED>().To<ShipDestroyedCommand>();
            commandBinder.Bind<GAME_OVER>().To<GameOverCommand>();
            commandBinder.Bind<REPLAY>().To<ReplayGameCommand>();
            commandBinder.Bind<GAME_REMOVE_SOCIAL_CONTEXT>().To<RemoveSocialContextCommand>();

            injectionBinder.Bind<GAME_UPDATE>().ToSingleton();
            injectionBinder.Bind<LIVES_CHANGE>().ToSingleton();
            injectionBinder.Bind<RESTART_GAME>().ToSingleton().CrossContext();
            injectionBinder.Bind<SCORE_CHANGE>().ToSingleton();

            BindSignalCrossContext<MAIN_REMOVE_SOCIAL_CONTEXT>();
        }
    }
}


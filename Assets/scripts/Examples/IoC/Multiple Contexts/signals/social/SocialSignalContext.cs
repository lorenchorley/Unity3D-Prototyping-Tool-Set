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

/// SocialContext maps the Context for the social interactivity component.
/// ===========
/// A key thing to notice here is how easily we can swap one service (or model, or whatever)
/// for another that satisfies the same interface.

using System;
using UnityEngine;
using strange.examples.multiplecontexts.signals.main;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.examples.multiplecontexts.signals.game;

namespace strange.examples.multiplecontexts.signals.social {
    public class SocialSignalContext : MVCSContext {

        public SocialSignalContext() {
        }

        public SocialSignalContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup) {
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
            SOCIAL_START_SIGNAL startSignal = (SOCIAL_START_SIGNAL) injectionBinder.GetInstance<SOCIAL_START_SIGNAL>();
            startSignal.Dispatch();
            return this;
        }

        protected override void mapBindings() {
            commandBinder.Rebind<SOCIAL_START_SIGNAL>().To<StartCommand>().Once();
            commandBinder.Rebind<FULFILL_CURRENT_USER_REQUEST>().To<CreateUserTileCommand>();

            commandBinder.Rebind<GAME_COMPLETE>().InSequence()
                .To<GameCompleteCommand>()
                .To<CreateFriendListCommand>();

            // Binding a command to a cross context-bound signal doesn't seem to work at the moment!  // TODO Remove
            commandBinder.Rebind<MAIN_REMOVE_SOCIAL_CONTEXT>().To<RemoveContextCommand>();

            //So today we're posting to Facebook. Maybe tomorrow we'll want to use
            //GooglePlus, or Twitter, or Pinterest...
            injectionBinder.Rebind<ISocialService>().To<FacebookService>().ToSingleton();
            //injectionBinder.Rebind<ISocialService> ().To<GoogleService> ().ToSingleton ();

            mediationBinder.Rebind<UserTileSignalView>().To<UserTileSignalMediator>();
            mediationBinder.Rebind<AwardSignalView>().To<AwardSignalMediator>();

            injectionBinder.Rebind<FULFILL_FRIENDS_REQUEST>().ToSingleton();
            injectionBinder.Rebind<REWARD_TEXT>().ToSingleton();

            BindSignalCrossContext<RESTART_GAME>();
        }
    }
}


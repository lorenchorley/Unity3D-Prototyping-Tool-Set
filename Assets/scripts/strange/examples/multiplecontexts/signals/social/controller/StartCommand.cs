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

/// StartCommand
/// ============================
/// This sets up the social component

using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.examples.multiplecontexts.signals.social;

namespace strange.examples.multiplecontexts.signals.social {
    public class StartCommand : Command {

        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject contextView { get; set; }
        [Inject] public ISocialService social { get; set; }
        [Inject] public FULFILL_CURRENT_USER_REQUEST FULFILL_CURRENT_USER_REQUEST { get; set; }

        public override void Execute() {
            Retain();
            //Note how we're using the same event for convenience here
            //and below. But the local event bus and the global one are separate, so there's
            //no systemic confusion.
            social.FULFILL_CURRENT_USER_REQUEST.AddListener(onResponse);
            social.FetchCurrentUser();
        }

        private void onResponse(UserVO vo) {
            social.FULFILL_CURRENT_USER_REQUEST.RemoveListener(onResponse);

            //We're going to Bind this for injection, since we'll need it later when we compare
            //the user's highscore with his own score and the highscore of others.
            injectionBinder.Unbind<UserVO>(UserVO.CURRENT);
            injectionBinder.Bind<UserVO>().ToValue(vo).ToName(UserVO.CURRENT);

            FULFILL_CURRENT_USER_REQUEST.Dispatch(vo);
            Release();
        }
    }
}


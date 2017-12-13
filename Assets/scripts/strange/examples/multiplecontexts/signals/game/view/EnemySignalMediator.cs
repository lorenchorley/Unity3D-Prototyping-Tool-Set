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

/// Ship mediator
/// =====================
/// Make your Mediator as thin as possible. Its function is to mediate
/// between view and app. Don't load it up with behavior that belongs in
/// the View (listening to/controlling interface), Commands (business logic),
/// Models (maintaining state) or Services (reaching out for data).

using System;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.mediation.impl;
using strange.examples.multiplecontexts.signals.game;

namespace strange.examples.multiplecontexts.signals.game {
    public class EnemySignalMediator : EventMediator<EnemySignalView> {

        [Inject] public GAME_UPDATE GAME_UPDATE { get; set; }
        [Inject] public GAME_OVER GAME_OVER { get; set; }
        [Inject] public RESTART_GAME RESTART_GAME { get; set; }
        [Inject] public ADD_TO_SCORE ADD_TO_SCORE { get; set; }

        public override void OnRegister() {
            view.init();
            UpdateListeners(true);
        }

        public override void OnRemove() {
            UpdateListeners(false);
        }

        private void UpdateListeners(bool value) {
            view.CLICK_EVENT.UpdateListener(value, onViewClicked);
            GAME_UPDATE.UpdateListener(value, onGameUpdate);
            GAME_OVER.UpdateListener(value, onGameOver);
            RESTART_GAME.UpdateListener(value, onRestart);
        }

        private void onViewClicked() {
            ADD_TO_SCORE.Dispatch(10);
        }

        private void onGameUpdate() {
            view.updatePosition();
        }

        private void onGameOver() {
            view.CLICK_EVENT.UpdateListener(false, onViewClicked);
            GAME_UPDATE.UpdateListener(false, onGameUpdate);
            //UpdateListeners(false);
        }

        private void onRestart() {
            view.CLICK_EVENT.UpdateListener(true, onViewClicked);
            GAME_UPDATE.UpdateListener(true, onGameUpdate);
            //UpdateListeners(true);
        }
    }
}


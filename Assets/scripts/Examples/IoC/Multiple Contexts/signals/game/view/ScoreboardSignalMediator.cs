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

using System;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace strange.examples.multiplecontexts.signals.game {
    public class ScoreboardSignalMediator : EventMediator<ScoreboardSignalView> {

        [Inject] public IScore model { get; set; }
        [Inject] public SCORE_CHANGE SCORE_CHANGE { get; set; }
        [Inject] public LIVES_CHANGE LIVES_CHANGE { get; set; }
        [Inject] public GAME_OVER GAME_OVER { get; set; }
        [Inject] public RESTART_GAME RESTART_GAME { get; set; }
        [Inject] public REPLAY REPLAY { get; set; }
        [Inject] public GAME_REMOVE_SOCIAL_CONTEXT REMOVE_SOCIAL_CONTEXT { get; set; }

        private const string SCORE_STRING = "score: ";
        private const string LIVES_STRING = "lives remaining: ";

        public override void OnRegister() {
            view.init(SCORE_STRING + "0", LIVES_STRING + model.lives.ToString());
            UpdateListeners(true);
        }

        public override void OnRemove() {
            UpdateListeners(false);
        }

        private void UpdateListeners(bool value) {
            SCORE_CHANGE.UpdateListener(value, onScoreChange);
            LIVES_CHANGE.UpdateListener(value, onLivesChange);
            view.REPLAY.UpdateListener(value, onReplay);
            view.REMOVE_CONTEXT.UpdateListener(value, onRemoveContext);

            GAME_OVER.UpdateListener(value, onGameOver);
            RESTART_GAME.UpdateListener(value, onRestart);
        }

        private void onScoreChange(int evt) {
            string score = SCORE_STRING + evt;
            view.updateScore(score);
        }

        private void onLivesChange(int evt) {
            string lives = LIVES_STRING + evt;
            view.updateLives(lives);
        }

        private void onGameOver() {
            SCORE_CHANGE.UpdateListener(false, onScoreChange);
            LIVES_CHANGE.UpdateListener(false, onLivesChange);

            view.gameOver();
        }

        private void onReplay() {
            REPLAY.Dispatch();
        }

        private void onRemoveContext() {
            REMOVE_SOCIAL_CONTEXT.Dispatch();
        }

        private void onRestart() {
            SCORE_CHANGE.UpdateListener(true, onScoreChange);
            LIVES_CHANGE.UpdateListener(true, onLivesChange);

            view.restart();
        }

    }
}


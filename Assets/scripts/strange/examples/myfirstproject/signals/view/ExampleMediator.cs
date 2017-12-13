/// Example mediator
/// =====================
/// Make your Mediator as thin as possible. Its function is to mediate
/// between view and app. Don't load it up with behavior that belongs in
/// the View (listening to/controlling interface), Commands (business logic),
/// Models (maintaining state) or Services (reaching out for data).

using System;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace strange.examples.myfirstproject.signals {
    public class ExampleMediator : EventMediator<ExampleView> {

        [Inject] public SCORE_CHANGE SCORE_CHANGE { get; set; }
        [Inject] public REQUEST_WEB_SERVICE REQUEST_WEB_SERVICE { get; set; }

        public override void OnRegister() {
            view.init();

            //Listen to the view for an event
            view.CLICK_EVENT.AddListener(onViewClicked);

            //Listen to the global event bus for events
            SCORE_CHANGE.AddListener(onScoreChange);

        }

        public override void OnRemove() {
            //Clean up listeners when the view is about to be destroyed
            view.CLICK_EVENT.RemoveListener(onViewClicked);
            SCORE_CHANGE.RemoveListener(onScoreChange);
            Debug.Log("Mediator OnRemove");
        }

        private void onViewClicked() {
            Debug.Log("View click detected");
            REQUEST_WEB_SERVICE.Dispatch();
        }

        private void onScoreChange(string score) {
            view.updateScore(score);
        }
    }
}


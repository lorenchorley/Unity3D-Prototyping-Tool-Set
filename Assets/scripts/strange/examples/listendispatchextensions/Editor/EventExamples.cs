using NUnit.Framework;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using System.Collections;
using UniRx;
using UnityEngine;
using static ListenDispatchExtensions;

namespace strange.examples.listendispatchextensions {

    public class EventExamples {

        #region Test specific 
        private bool requestReturned = false;

        public bool IsTestFinished {
            get {
                return requestReturned;
            }
        }
        #endregion

        #region Request with complete event
        public enum CompleteRequestEvents {
            Request,
            Request_Complete,
        }

        public void SetupCompleteRequest() {
            // Request handler
            dispatcher.AddListener(CompleteRequestEvents.Request, () => dispatcher.Dispatch(CompleteRequestEvents.Request_Complete));
        }
        #endregion

        #region Request with success and failure events
        public enum SuccessFailureRequestEvents {
            Request,
            Request_Success,
            Request_Failure
        }

        public void SetupSuccessFailureRequest() {
            // Request handler
            dispatcher.AddListener(SuccessFailureRequestEvents.Request, HandleSuccessFailureRequest);
        }

        public void HandleSuccessFailureRequest() {
            object returnEvent = (Random.Range(0f, 1f) >= 0.5f) ? SuccessFailureRequestEvents.Request_Success : SuccessFailureRequestEvents.Request_Failure;
            dispatcher.Dispatch(returnEvent);
        }
        #endregion

        public IEventDispatcher dispatcher;

        [SetUp]
        public void Setup() {
            dispatcher = new EventDispatcher();
            SetupCompleteRequest();
            SetupSuccessFailureRequest();
        }

        [Test]
        public void AsyncRequestWithCompletion() {

            dispatcher.DispatchAndListen(
                new ER(CompleteRequestEvents.Request, null),

                // Signal listener, listening for success
                new EL(CompleteRequestEvents.Request_Complete, () => {
                    requestReturned = true;
                })

            );

        }

        [Test]
        public void AsyncRequestWithSuccessAndFailure() {

            dispatcher.DispatchAndListen(
                new ER(SuccessFailureRequestEvents.Request, null),

                // Signal listener, listening for success
                new EL(SuccessFailureRequestEvents.Request_Success, () => {
                    requestReturned = true;
                }),

                // Signal listener, listening for failure
                new EL(SuccessFailureRequestEvents.Request_Failure, () => {
                    requestReturned = true;
                })

                // All listeners are stopped when one is returned

            );

        }

    }

}
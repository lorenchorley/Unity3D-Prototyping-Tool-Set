using NUnit.Framework;
using strange.extensions.signal.impl;
using System.Collections;
using UniRx;
using UnityEngine;
using static ListenDispatchExtensions;

namespace strange.examples.listendispatchextensions {

    public class SignalExamples {

        #region Test specific 
        private bool request1Returned = false;
        private bool request2Returned = false;
        private bool request3Returned = false;

        public bool IsTestFinished {
            get {
                return request1Returned && request2Returned && request3Returned;
            }
        }
        #endregion

        #region Request with complete event
        public class CompletionRequestSignal : Signal {
            public class Complete : Signal { }
        }

        public void SetupCompleteRequest() {
            CompletionRequest = new CompletionRequestSignal();
            CompletionRequest_Complete = new CompletionRequestSignal.Complete();

            // Request handler
            CompletionRequest.AddListener(HandleCompletionRequest);
        }

        public void HandleCompletionRequest() {
            CompletionRequest_Complete.Dispatch();
        }
        #endregion

        #region Request with success and failure
        public class SuccessFailureRequestSignal : Signal { // The signal that is dispatched when the request is made. Can take parameters using Signal<A,B,C>
            public class Success : Signal { } // The signal that is dispatched by the handler of RequestSignal to indicate success for the request. Parameters can be returned as normal
            public class Failure : Signal { } // Similar to success
        }

        public void SetupSuccessFailureRequest() {
            SuccessFailureRequest = new SuccessFailureRequestSignal();
            SuccessFailureRequest_Success = new SuccessFailureRequestSignal.Success();
            SuccessFailureRequest_Failure = new SuccessFailureRequestSignal.Failure();

            // Request handler
            SuccessFailureRequest.AddListener(HandleSuccessFailureRequest);
        }

        public void HandleSuccessFailureRequest() {
            Signal returnSignal = (Random.Range(0f, 1f) >= 0.5f) ? 
                (Signal) SuccessFailureRequest_Success : 
                (Signal) SuccessFailureRequest_Failure;
            returnSignal.Dispatch();
        }
        #endregion

        //#region Multiple requests with success and failure
        //public class FirstSuccessFailureRequestSignal : Signal {
        //    public class Success : Signal { } 
        //    public class Failure : Signal { } 
        //}

        //public class SecondSuccessFailureRequestSignal : Signal {
        //    public class Success : Signal { }
        //    public class Failure : Signal { }
        //}

        //public void SetupMultipleSuccessFailureRequests() {
        //    FirstSuccessFailureRequest = new FirstSuccessFailureRequestSignal();
        //    FirstSuccessFailureRequest_Success = new FirstSuccessFailureRequestSignal.Success();
        //    FirstSuccessFailureRequest_Failure = new FirstSuccessFailureRequestSignal.Failure();

        //    SecondSuccessFailureRequest = new SecondSuccessFailureRequestSignal();
        //    SecondSuccessFailureRequest_Success = new SecondSuccessFailureRequestSignal.Success();
        //    SecondSuccessFailureRequest_Failure = new SecondSuccessFailureRequestSignal.Failure();

        //    // Request handler
        //    FirstSuccessFailureRequest.AddListener(HandleFirstSuccessFailureRequest);
        //    SecondSuccessFailureRequest.AddListener(HandleSecondSuccessFailureRequest);
        //}

        //public void HandleFirstSuccessFailureRequest() {
        //    Signal returnSignal = (Random.Range(0f, 1f) >= 0.5f) ?
        //        (Signal) FirstSuccessFailureRequest_Success :
        //        (Signal) FirstSuccessFailureRequest_Failure;
        //    returnSignal.Dispatch();
        //}

        //public void HandleSecondSuccessFailureRequest() {
        //    Signal returnSignal = (Random.Range(0f, 1f) >= 0.5f) ?
        //        (Signal) SecondSuccessFailureRequest_Success :
        //        (Signal) SecondSuccessFailureRequest_Failure;
        //    returnSignal.Dispatch();
        //}
        //#endregion

        [Inject] public CompletionRequestSignal CompletionRequest { get; set; }
        [Inject] public CompletionRequestSignal.Complete CompletionRequest_Complete { get; set; }

        [Inject] public SuccessFailureRequestSignal SuccessFailureRequest { get; set; }
        [Inject] public SuccessFailureRequestSignal.Success SuccessFailureRequest_Success { get; set; }
        [Inject] public SuccessFailureRequestSignal.Failure SuccessFailureRequest_Failure { get; set; }

        //[Inject] public FirstSuccessFailureRequestSignal FirstSuccessFailureRequest { get; set; }
        //[Inject] public FirstSuccessFailureRequestSignal.Success FirstSuccessFailureRequest_Success { get; set; }
        //[Inject] public FirstSuccessFailureRequestSignal.Failure FirstSuccessFailureRequest_Failure { get; set; }

        //[Inject] public SecondSuccessFailureRequestSignal SecondSuccessFailureRequest { get; set; }
        //[Inject] public SecondSuccessFailureRequestSignal.Success SecondSuccessFailureRequest_Success { get; set; }
        //[Inject] public SecondSuccessFailureRequestSignal.Failure SecondSuccessFailureRequest_Failure { get; set; }

        [SetUp]
        public void Setup() {
            SetupCompleteRequest();
            SetupSuccessFailureRequest();
            //SetupMultipleSuccessFailureRequests();
        }

        [Test]
        public void AsyncRequestWithCompletion() {

            CompletionRequest.DispatchAndListen(

                // Signal listener, listening for success
                new SL(CompletionRequest_Complete, () => {
                    request1Returned = true;
                })

            );

        }

        [Test]
        public void AsyncRequestWithSuccessAndFailure() {

            SuccessFailureRequest.DispatchAndListen(

                // Signal listener, listening for success
                new SL(SuccessFailureRequest_Success, () => {
                    request2Returned = true;
                }),

                // Signal listener, listening for failure
                new SL(SuccessFailureRequest_Failure, () => {
                    request2Returned = true;
                })

                // All listeners are stopped when one is returned

            );

        }

        //[Test]
        //public void AsyncMultipleRequestsWithSuccessAndFailure() {

        //    new BaseSignalListener[] { 

        //        new SL(FirstSuccessFailureRequest_Success, () => {
        //        }),

        //        new SL(SecondSuccessFailureRequest_Success, () => {
        //        }),

        //    }.ListenOnceForAll();

        //    new BaseSignalListener[] {

        //        new SL(FirstSuccessFailureRequest_Failure, () => {
        //        })

        //        new SL(SecondSuccessFailureRequest_Failure, () => {
        //        })

        //    }.ListenOnceForOne();

        //    // Start multiple requests
        //    new SignalRequester[] {
        //        new CSR(FirstSuccessFailureRequest),
        //        new CSR(SecondSuccessFailureRequest)
        //    }.Dispatch();

        //}

    }

}
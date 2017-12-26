/// An Asynchronous Command
/// ============================
/// This demonstrates how to use a Command to perform an asynchronous action;
/// for example, if you need to call a web service. The two most important lines
/// are the Retain() and Release() calls.

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace strange.examples.myfirstproject.signals {
    public class CallWebServiceCommand : Command {

        [Inject] public IExampleModel model { get; set; }
        [Inject] public IExampleService service { get; set; }
        [Inject] public SCORE_CHANGE SCORE_CHANGE { get; set; }

        static int counter = 0;

        public CallWebServiceCommand() {
            ++counter;  //This counter is here to demonstrate that a new Command is created each time.
        }

        public override void Execute() {
            //Retain marks the Command as requiring time to execute.
            //If you call Retain, you MUST have corresponding Release()
            //calls, or you will get memory leaks.
            Retain();

            //Call the service. Listen for a response
            service.FULFILL_SERVICE_REQUEST.AddListener(onComplete);
            service.Request("http://www.thirdmotion.com/ ::: " + counter.ToString());
        }

        //The payload is in the form of a IEvent
        private void onComplete(string data) {
            //Remember to clean up. Remove the listener.
            service.FULFILL_SERVICE_REQUEST.RemoveListener(onComplete);

            model.data = data;
            SCORE_CHANGE.Dispatch(data);

            //Remember to call release when done.
            Release();
        }
    }
}


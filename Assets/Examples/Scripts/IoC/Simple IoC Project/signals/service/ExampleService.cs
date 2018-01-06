/// Example Service
/// ======================
/// Nothing to see here. Just your typical place to store some data.

using System;
using System.Collections;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.signal.impl;

namespace strange.examples.myfirstproject.signals {
    public class ExampleService : IExampleService {

        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject contextView { get; set; }

        public Signal<string> FULFILL_SERVICE_REQUEST { get; }

        private string url;

        public ExampleService() {
            FULFILL_SERVICE_REQUEST = new Signal<string>();
        }

        public void Request(string url) {
            this.url = url;

            //For now, we'll spoof a web service by running a coroutine for 1 second...
            MonoBehaviour root = contextView.GetComponent<MyFirstSignalProjectRoot>();
            root.StartCoroutine(waitASecond());
        }

        private IEnumerator waitASecond() {
            yield return new WaitForSeconds(1f);

            //...then pass back some fake data
            FULFILL_SERVICE_REQUEST.Dispatch(url);
        }
    }
}


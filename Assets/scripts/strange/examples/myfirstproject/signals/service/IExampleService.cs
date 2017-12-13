using System;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.signal.impl;

namespace strange.examples.myfirstproject.signals {
    public interface IExampleService {
        void Request(string url);
        Signal<string> FULFILL_SERVICE_REQUEST { get; }
    }
}


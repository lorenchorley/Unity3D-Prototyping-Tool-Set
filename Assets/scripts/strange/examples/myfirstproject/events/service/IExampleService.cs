using System;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace strange.examples.myfirstproject.events {
    public interface IExampleService {
        void Request(string url);
        IEventDispatcher dispatcher { get; set; }
    }
}


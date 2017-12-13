using strange.extensions.signal.impl;
using System;

namespace strange.examples.myfirstproject.signals {
    public class SOCIAL_START_SIGNAL : Signal { };
    public class SCORE_CHANGE : Signal<string> { };
    public class REQUEST_WEB_SERVICE : Signal { };
    public class FULFILL_SERVICE_REQUEST : Signal { };
}


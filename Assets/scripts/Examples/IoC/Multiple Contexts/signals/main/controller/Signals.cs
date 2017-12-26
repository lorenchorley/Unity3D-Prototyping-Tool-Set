using strange.examples.multiplecontexts.signals.game;
using strange.extensions.signal.impl;

namespace strange.examples.multiplecontexts.signals.main {
    public class MAIN_START_SIGNAL : Signal { }
    public class LOAD_SCENE : Signal<string> { }
    public class SCENE_LOADED : StrictSignal { }
    public class GAME_COMPLETE : Signal<int> { }
    public class FULFILL_SERVICE_REQUEST : StrictSignal { }
    public class MAIN_REMOVE_SOCIAL_CONTEXT : Signal { }
    public class REQUEST_WEB_SERVICE : StrictSignal { }
}


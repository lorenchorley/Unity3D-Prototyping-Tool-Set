using System;
using strange.extensions.signal.impl;

namespace strange.examples.multiplecontexts.signals.game {
    //public class GAME_START_SIGNAL : Signal { }
    public class ADD_TO_SCORE : Signal<int> { }
    public class GAME_OVER : Signal { }
    public class GAME_UPDATE : StrictSignal { }
    public class LIVES_CHANGE : StrictSignal<int> { }
    public class GAME_REMOVE_SOCIAL_CONTEXT : Signal { }
    public class REPLAY : Signal { }
    public class RESTART_GAME : StrictSignal { }
    public class SCORE_CHANGE : StrictSignal<int> { }
    public class SHIP_DESTROYED : Signal { }
}


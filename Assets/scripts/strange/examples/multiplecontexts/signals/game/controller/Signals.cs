using System;
using strange.extensions.signal.impl;

namespace strange.examples.multiplecontexts.signals.game {
    public class GAME_START_SIGNAL : Signal { }
    public class ADD_TO_SCORE : Signal<int> { }
    public class GAME_OVER : Signal { }
    public class GAME_UPDATE : Signal { }
    public class LIVES_CHANGE : Signal<int> { }
    public class GAME_REMOVE_SOCIAL_CONTEXT : Signal { }
    public class REPLAY : Signal { }
    public class RESTART_GAME : Signal { }
    public class SCORE_CHANGE : Signal<int> { }
    public class SHIP_DESTROYED : Signal { }
}


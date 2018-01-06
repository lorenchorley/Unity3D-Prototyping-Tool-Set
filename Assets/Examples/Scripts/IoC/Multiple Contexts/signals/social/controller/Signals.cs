using strange.examples.multiplecontexts.signals.social;
using strange.extensions.signal.impl;

namespace strange.examples.multiplecontexts.signals.social {
    //public class SOCIAL_START_SIGNAL : Signal { }
    public class FULFILL_CURRENT_USER_REQUEST : Signal<UserVO> { }
    public class FULFILL_FRIENDS_REQUEST : StrictSignal { }
    public class REWARD_TEXT : StrictSignal<string> { }
}


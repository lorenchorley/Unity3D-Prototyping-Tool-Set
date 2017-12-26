using UnityEngine;
using System.Collections;

namespace eventsource {

    public interface IESCommandable<C> where C : ESCommand {
        //void CommandThis(C c);
        ESEvent ESUSEONLYCOMMAND(C c);
    }

}
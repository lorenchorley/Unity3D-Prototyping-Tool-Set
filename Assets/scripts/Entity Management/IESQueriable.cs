using UnityEngine;
using System.Collections;

namespace eventsource {

    public interface IESQueriable<Q> where Q : IESQuery {
        //void QueryThis(Q q);
        void ESUSEONLYQUERY(Q q);
    }

}
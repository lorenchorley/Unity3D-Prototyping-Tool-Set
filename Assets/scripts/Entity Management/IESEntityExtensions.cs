using UnityEngine;
using System;

namespace eventsource {

    public static class IESEntityExtensions {
        
        public static void Command<E, C>(this E e, C c) where C : ESCommand where E : IESEntity, IESCommandable<C> {
            e.ES.Command(e, c);
        }

        public static void Query<E, Q>(this E e, Q q) where Q : IESQuery where E : IESEntity, IESQueriable<Q> {
            e.ES.Query(e, q);
        }

    }

}
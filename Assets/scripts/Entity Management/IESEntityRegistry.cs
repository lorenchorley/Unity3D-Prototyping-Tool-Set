using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace eventsource {

    public interface IESEntityRegistry {
    }

    public interface IESEntityRegistry<E> : IESEntityRegistry where E : IESEntity {

        E GetEntityByUID(int uid);
        void SetEntity(int uid, E e);
        int EntityCount { get; }
        IList<E> Entities { get; }
        IList<int> UIDs { get; }

    }

}
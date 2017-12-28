using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace eventsourcing {

    public interface IEntityRegistry {
    }

    public interface IEntityRegistry<E> : IEntityRegistry where E : IEntity {

        E GetEntityByUID(int uid);
        void SetEntity(int uid, E e);
        int EntityCount { get; }
        IList<E> Entities { get; }
        IList<int> UIDs { get; }

    }

}
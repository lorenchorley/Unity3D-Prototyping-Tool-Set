using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace eventsourcing {

    public interface IEntityRegistry {
        int EntityCount { get; }
        IEnumerable<int> UIDs { get; }
    }

    public interface IEntityRegistry<E> : IEntityRegistry where E : IEntity {
        E GetEntityByUID(int uid);
        E GetEntityByKey(EntityKey key);
        void ApplyQuery<F, Q>(EntityKey key, Q q) where Q : IQuery where F : E, IQueriable<Q>; // Good for struct entities, the value doesn't need to move
        void SetEntity(E e);
        IEnumerable<E> Entities { get; }
    }

}
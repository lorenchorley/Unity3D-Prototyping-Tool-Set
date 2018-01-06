using System.Collections.Generic;

namespace entitymanagement {

    public interface IEntityRegistry : ISerialisationAware {
        int EntityCount { get; }
        IEnumerable<int> UIDs { get; }
        IEntity GetUncastEntityByKey(EntityKey key);
    }

    public interface IEntityRegistry<E> : IEntityRegistry where E : IEntity {
        E GetEntityByUID(int uid);
        E GetEntityByKey(EntityKey key);
        void ApplyQuery<F, Q>(EntityKey key, Q q) where Q : IQuery where F : E, IQueriable<Q>; // Good for struct entities, the value doesn't need to move
        IEnumerable<E> Entities { get; }
    }

    public interface IReferenceEntityRegistry<E> : IEntityRegistry<E> where E : IEntity {
        void SetEntity(E e);
    }

    public interface IValueEntityRegistry<E> : IEntityRegistry<E> where E : IEntity {
        void SetEntity(ref E e);
    }

}
using UnityEngine;
using System.Collections;
using eventsourcing;

namespace entitymanagement {

    public interface IModifiable<M> where M : IEntityModifier {
        IEvent ApplyMod(M m);
    }

}
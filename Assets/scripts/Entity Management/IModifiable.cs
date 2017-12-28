using UnityEngine;
using System.Collections;

namespace eventsourcing {

    public interface IModifiable<M> where M : IModifier {
        IEvent ApplyMod(M m);
    }

}
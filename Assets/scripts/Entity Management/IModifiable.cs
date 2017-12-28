using UnityEngine;
using System.Collections;

namespace eventsourcing {

    public interface IModifiable<C> where C : IModifier {
        IEvent ESUSEONLYMODIFY(C c);
    }

}
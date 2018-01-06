using UnityEngine;
using System.Collections;

namespace entitymanagement {

    public interface IEntity {

        int UID { get; }

        // Don't want this to be serialised, only temporary information
        EntityKey Key { get; set; }

    }

}
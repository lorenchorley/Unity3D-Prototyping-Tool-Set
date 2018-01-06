using UnityEngine;
using System.Collections;
using System;

namespace entitymanagement {

    // Used as a stand-in for another entity, so that the key may be read
    public class EmptyEntity : IEntity {

        public int UID {
            get {
                throw new Exception();
            }
        }

        public EntityKey Key { get; set; }

    }

}
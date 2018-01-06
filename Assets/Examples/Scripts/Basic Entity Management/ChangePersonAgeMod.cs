using UnityEngine;
using System;

namespace entitymanagement.examples.basic {

    [Serializable]
    public class ChangePersonAgeMod : IEntityModifier {
        public int NewAge;

        public IEntity e { get; set; }

        public void Execute() {
            // Could be used if run independently of an entity
            // Otherwise not used, see IEvent ApplyMod(ChangePersonAgeMod m) method in PersonEntity
            // This is done so that the method has private level access to PersonEntity
        }

    }

}
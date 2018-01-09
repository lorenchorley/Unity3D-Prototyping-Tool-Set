using UnityEngine;
using System.Collections;
using System;
using strange.extensions.command.api;

namespace entitymanagement {

    public interface IEntityModifier : IModifier  {
        IEntity e { get; set; }
        Type IntendedEntityType { get; }
    }

}
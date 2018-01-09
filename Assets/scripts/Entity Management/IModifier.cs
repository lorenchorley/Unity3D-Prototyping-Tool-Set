using UnityEngine;
using System.Collections;
using System;
using strange.extensions.command.api;

namespace entitymanagement {

    public interface IModifier : IBaseCommand {

        long CreationTime { get; }

    }
}
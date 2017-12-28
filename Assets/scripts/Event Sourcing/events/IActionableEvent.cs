using UnityEngine;
using System.Collections;
using System;
using ZeroFormatter;
using eventsourcing.examples.network;
using strange.extensions.command.api;

namespace eventsourcing {

    public interface IActionableEvent : IEvent {

        IBaseCommand NewDoCommand();
        IBaseCommand NewUndoCommand();

    }

}
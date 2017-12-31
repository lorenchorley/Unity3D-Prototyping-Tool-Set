using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace eventsourcing.examples.network {

    public class PositionCheckProjection : IProjection {

        public int PlayerCount;
        public Dictionary<int, Vector2> PlayerPositionsByUID;

        public IDisposable CancelToken { get; set; }

        public void Reset() {
            PlayerPositionsByUID = new Dictionary<int, Vector2>();
        }

        public bool Process(IEvent e) {
            if (e is PlayerInputEvent) {
                PlayerInputEvent inputEvent = e as PlayerInputEvent;
                Vector2 pos;

                if (!PlayerPositionsByUID.TryGetValue(inputEvent.PlayerUID, out pos))
                    throw new Exception("Player moved that was not yet created: " + inputEvent.PlayerUID);

                PlayerPositionsByUID[inputEvent.PlayerUID] = pos + NetworkGameMaster.DirectionToMovement(inputEvent.Direction);

            } else if (e is PlayerCreatedEvent) {
                PlayerCreatedEvent createdEvent = e as PlayerCreatedEvent;

                if (PlayerPositionsByUID.ContainsKey(createdEvent.PlayerUID))
                    throw new Exception("Player was created for a second time: " + createdEvent.PlayerUID);

                PlayerPositionsByUID[createdEvent.PlayerUID] = Vector2.zero;

            }

            return true;
        }

        public void OnFinish() {
        }

    }

}
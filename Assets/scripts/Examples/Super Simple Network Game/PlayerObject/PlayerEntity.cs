using UnityEngine;
using System;

namespace eventsourcing.examples.network {

    public class PlayerEntity : IEntity, IQueriable<PlayerPositionQuery>, IModifiable<DoPlayerInputMod> {

        private int uid;
        public int UID { get { return uid; } }
        
        public int Index { get; set; }

        public PlayerComponent PlayerComponent;
        private Vector2 position;

        public PlayerEntity(int uid) {
            this.uid = uid;
            position = Vector2.zero;
        }

        public IEvent ApplyMod(DoPlayerInputMod c) {
            // Record old value
            PlayerInputEvent e = new PlayerInputEvent {
                Direction = c.direction,
                PlayerUID = uid,
                OldPosition = position
            };

            // Apply command
            position += NetworkGameMaster.DirectionToMovement(c.direction);
            position = new Vector2(Mathf.Clamp(position.x, -1, 1), Mathf.Clamp(position.y, -1, 1));

            // Record new value
            e.NewPosition = position;

            return e;
        }

        public static IEvent ApplyMod(EntityManager EM, CreateLocalPlayerMod c) {
            NetworkGameMaster NetworkTester = GameObject.FindObjectOfType<NetworkGameMaster>();
            if (NetworkTester.CurrentPlayer != null)
                throw new Exception("Current player is already set");

            PlayerCreatedEvent e = new PlayerCreatedEvent();

            // Apply command
            NetworkTester.CurrentPlayer = EM.GetRegistry<PlayerRegistry>().NewEntity();
            NetworkTester.CurrentPlayer.PlayerComponent = GameObject.Instantiate(NetworkTester.PlayerTemplate.gameObject).GetComponent<PlayerComponent>();
            NetworkTester.CurrentPlayer.PlayerComponent.UID = NetworkTester.CurrentPlayer.UID;

            e.PlayerUID = NetworkTester.CurrentPlayer.uid;

            return e;
        }

        public static IEvent ApplyMod(EntityManager EM, DisablePlayerMod c) {
            PlayerLeftEvent e = new PlayerLeftEvent();

            // Apply command
            PlayerEntity player = EM.GetRegistry<PlayerRegistry>().NewEntity();
            GameObject.Destroy(player.PlayerComponent);

            e.PlayerUID = player.uid;

            return e;
        }

        public void Query(PlayerPositionQuery q) {
            q.Position = position;
        }

    }

}
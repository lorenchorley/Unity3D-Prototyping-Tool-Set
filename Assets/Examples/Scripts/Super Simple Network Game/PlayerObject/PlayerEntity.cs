using UnityEngine;
using System;
using entitymanagement;
using FullSerializer;

namespace eventsourcing.examples.network {

    public class PlayerEntity : IEntity, IQueriable<PlayerPositionQuery>, IModifiable<DoPlayerInputMod> {

        private int uid;
        public int UID { get { return uid; } }

        public EntityKey Key { get; set; }

        [fsIgnore]
        public PlayerComponent PlayerComponent;

        private Vector2 position;

        public PlayerEntity(int uid, EntityKey Key) {
            this.uid = uid;
            this.Key = Key;
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

        public static IEvent ApplyMod(EntityManager EM, CreatePlayerMod c) {
            NetworkGameMaster NetworkTester = GameObject.FindObjectOfType<NetworkGameMaster>();
            PlayerEntity player;
            PlayerCreatedEvent e = new PlayerCreatedEvent();

            // Apply command
            player = EM.GetRegistry<PlayerRegistry>().NewEntity();
            player.PlayerComponent = GameObject.Instantiate(NetworkTester.PlayerTemplate.gameObject).GetComponent<PlayerComponent>();
            player.PlayerComponent.UID = player.UID;
            e.PlayerUID = player.uid;
            e.PlayerPhotonID = c.PlayerPhotonID;

            if (NetworkTester.CurrentPlayer == null && PhotonNetwork.player.ID == c.PlayerPhotonID) {
                NetworkTester.CurrentPlayer = player;
            }

            return e;
        }

        public IEvent ApplyMod(DisablePlayerMod c) {
            PlayerLeftEvent e = new PlayerLeftEvent();

            // Apply command
            GameObject.Destroy(PlayerComponent);

            e.PlayerUID = uid;

            return e;
        }

        public void Query(PlayerPositionQuery q) {
            q.Position = position;
        }

    }

}
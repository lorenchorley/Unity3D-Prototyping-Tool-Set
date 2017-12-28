using UnityEngine;
using System;

namespace eventsourcing.examples.network {

    public class PlayerEntity : IEntity, IQueriable<PlayerPositionQuery>, IModifiable<DoPlayerInputCommand> {

        private int uid;
        public int UID { get { return uid; } }

        protected EventSource es;
        public EventSource ES {
            get {
                return es;
            }
        }

        public int Index { get; set; }
        public PlayerComponent PlayerComponent;

        private Vector2 position;

        public PlayerEntity(EventSource es, int uid) {
            this.es = es;
            this.uid = uid;
            position = Vector2.zero;
        }

        public IEvent ESUSEONLYMODIFY(DoPlayerInputCommand c) {
            // Record old value
            PlayerInputEvent e = new PlayerInputEvent {
                Direction = c.direction,
                PlayerUID = uid,
                OldPosition = position
            };

            // Apply command
            position += NetworkTester.DirectionToMovement(c.direction);
            position = new Vector2(Mathf.Clamp(position.x, -1, 1), Mathf.Clamp(position.y, -1, 1));

            // Record new value
            e.NewPosition = position;

            return e;
        }

        public static IEvent ESUSEONLYCOMMAND(EventSource ES, CreateLocalPlayerCommand c) {
            NetworkTester NetworkTester = GameObject.FindObjectOfType<NetworkTester>();
            if (NetworkTester.CurrentPlayer != null)
                throw new Exception("Current player is already set");

            PlayerCreatedEvent e = new PlayerCreatedEvent();

            // Apply command
            NetworkTester.CurrentPlayer = ES.GetRegistry<PlayerRegistry>().NewEntity();
            NetworkTester.CurrentPlayer.PlayerComponent = GameObject.Instantiate(NetworkTester.PlayerTemplate.gameObject).GetComponent<PlayerComponent>();
            NetworkTester.CurrentPlayer.PlayerComponent.UID = NetworkTester.CurrentPlayer.UID;

            e.PlayerUID = NetworkTester.CurrentPlayer.uid;

            return e;
        }

        public void ESUSEONLYQUERY(PlayerPositionQuery q) {
            q.Position = position;
        }

    }

}
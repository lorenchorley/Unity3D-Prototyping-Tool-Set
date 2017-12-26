using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine.Assertions;
using ZeroFormatter;

namespace eventsource.examples.network {

    public class NetworkTester : MonoBehaviour {

        public static float movementIncrement = 0.1f;

        public static Direction KeyToDirection(KeyCode k) {
            switch (k) {
            case KeyCode.UpArrow:
                return Direction.up;
            case KeyCode.DownArrow:
                return Direction.down;
            case KeyCode.LeftArrow:
                return Direction.left;
            case KeyCode.RightArrow:
                return Direction.right;
            default:
                throw new Exception("KeyCode not implemented: " + k);
            }
        }

        public static IEnumerable<KeyCode> MappedKeys() {
            yield return KeyCode.UpArrow;
            yield return KeyCode.DownArrow;
            yield return KeyCode.LeftArrow;
            yield return KeyCode.RightArrow;
        }

        public static Vector2 DirectionToVector(Direction d) {
            switch (d) {
            case Direction.up:
                return Vector2.up;
            case Direction.down:
                return Vector2.down;
            case Direction.left:
                return Vector2.left;
            case Direction.right:
                return Vector2.right;
            default:
                throw new Exception("Direction not implemented: " + d);
            }
        }

        public static Vector2 DirectionToMovement(Direction d) {
            return DirectionToVector(d) * movementIncrement;
        }

        [Header("Templates")]
        public PlayerComponent PlayerTemplate;

        [Header("Event Source")]
        public EventSource ES;
        public PlayerRegistry PlayerRegister;

        [Header("Photon")]
        public PhotonView View;
        public PUNManager PUNManager;
        public PUNCommander PUNCommander;
        public PUNHasher PUNHasher;
        public PUNPauser PUNPauser;
        public PUNESRequester PUNESRequester;

        [Header("Game State")]
        public bool isPlaying;
        public PlayerEntity CurrentPlayer;

        void Start() {
            isPlaying = false;

            ZeroFormatterInitializer.Register();

            View = GetComponent<PhotonView>() ?? gameObject.AddComponent<PhotonView>();
            ES = GetComponent<EventSource>() ?? gameObject.AddComponent<EventSource>();
            PlayerRegister = new PlayerRegistry(ES);

            PUNManager = GetComponent<PUNManager>() ?? gameObject.AddComponent<PUNManager>();
            PUNCommander = GetComponent<PUNCommander>() ?? gameObject.AddComponent<PUNCommander>();
            PUNHasher = GetComponent<PUNHasher>() ?? gameObject.AddComponent<PUNHasher>();
            PUNPauser = GetComponent<PUNPauser>() ?? gameObject.AddComponent<PUNPauser>();
            PUNESRequester = GetComponent<PUNESRequester>() ?? gameObject.AddComponent<PUNESRequester>();
            
            PUNManager.StartPhoton(() => {
                if (PhotonNetwork.otherPlayers.Length == 0) { 
                    ES.Command(new CreateLocalPlayerCommand());
                    isPlaying = true;
                } else
                    SynchroniseWithOtherPlayers();
            });
        }

        void Update() {
            if (!isPlaying) {
                Debug.Log("Is Paused");
                return;
            }

            bool inputDone = false;
            foreach (KeyCode key in MappedKeys()) {
                if (Input.GetKeyDown(key)) {
                    DoInput(KeyToDirection(key));
                    inputDone = true;
                }
            }

            if (inputDone) // This needs to be done once the event occurs!
                CurrentPlayer.PlayerComponent.RefreshPosition();

        }

        void OnApplicationQuit() {
            PhotonNetwork.Disconnect();
        }

        public void SynchroniseWithOtherPlayers() {
            Action PauseAll = null, RequestES = null, CheckHash = null, CreateLocalPlayer = null, UnpauseAll = null;

            // Pause game for all players
            PauseAll = () => PUNPauser.SetAllPlayersPaused(true, RequestES);

            // Request ES data from one player
            RequestES = () => PUNESRequester.RequestESAndImport(CheckHash);

            // Do hashcode check on data for all players
            CheckHash = () => PUNHasher.RequestHashCheck(CreateLocalPlayer);

            // Create local player and broadcast
            CreateLocalPlayer = () => {
                Debug.Log("CreateLocalPlayer");

                CreateLocalPlayer();

                UnpauseAll.Invoke();
            };

            // Unpause game for all players
            UnpauseAll = () => PUNPauser.SetAllPlayersPaused(false, () => { });

            // Start
            PauseAll.Invoke();
        }

        public void CreateLocalPlayer() {
            ES.Command(new CreateLocalPlayerCommand());
        }
        
        public void DoInput(Direction direction) {
            ES.Command(CurrentPlayer, new DoPlayerInputCommand() { direction = direction });
        }

    }

}
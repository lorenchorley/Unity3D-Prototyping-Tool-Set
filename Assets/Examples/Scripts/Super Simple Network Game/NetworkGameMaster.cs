using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine.Assertions;
using ZeroFormatter;
using entitymanagement;
using photon.helpers;
using photon.essynchronisation;

namespace eventsourcing.examples.network {

    [RequireComponent(typeof(PhotonView))]
    public class NetworkGameMaster : MonoBehaviour {

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

        [Header("Entity Management")]
        public EntityManager EM;
        public PlayerRegistry PlayerRegister;

        [Header("Photon")]
        public PhotonView View;
        public PUNConnecter PUNConnecter;
        public PUNSynchroniser PUNESSynchroniser;

        [Header("Game State")]
        public PlayerEntity CurrentPlayer;

        void Start() {
            Serialisation.InitialiseDevelopmentSerialisation();

            View = GetComponent<PhotonView>();
            ES = GetComponent<EventSource>() ?? FindObjectOfType<EventSource>() ?? gameObject.AddComponent<EventSource>();
            EM = GetComponent<EntityManager>() ?? FindObjectOfType<EntityManager>() ?? gameObject.AddComponent<EntityManager>();
            PUNConnecter = GetComponent<PUNConnecter>() ?? FindObjectOfType<PUNConnecter>() ?? gameObject.AddComponent<PUNConnecter>();
            PUNESSynchroniser = GetComponent<PUNSynchroniser>() ?? FindObjectOfType<PUNSynchroniser>() ?? gameObject.AddComponent<PUNSynchroniser>();

            PlayerRegister = new PlayerRegistry(EM, 5);

            if (!PUNConnecter.ConnectOnStart)
                PUNConnecter.StartPhoton();

            PUNConnecter.RegisterGuaranteedConnectedCallback(() => {
                Assert.IsTrue(PhotonNetwork.inRoom);
                PUNESSynchroniser.SetupAndSynchronise(OnSynchronisedAndReady);
            });

        }

        void Update() {
            if (PUNESSynchroniser.PUNPauser.IsPaused)
                return;

            foreach (KeyCode key in MappedKeys()) {
                if (Input.GetKeyDown(key)) {
                    DoInput(KeyToDirection(key));
                }
            }

            bool eventsHaveBeenReceivedThisStep = !ES.IsLastEventOfType<IntervalStepEvent>();
            if (eventsHaveBeenReceivedThisStep) { 
                if (CurrentPlayer != null && CurrentPlayer.PlayerComponent != null)
                    CurrentPlayer.PlayerComponent.RefreshPosition();
            }

        }

        void OnSynchronisedAndReady() {

            // Create local player and broadcast
            EM.ApplyMod(new CreatePlayerMod() { PlayerPhotonID = PhotonNetwork.player.ID });

        }

        void OnApplicationQuit() {
            PhotonNetwork.Disconnect();
        }

        public void DoInput(Direction direction) {
            EM.ApplyMod(CurrentPlayer, new DoPlayerInputMod() { direction = direction });
        }

    }

}
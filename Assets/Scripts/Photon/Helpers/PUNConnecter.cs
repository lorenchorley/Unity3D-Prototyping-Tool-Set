using UnityEngine;
using System;

namespace photon.helpers {

    public class PUNConnecter : Photon.MonoBehaviour {

        [Header("Preferences")]
        public string Version = "v1.0";
        public string DefaultRoomName = "DefaultRoomName";
        public byte MaxPlayerPreference = 4;
        public bool ConnectOnStart = true;

        [Header("Status")]
        public string Status = "Not started";

        private event Action GuaranteedConnectedCallback;

        public void RegisterGuaranteedConnectedCallback(Action callback) {
            GuaranteedConnectedCallback += callback;
            if (PhotonNetwork.inRoom)
                callback.Invoke();
        }

        public void UnregisterGuaranteedConnectedCallback(Action callback) {
            GuaranteedConnectedCallback -= callback;
        }

        void Start() {
            if (ConnectOnStart && !PhotonNetwork.connected)
                StartPhoton();
        }

        public void StartPhoton() {
            PhotonNetwork.ConnectUsingSettings(Version);
        }

        void OnConnectedToMaster() {
            Status = "Connected to master";
            JoinRoom();
        }

        void OnJoinedLobby() {
            Status = "Joined lobby";
            JoinRoom();
        }

        void JoinRoom() {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = (byte) MaxPlayerPreference;
            PhotonNetwork.JoinOrCreateRoom(DefaultRoomName, options, TypedLobby.Default);
        }

        void OnJoinedRoom() {
            Status = "Joined room";

            GuaranteedConnectedCallback.Invoke();
        }

    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine.Assertions;

namespace eventsourcing.examples.network {

    public class PUNManager : Photon.MonoBehaviour {

        public string Version = "v1.0";

        private Action connectedCallback;

        public void StartPhoton(Action connectedCallback) {
            this.connectedCallback = connectedCallback;
            PhotonNetwork.ConnectUsingSettings(Version);
        }

        void OnConnectedToMaster() {
            Debug.Log("OnConnectedToMaster");
            JoinRoom();
        }

        void OnJoinedLobby() {
            Debug.Log("OnJoinedLobby");
            JoinRoom();
        }

        void JoinRoom() {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 4;
            PhotonNetwork.JoinOrCreateRoom("PlayerEventSource", options, TypedLobby.Default);
        }

        void OnJoinedRoom() {
            Debug.Log("OnJoinedRoom");

            connectedCallback.Invoke();
        }

    }

}
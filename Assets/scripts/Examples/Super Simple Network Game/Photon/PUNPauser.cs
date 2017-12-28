using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine.Assertions;

namespace eventsourcing.examples.network {

    public class PUNPauser : Photon.MonoBehaviour {

        private PhotonView View;
        private Action<int, bool> ReturnGamePaused;
        private PhotonRequest<bool> PauseRequest, UnpauseRequest;

        void Start() {
            View = GetComponent<PhotonView>();
            PauseRequest = new AllPlayersPhotonRequest<bool>() {
                View = View,
                RPCName = "PauseGame",
                DetermineHasReturned = x => x
            };
            UnpauseRequest = new AllPlayersPhotonRequest<bool>() {
                View = View,
                RPCName = "UnpauseGame",
                DetermineHasReturned = x => x
            };
        }

        public void SetAllPlayersPaused(bool pause, Action finished) {
            if (ReturnGamePaused != null)
                throw new Exception("Already attempting to pause/unpause game");

            Debug.Log(pause ? "Set All Players Paused" : "Set All Players Unpaused");

            PhotonHelper.RequestFromPlayers(
                pause ? PauseRequest : UnpauseRequest,
                ref ReturnGamePaused,
                _ => {
                    Debug.Log(pause ? "All Players Paused" : "All Players Unpaused");
                    ReturnGamePaused = null;
                    finished.Invoke();
                }
            );

        }

        [PunRPC]
        private void PauseGame(int sourcePlayerID) {
            // Finish all current processes
            Debug.Log("Pause Game");
            FindObjectOfType<NetworkGameMaster>().isPlaying = false;
            // Local game no longer produces events, but can still accept new events produced by other games as they become paused

            View.RPC("PlayerPausedGame", PhotonHelper.GetPlayerByID(sourcePlayerID), PhotonNetwork.player.ID);
        }

        [PunRPC]
        private void PlayerPausedGame(int sourcePlayerID) {
            ReturnGamePaused(sourcePlayerID - 1, true);
        }

        [PunRPC]
        private void UnpauseGame(int sourcePlayerID) {
            Debug.Log("Unpause Game");
            FindObjectOfType<NetworkGameMaster>().isPlaying = true;

            View.RPC("PlayerUnpausedGame", PhotonHelper.GetPlayerByID(sourcePlayerID), PhotonNetwork.player.ID);
        }

        [PunRPC]
        private void PlayerUnpausedGame(int sourcePlayerID) {
            ReturnGamePaused(sourcePlayerID - 1, true);
        }

    }

}
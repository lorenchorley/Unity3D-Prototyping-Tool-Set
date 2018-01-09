using UnityEngine;
using System;
using photon.helpers;
using strange.extensions.injector.api;
using strange.extensions.context.impl;

namespace photon.essynchronisation {

    public class PUNGamePauser : MonoBehaviour {

        public static event Action<bool> OnPause;

        public static void Pause() {
            OnPause.Invoke(true);
        }

        public static void Unpause() {
            OnPause.Invoke(false);
        }

        public bool IsPaused;

        private PUNSynchroniser PUNESSynchroniser;

        void Start() {
            OnPause += p => IsPaused = p;
            PUNESSynchroniser = GetComponent<PUNSynchroniser>() ?? FindObjectOfType<PUNSynchroniser>() ?? gameObject.AddComponent<PUNSynchroniser>();
        }

        public void RequestPause(Action callback) {
            Action<bool> OnNextPause = null;
            OnNextPause = p => {
                if (p) {
                    OnPause -= OnNextPause;
                    callback.Invoke();
                }
            };
            OnPause += OnNextPause;

            PUNESSynchroniser.SendIndependentMod(new PauseMod());
        }

        public void RequestPause() {
            PUNESSynchroniser.SendIndependentMod(new PauseMod());
        }

        public void RequestUnpause(Action callback) {
            Action<bool> OnNextUnpause = null;
            OnNextUnpause = p => {
                if (!p) {
                    OnPause -= OnNextUnpause;
                    callback.Invoke();
                }
            };

            PUNESSynchroniser.SendIndependentMod(new UnpauseMod());
        }

        public void RequestUnpause() {
            PUNESSynchroniser.SendIndependentMod(new UnpauseMod());
        }

        //private PhotonView View;
        //private Action<int, bool> ReturnGamePaused;
        //private PhotonRequest<bool> PauseRequest, UnpauseRequest;

        //void Start() {
        //    View = GetComponent<PhotonView>();
        //    PauseRequest = new AllPlayersPhotonRequest<bool>() {
        //        View = View,
        //        RPCName = "PauseGame",
        //        DetermineHasReturned = x => x
        //    };
        //    UnpauseRequest = new AllPlayersPhotonRequest<bool>() {
        //        View = View,
        //        RPCName = "UnpauseGame",
        //        DetermineHasReturned = x => x
        //    };
        //}

        //public void SetAllPlayersPaused(bool pause, Action finished) {
        //    if (ReturnGamePaused != null)
        //        throw new Exception("Already attempting to pause/unpause game");

        //    Debug.Log(pause ? "Set All Players Paused" : "Set All Players Unpaused");

        //    PhotonHelper.RequestFromPlayers(
        //        pause ? PauseRequest : UnpauseRequest,
        //        ref ReturnGamePaused,
        //        _ => {
        //            Debug.Log(pause ? "All Players Paused" : "All Players Unpaused");
        //            ReturnGamePaused = null;
        //            finished.Invoke();
        //        }
        //    );

        //}

        //[PunRPC]
        //private void PauseGame(int sourcePlayerID) {
        //    // Finish all current processes
        //    OnPause.Invoke(true);
        //    // Local game no longer produces events, but can still accept new events produced by other games as they become paused

        //    View.RPC("PlayerPausedGame", PhotonHelper.GetPlayerByID(sourcePlayerID), PhotonNetwork.player.ID);
        //}

        //[PunRPC]
        //private void PlayerPausedGame(int sourcePlayerID) {
        //    ReturnGamePaused(sourcePlayerID - 1, true);
        //}

        //[PunRPC]
        //private void UnpauseGame(int sourcePlayerID) {
        //    OnPause.Invoke(false);

        //    View.RPC("PlayerUnpausedGame", PhotonHelper.GetPlayerByID(sourcePlayerID), PhotonNetwork.player.ID);
        //}

        //[PunRPC]
        //private void PlayerUnpausedGame(int sourcePlayerID) {
        //    ReturnGamePaused(sourcePlayerID - 1, true);
        //}

    }

}
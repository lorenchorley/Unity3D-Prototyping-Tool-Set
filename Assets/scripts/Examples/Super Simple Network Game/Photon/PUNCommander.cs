using UnityEngine;
using System;

namespace eventsourcing.examples.network {

    // do commands only at certain time intervals that change depending on ping between all players
    // measure message back and forth time to get ping, if one player detects pong time large than interval alert other players to change
    // one player sends command, all players receive command and confirm that it will take place in the next interval
    // as long as that interval is not too close that the confirmation messages cannot be sent back in time for the sender
    // to realise that they can do the command too and does not have to cancel or move foreard the command. if the sender 
    // does not gey confitmation from all players in time, the command must be delayed til the next cycle.

    // Sends arbirary commands to be executed simultaneously by all players
    public class PUNCommander : Photon.MonoBehaviour {

        private EntityManager EM;
        private PhotonView View;
        private Action<int, bool> ReturnModSent;
        private PhotonRequest<bool> DependentRequest, IndependentRequest;

        void Start() {
            EM = GetComponent<EntityManager>();
            View = GetComponent<PhotonView>();
            DependentRequest = new AllPlayersPhotonRequest<bool>() {
                View = View,
                RPCName = "DoEntityDependentMod",
                DetermineHasReturned = x => x
            };
            IndependentRequest = new AllPlayersPhotonRequest<bool>() {
                View = View,
                RPCName = "DoIndependentMod",
                DetermineHasReturned = x => x
            };
        }

        public void SendMod(IEntity e, IModifier m, Action finished) {
            _SendMod(e, m, finished); // TODO Add queueing or something, add mod ID so parallel mods can be processed
        }

        public void SendMod(IModifier m, Action finished) {
            _SendMod(m, finished);
        }

        private void _SendMod(IEntity e, IModifier c, Action finished) {
            if (ReturnModSent != null)
                throw new Exception("Already attempting to compare game hashes");

            Debug.Log("Send Entity Mod");

            PhotonHelper.RequestFromPlayers(
                DependentRequest,
                ref ReturnModSent,
                _ => {
                    Debug.Log("Mod sent to all players, can now do the mod locally");
                    ReturnModSent = null;
                    finished.Invoke();
                }, 
                e, c
            );

        }

        private void _SendMod(IModifier m, Action finished) {
            if (ReturnModSent != null)
                throw new Exception("Already attempting to compare game hashes");

            Debug.Log("Send Independent Mod");

            PhotonHelper.RequestFromPlayers(
                IndependentRequest,
                ref ReturnModSent,
                _ => {
                    Debug.Log("Mod sent to all players, can now do the mod locally");
                    ReturnModSent = null;
                    finished.Invoke();
                },
                m
            );
        }

        [PunRPC]
        private void DoEntityDependentMod<E, M>(int sourcePlayerID, E e, M m) where M : IModifier where E : IEntity, IModifiable<M> {
            EM.Mod(e, m);
            View.RPC("ConfirmModExecuted", PhotonNetwork.playerList[sourcePlayerID - 1], PhotonNetwork.player.ID);
        }

        [PunRPC]
        private void DoIndependentMod<E, M>(int sourcePlayerID, M m) where M : IModifier {
            EM.Mod(m);
            View.RPC("ConfirmModExecuted", PhotonNetwork.playerList[sourcePlayerID - 1], PhotonNetwork.player.ID);
        }

        [PunRPC]
        private void ConfirmModExecuted(int sourcePlayerID) {
            ReturnModSent(sourcePlayerID - 1, true); // TODO Need a mod ID to be returned
        }

    }

}
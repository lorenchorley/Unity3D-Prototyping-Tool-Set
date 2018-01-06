using UnityEngine;
using System;
using System.Collections.Generic;
using entitymanagement;

namespace eventsourcing.examples.network {

    // do commands only at certain time intervals that change depending on ping between all players
    // measure message back and forth time to get ping, if one player detects pong time large than interval alert other players to change
    // one player sends command, all players receive command and confirm that it will take place in the next interval
    // as long as that interval is not too close that the confirmation messages cannot be sent back in time for the sender
    // to realise that they can do the command too and does not have to cancel or move foreard the command. if the sender 
    // does not gey confitmation from all players in time, the command must be delayed til the next cycle.

    // Sends arbirary commands to be executed simultaneously by all players
    public class PUNCommandSyncer : Photon.MonoBehaviour {

        public float MaxPing;

        private EntityManager EM;
        private PhotonView View;
        private Action<int, bool> ReturnModSent;
        private PhotonRequest<bool> DependentRequest, IndependentRequest;
        private Pinger Pinger;

        // TODO Integrate a response timer to gather pings, can send an empty command to gather pings if needed, otherwise piggy back on existing commands
        // Keep track of and synchronise intervals at the end of which the commands are executed
        // Intervals are determined by the max ping over all clients
        // If a client fails to execute a command in the correct interval, it is broadcast that the command must be revert and replayed in the next interval instead
        //   This too must a command that can fail
        // Must check that the next end of interval is not too close that the confirmation can't make it back in time
        //   If so start a failure early to avoid letting the command sender deal with the failure a step later

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
            Pinger = new Pinger(1000, RequestIndividualPlayerPing);
        }

        void Update() {
            MaxPing = Pinger.MaxPing;
        }

        #region Individual ping request
        private void RequestIndividualPlayerPing(int PhotonPlayerID) {
            int requestID = Pinger.RecordPingRequestStartedForPlayer(PhotonPlayerID);
            View.RPC("ReceiveIndividualPingRequest", PhotonHelper.GetPlayerByID(PhotonPlayerID), PhotonNetwork.player.ID, requestID);
        }

        [PunRPC]
        private void ReceiveIndividualPingRequest<E, M>(int sourcePlayerID, int requestID) where M : IEntityModifier {
            View.RPC("ConfirmPingRequest", PhotonNetwork.playerList[sourcePlayerID - 1], PhotonNetwork.player.ID, requestID);
        }

        [PunRPC]
        private void ConfirmPingRequest(int sourcePlayerID, int requestID) {
            Pinger.RecordPingReceived(sourcePlayerID, requestID);
        }
        #endregion

        #region Send mod to all players
        public void SendMod(IModifier m, Action finished) {
            if (m is IEntityModifier)
                SendEntityMod(m as IEntityModifier, finished);
            else if (m is IIndependentModifier)
                SendIndependentMod(m as IIndependentModifier, finished);
            else throw new Exception("Unknown mod type: " + m.GetType().Name);
        }

        public void SendEntityMod(IEntityModifier m, Action finished) {
            if (ReturnModSent != null)
                throw new Exception("Already attempting to compare game hashes");

            Debug.Log("Send Entity Mod");

            int requestID = Pinger.RecordMassPingRequestStarted();

            PhotonHelper.RequestFromPlayers(
                DependentRequest,
                ref ReturnModSent,
                _ => {
                    Debug.Log("Mod sent to all players, can now do the mod locally");
                    ReturnModSent = null;
                    finished.Invoke();
                },
                m, requestID
            );

        }

        public void SendIndependentMod(IIndependentModifier m, Action finished) {
            if (ReturnModSent != null)
                throw new Exception("Already attempting to compare game hashes");

            Debug.Log("Send Independent Mod");

            int requestID = Pinger.RecordMassPingRequestStarted();

            PhotonHelper.RequestFromPlayers(
                IndependentRequest,
                ref ReturnModSent,
                _ => {
                    Debug.Log("Mod sent to all players, can now do the mod locally at the end of the next interval");
                    ReturnModSent = null;
                    finished.Invoke();
                },
                m, requestID
            );
        }

        [PunRPC]
        private void DoEntityDependentMod<E, M>(int sourcePlayerID, E e, M m, int requestID) where M : IEntityModifier where E : IEntity, IModifiable<M> {
            EM.ApplyEntityMod(e, m);
            View.RPC("ConfirmModExecuted", PhotonNetwork.playerList[sourcePlayerID - 1], PhotonNetwork.player.ID, requestID);
        }

        [PunRPC]
        private void DoIndependentMod<E, M>(int sourcePlayerID, M m, int requestID) where M : IIndependentModifier {
            EM.ApplyMod(m);
            View.RPC("ConfirmModExecuted", PhotonNetwork.playerList[sourcePlayerID - 1], PhotonNetwork.player.ID, requestID);
        }

        [PunRPC]
        private void ConfirmModExecuted(int sourcePlayerID, int requestID) {
            ReturnModSent(sourcePlayerID - 1, true); // TODO Need a mod ID to be returned
            Pinger.RecordPingReceived(sourcePlayerID, requestID);
        }
        #endregion

    }

}
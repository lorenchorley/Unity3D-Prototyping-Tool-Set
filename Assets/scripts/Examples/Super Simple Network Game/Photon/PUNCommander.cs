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

        private EventSource ES;
        private PhotonView View;
        private Action<int, bool> ReturnCommandSent;
        private PhotonRequest<bool> DependentRequest, IndependentRequest;

        void Start() {
            ES = GetComponent<EventSource>();
            View = GetComponent<PhotonView>();
            DependentRequest = new AllPlayersPhotonRequest<bool>() {
                View = View,
                RPCName = "DoEntityDependentCommand",
                DetermineHasReturned = x => x
            };
            IndependentRequest = new AllPlayersPhotonRequest<bool>() {
                View = View,
                RPCName = "DoIndependentCommand",
                DetermineHasReturned = x => x
            };
        }

        public void SendCommand(IEntity e, IModifier c, Action finished) {
            _SendCommand(e, c, finished); // TODO Add queueing or something, add command ID so parallel commands can be processed
        }

        public void SendCommand(IndependentModifier c, Action finished) {
            _SendCommand(c, finished);
        }

        private void _SendCommand(IEntity e, IModifier c, Action finished) {
            if (ReturnCommandSent != null)
                throw new Exception("Already attempting to compare game hashes");

            Debug.Log("Send Entity Command");

            PhotonHelper.RequestFromPlayers(
                DependentRequest,
                ref ReturnCommandSent,
                _ => {
                    Debug.Log("Command sent to all players, can now do the command locally");
                    ReturnCommandSent = null;
                    finished.Invoke();
                }, 
                e, c
            );

        }

        private void _SendCommand(IndependentModifier c, Action finished) {
            if (ReturnCommandSent != null)
                throw new Exception("Already attempting to compare game hashes");

            Debug.Log("Send Independent Command");

            PhotonHelper.RequestFromPlayers(
                IndependentRequest,
                ref ReturnCommandSent,
                _ => {
                    Debug.Log("Command sent to all players, can now do the command locally");
                    ReturnCommandSent = null;
                    finished.Invoke();
                },
                c
            );
        }

        [PunRPC]
        private void DoEntityDependentCommand<E, C>(int sourcePlayerID, E e, C c) where C : IModifier where E : IEntity, IModifiable<C> {
            ES.Command(e, c);
            View.RPC("ConfirmCommandExecuted", PhotonNetwork.playerList[sourcePlayerID - 1], PhotonNetwork.player.ID);
        }

        [PunRPC]
        private void DoIndependentCommand<E, C>(int sourcePlayerID, C c) where C : IndependentModifier {
            ES.Command(c);
            View.RPC("ConfirmCommandExecuted", PhotonNetwork.playerList[sourcePlayerID - 1], PhotonNetwork.player.ID);
        }

        [PunRPC]
        private void ConfirmCommandExecuted(int sourcePlayerID) {
            ReturnCommandSent(sourcePlayerID - 1, true); // TODO Need a command ID to be returned
        }

    }

}
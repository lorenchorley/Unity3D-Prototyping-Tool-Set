using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using eventsourcing;
using photon.helpers;

namespace photon.essynchronisation {

    public class PUNHashChecker : Photon.MonoBehaviour {

        private EventSource ES;
        private PhotonView View;
        private Action<int, int?> ReturnHashes; 
        private PhotonRequest<int?> Request;

        void Start() {
            ES = GetComponent<EventSource>();
            View = GetComponent<PhotonView>();
            Request = new AllPlayersPhotonRequest<int?>() {
                View = View,
                RPCName = "HashCheck",
                DetermineHasReturned = x => x.HasValue,
                SerialiseArgsArray = false
            };
        }

        public void RequestHashCheck(Action finished) {
            if (ReturnHashes != null)
                throw new Exception("Already attempting to compare game hashes");

            PhotonHelper.RequestFromPlayers(
                Request,
                r => ReturnHashes = r,
                xs => {
                    HashSet<int> hashes = new HashSet<int>();

                    for (int i = 0; i < xs.Count; i++) {
                        Assert.IsTrue(xs[i].HasValue);
                        hashes.Add(xs[i].Value);
                    }

                    if (hashes.Count != 1)
                        throw new Exception("Hash check failed, there were " + hashes.Count + " different hashes");

                    ReturnHashes = null;
                    finished.Invoke();
                },
                PhotonNetwork.player.ID
            );

        }

        [PunRPC]
        private void HashCheck(int sourcePlayerID) {
            HashProjection hasher = new HashProjection(ES);
            ES.ApplyProjection(hasher, EventStream.AllExistingEvents);
            View.RPC("HashCheckReturn", PhotonHelper.GetPlayerByID(sourcePlayerID), PhotonNetwork.player.ID, hasher.GenerateHashCode(), ES.EventCount);
        }

        [PunRPC]
        private void HashCheckReturn(int sourcePlayerID, int hashcode, int eventCount) {
            Debug.Log("Player " + sourcePlayerID + " returned hash " + hashcode + " derived from " + eventCount + " events.");
            ReturnHashes(sourcePlayerID - 1, hashcode);
        }

    }

}
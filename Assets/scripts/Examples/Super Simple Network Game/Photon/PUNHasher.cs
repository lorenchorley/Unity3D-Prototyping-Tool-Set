using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine.Assertions;

namespace eventsourcing.examples.network {

    public class PUNHasher : Photon.MonoBehaviour {

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
                DetermineHasReturned = x => x.HasValue
            };
        }

        public void RequestHashCheck(Action finished) {
            if (ReturnHashes != null)
                throw new Exception("Already attempting to compare game hashes");

            Debug.Log("Request Hash Check");

            PhotonHelper.RequestFromPlayers(
                Request,
                ref ReturnHashes,
                xs => {
                    HashSet<int> hashes = new HashSet<int>();

                    for (int i = 0; i < xs.Length; i++) {
                        Assert.IsTrue(xs[i].HasValue);
                        hashes.Add(xs[i].Value);
                    }

                    if (hashes.Count != 1)
                        throw new Exception("Hash check failed, there were " + hashes.Count + " different hashes");

                    Debug.Log("Hashes Checked");
                    ReturnHashes = null;
                    finished.Invoke();
                }
            );

        }

        [PunRPC]
        private void HashCheck(int sourcePlayerID) {
            HashProjection hasher = new HashProjection(ES);
            ES.ApplyProjection(hasher);

            Debug.Log("HashCheck: " + hasher.GetHashCode());

            View.RPC("HashCheckReturn", PhotonHelper.GetPlayerByID(sourcePlayerID), PhotonNetwork.player.ID, hasher.GetHashCode());
        }

        [PunRPC]
        private void HashCheckReturn(int sourcePlayerID, int hashcode) {
            ReturnHashes(sourcePlayerID - 1, hashcode);
        }

    }

}
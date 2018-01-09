using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine.Assertions;
using eventsourcing;
using photon.helpers;

namespace photon.essynchronisation {

    // PUNESSynchroniser perhaps
    public class PUNStateExchanger : Photon.MonoBehaviour {

        private EventSource ES;
        private PhotonView View;
        private Action<byte[]> ReturnedESData;

        void Start() {
            ES = GetComponent<EventSource>();
            View = GetComponent<PhotonView>();
        }

        public void RequestESAndImport(Action finished) {
            RequestES(bx => {
                ES.ResetWithByteData(bx);
                finished.Invoke();
            });
        }

        public void RequestES(Action<byte[]> finished) {
            if (ReturnedESData != null)
                throw new Exception("Already attempting to get event source data");

            ReturnedESData = bx => finished.Invoke(bx);

            View.RPC("RequestESData", PhotonHelper.SelectRandomPlayer(), PhotonNetwork.player.ID);
        }

        [PunRPC]
        private void RequestESData(int sourcePlayerID) {
            ES.ExtractByteData(bx => View.RPC("AcceptESData", PhotonHelper.GetPlayerByID(sourcePlayerID), bx));
        }

        [PunRPC]
        private void AcceptESData(byte[] bx) {
            ReturnedESData(bx);
        }

    }

}
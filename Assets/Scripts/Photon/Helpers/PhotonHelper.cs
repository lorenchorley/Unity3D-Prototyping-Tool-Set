using UniRx;
using System;
using System.Text;
using System.Collections.Generic;

namespace photon.helpers {

    public static class PhotonHelper {

        public static PhotonPlayer GetPlayerByID(int id) {
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {
                if (PhotonNetwork.playerList[i].ID == id)
                    return PhotonNetwork.playerList[i];
            }
            throw new Exception("No photon player with id: " + id);
        }

        public static PhotonPlayer SelectRandomOtherPlayer() {
            return PhotonNetwork.otherPlayers[UnityEngine.Random.Range(0, PhotonNetwork.otherPlayers.Length - 1)];
        }

        public static PhotonPlayer SelectRandomPlayer() {
            return PhotonNetwork.playerList[UnityEngine.Random.Range(0, PhotonNetwork.playerList.Length - 1)];
        }

        public static void RequestFromPlayers<T>(PhotonRequest<T> request, Action<Action<int, T>> ValueReturnedCallback, Action<List<T>> FinishedCallback, params object[] args) {
            ReactiveProperty<List<T>> props = new ReactiveProperty<List<T>>();
            props.Value = new List<T>();
            
            // Give back a method that can be called to inform that a value has been received
            ValueReturnedCallback.Invoke((i, v) => {
                List<T> a = props.Value;
                a.Add(v); // Ignore the index for now
                props.SetValueAndForceNotify(a);
            });

            bool finished = false; // Safegaurd
            Action<List<T>> doMethod;
            doMethod = a => {
                if (finished)
                    throw new Exception("Already finished");

                if (!AreAllRequestsReceived(request.Players, (List<T>) props.Value))
                    return;

                // Old, to remove
                //for (int i = 0; i < a.Count; i++) {
                //    if (!request.DetermineHasReturned(data.Value[i])) {
                //        return;
                //    }
                //}

                // TODO Clean up disconnect callback
                // OnPlayerDisconnect -= doMethod;

                finished = true;
                FinishedCallback.Invoke(a);
            };

            // TODO Deal with disconnecting players by calling doMethod again when they do
            // OnPlayerDisconnect += doMethod;

            // TODO Some kind of check to see if it's taking too long, to pause the game

            // Sub to reactive properties
            props.Do(doMethod).Subscribe();

            if (args == null || args.Length == 0)
                request.View.RPC(request.RPCName, request.Players);
            else if (request.SerialiseArgsArray) {
                object[] serialisedArgs = new object[args.Length];
                for (int i = 0; i < args.Length; i++) {
                    serialisedArgs[i] = Serialisation.ToBinary(args[i]);
                }
                // Convert to object[] where each element is a byte[], then convert back to object[] on the other side
                // Serialise each array object individually and send object[] instead since RPC takes object[]
                request.View.RPC(request.RPCName, request.Players, serialisedArgs);
            } else
                request.View.RPC(request.RPCName, request.Players, args);
        }

        private static bool AreAllRequestsReceived<T>(PhotonTargets Players, List<T> returned) {
            switch (Players) {
            case PhotonTargets.All:
            case PhotonTargets.AllBuffered:
            case PhotonTargets.AllBufferedViaServer:
            case PhotonTargets.AllViaServer:
                return returned.Count >= PhotonNetwork.playerList.Length;
            case PhotonTargets.MasterClient:
                return returned.Count >= 1;
            case PhotonTargets.Others:
            case PhotonTargets.OthersBuffered:
                return returned.Count >= PhotonNetwork.otherPlayers.Length;
            default:
                throw new Exception("Unknown player target: " + Players.ToString());
            }
        }

    }

}
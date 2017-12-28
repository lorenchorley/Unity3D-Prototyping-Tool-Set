using UniRx;
using System;

namespace eventsourcing.examples.network {

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
        
        public static void RequestFromPlayers<T>(PhotonRequest<T> request, ref Action<int, T> ValueReturnedCallback, Action<T[]> FinishedCallback, params object[] args) {
            ReactiveProperty<T[]> data = new ReactiveProperty<T[]>();

            switch (request.Players) {
            case PhotonTargets.All:
            case PhotonTargets.AllBuffered:
            case PhotonTargets.AllBufferedViaServer:
            case PhotonTargets.AllViaServer:
                data.Value = new T[PhotonNetwork.playerList.Length];
                break;
            case PhotonTargets.MasterClient:
                data.Value = new T[1];
                break;
            case PhotonTargets.Others:
            case PhotonTargets.OthersBuffered:
                data.Value = new T[PhotonNetwork.otherPlayers.Length];
                break;
            }

            for (int i = 0; i < data.Value.Length; i++)
                data.Value[i] = default(T);

            ValueReturnedCallback = (i, v) => {
                T[] a = data.Value;
                a[i] = v;
                data.SetValueAndForceNotify(a);
            };

            bool finished = false;
            data.Do(a => {
                if (finished)
                    throw new Exception("Already finished");

                for (int i = 0; i < a.Length; i++) {
                    if (!request.DetermineHasReturned(data.Value[i])) {
                        return;
                    }
                }

                finished = true;
                FinishedCallback.Invoke(a);
            }).Subscribe();

            request.View.RPC(request.RPCName, request.Players, PhotonNetwork.player.ID, args);
        }

    }

    public abstract class PhotonRequest<T> {
        public PhotonView View;
        public string RPCName;
        public PhotonTargets Players { get; protected set; }
        public Func<T, bool> DetermineHasReturned;
    }

    public class OtherPlayerPhotonRequest<T> : PhotonRequest<T> {
        public OtherPlayerPhotonRequest() {
            Players = PhotonTargets.Others;
        }
    }

    public class AllPlayersPhotonRequest<T> : PhotonRequest<T> {
        public AllPlayersPhotonRequest() {
            Players = PhotonTargets.All;
        }
    }

    public class MasterClientPhotonRequest<T> : PhotonRequest<T> {
        public MasterClientPhotonRequest() {
            Players = PhotonTargets.MasterClient;
        }
    }

}
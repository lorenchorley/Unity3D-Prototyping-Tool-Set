using UniRx;
using System;

namespace eventsource.examples.network {

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

        public static void RequestFromAllPlayers<T>(ref Action<int, T> SetReturnedValue, Action startAction, Func<T, bool> ReturnedCompare, Action<T[]> finishedCallback) {
            RequestFromPlayers<T>(ref SetReturnedValue, PhotonTargets.All, startAction, ReturnedCompare, finishedCallback);
        }

        public static void RequestFromOtherPlayers<T>(ref Action<int, T> SetReturnedValue, Action startAction, Func<T, bool> ReturnedCompare, Action<T[]> finishedCallback) {
            RequestFromPlayers<T>(ref SetReturnedValue, PhotonTargets.Others, startAction, ReturnedCompare, finishedCallback);
        }

        public static void RequestFromPlayers<T>(ref Action<int, T> SetReturnedValue,
                                                 PhotonTargets players,
                                                 Action startAction,
                                                 Func<T, bool> ReturnedCompare,
                                                 Action<T[]> finishedCallback) {

            ReactiveProperty<T[]> data = new ReactiveProperty<T[]>();

            switch (players) {
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

            SetReturnedValue = (i, v) => {
                T[] a = data.Value;
                a[i] = v;
                data.SetValueAndForceNotify(a);
            };

            bool finished = false;
            data.Do(a => {
                if (finished)
                    throw new Exception("Already finished");

                for (int i = 0; i < a.Length; i++) {
                    if (!ReturnedCompare(data.Value[i])) {
                        return;
                    }
                }

                finished = true;
                finishedCallback.Invoke(a);
            }).Subscribe();

            startAction.Invoke();
        }

    }

}
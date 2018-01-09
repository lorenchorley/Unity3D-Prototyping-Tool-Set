using UniRx;
using System;

namespace photon.helpers {
    
    public abstract class PhotonRequest<T> {
        public PhotonView View;
        public string RPCName;
        public PhotonTargets Players { get; protected set; }
        public Func<T, bool> DetermineHasReturned;
        public bool SerialiseArgsArray = true;
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
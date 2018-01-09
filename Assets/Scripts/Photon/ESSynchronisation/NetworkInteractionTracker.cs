using UniRx;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace photon.essynchronisation {

    public class NetworkInteractionTracker {

        private float TicksPerMillisecond = TimeSpan.TicksPerMillisecond;

        private struct PingProfile {
            public long TimeOfLastSentPingRequest; // In ticks
            public int PingRequestID;
            public float LatestPingMS;
            public bool HasBeenPinged;
        }

        private class PingRequest {
            public int RequestID;
            public int PhotonPlayerID;
            public long Sent; // In ticks

            public override int GetHashCode() {
                return (RequestID + PhotonPlayerID + Sent).GetHashCode();
            }
        }

        private class PingRequestEqualityComparer : EqualityComparer<PingRequest> {

            public override bool Equals(PingRequest x, PingRequest y) {
                return x.PhotonPlayerID == y.PhotonPlayerID;
            }

            public override int GetHashCode(PingRequest obj) {
                return obj.GetHashCode();
            }

        }

        private event Action OnRequestsCleared;
        private PingProfile[] pingProfiles;
        private HashSet<PingRequest> pingRequests;
        private float maxPing;
        private bool maxPingOutOfDate = true;
        private Action<int> requestPingOfPlayer;
        private float repingPeriodMS;
        private int pingRequestIDCounter;

        public NetworkInteractionTracker(float repingPeriodMS, Action<int> requestPingOfPlayer) {
            this.repingPeriodMS = repingPeriodMS;
            this.requestPingOfPlayer = requestPingOfPlayer;
            pingRequestIDCounter = 0;
            pingRequests = new HashSet<PingRequest>(new PingRequestEqualityComparer());
            pingProfiles = new PingProfile[0];
            OnRequestsCleared = delegate { };
            ExpandArrayIfNecessary();
        }

        public int RecordMassPingRequestStarted() {
            ExpandArrayIfNecessary();

            DateTime now = DateTime.Now;
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {
                int photonPlayerID = PhotonNetwork.playerList[i].ID;

                UpdateProfile(photonPlayerID, profile => {
                    profile.TimeOfLastSentPingRequest = now.Ticks;
                    profile.PingRequestID = pingRequestIDCounter;
                    return profile;
                });

                PingRequest request = new PingRequest() {
                    PhotonPlayerID = photonPlayerID,
                    RequestID = pingRequestIDCounter,
                    Sent = now.Ticks
                };
                
                pingRequests.Add(request);
                
            }

            return pingRequestIDCounter++;
        }

        public int RecordPingRequestStartedForPlayer(int photonPlayerID) {
            ExpandArrayIfNecessary();

            UpdateProfile(photonPlayerID, profile => {
                profile.TimeOfLastSentPingRequest = DateTime.Now.Ticks;
                profile.PingRequestID = pingRequestIDCounter;

                return profile;
            });

            pingRequests.Add(new PingRequest() {
                PhotonPlayerID = photonPlayerID,
                RequestID = pingRequestIDCounter,
                Sent = DateTime.Now.Ticks
            });
            
            return pingRequestIDCounter++;
        }

        public void RecordPingReceived(int photonPlayerID, int requestID) {
            bool requestIsLatest = false;
            foreach (PingRequest request in pingRequests) {
                if (request.PhotonPlayerID == photonPlayerID) {
                    requestIsLatest = request.RequestID == requestID;
                    Assert.IsTrue(requestIsLatest); // Should always be true because of hashset
                    break;
                }
            }

            if (requestIsLatest) {
                UpdateProfile(photonPlayerID, profile => {
                    profile.HasBeenPinged = true;
                    profile.LatestPingMS = MillisecondsSince(profile.TimeOfLastSentPingRequest);
                    return profile;
                });

                int count = pingRequests.RemoveWhere(p => p.PhotonPlayerID == photonPlayerID);
                Assert.IsTrue(count == 1);
                maxPingOutOfDate = true;

                CheckRequestsCleared();

                //Debug.LogWarning("Ping registered for photon player " + photonPlayerID + ": " + pingProfiles[photonPlayerID].LatestPingMS);

            }
        }

        public List<int> PlayersYetNotPinged() {
            List<int> list = new List<int>();
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {
                if (PhotonNetwork.playerList[i].ID + 1 > pingProfiles.Length ||
                    !pingProfiles[PhotonNetwork.playerList[i].ID].HasBeenPinged) {
                    list.Add(PhotonNetwork.playerList[i].ID);
                }
            }
            return list;
        }

        public void PerformPeriodicPingCheck() {
            if (WaitingOnPingRequests)
                return;

            ExpandArrayIfNecessary();
            for (int id = 0; id < pingProfiles.Length; id++) {
                PingProfile profile = pingProfiles[id];

                if (id == 0)
                    Assert.IsFalse(IsPlayerConnected(id));

                if (profile.HasBeenPinged) {
                    if (IsPlayerConnected(id) && IsPlayerPingOld(profile))
                        requestPingOfPlayer.Invoke(id);
                } else {
                    if (IsPlayerConnected(id))
                        requestPingOfPlayer.Invoke(id);
                }

            }
        }

        public void RefreshMaxPing() {
            maxPingOutOfDate = true;
        }

        public float MaxPing {
            get {
                if (maxPingOutOfDate) {
                    maxPing = 0;

                    for (int id = 1; id < pingProfiles.Length; id++) {
                        PingProfile profile = pingProfiles[id];

                        //Assert.IsTrue(profile.HasBeenPinged, "Player has not been pinged: " + id);

                        if (profile.HasBeenPinged) {
                            maxPing = Mathf.Max(maxPing, profile.LatestPingMS);
                        } else {
                            if (IsPlayerConnected(id))
                                requestPingOfPlayer.Invoke(id);
                        }

                    }

                    maxPingOutOfDate = false;
                }

                return maxPing;
            }
        }

        private void CheckRequestsCleared() {
            if (pingRequests.Count == 0) {
                OnRequestsCleared.Invoke();
                OnRequestsCleared = delegate { };
            }
        }

        public void WaitUntilCleared(Action a) {
            if (pingRequests.Count == 0) {
                a.Invoke();
            } else {
                OnRequestsCleared += a;
            }
        }

        public bool WaitingOnPingRequests {
            get {
                return pingRequests.Count > 0;
            }
        }

        private bool IsPlayerPingOld(PingProfile profile) {
            return profile.TimeOfLastSentPingRequest / TicksPerMillisecond + profile.LatestPingMS > repingPeriodMS;
        }

        private bool IsPlayerConnected(int photonPlayerID) {
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {
                if (PhotonNetwork.playerList[i].ID == photonPlayerID) {
                    return !PhotonNetwork.playerList[i].IsInactive;
                }
            }
            return false;
        }

        private float MillisecondsSince(long then) {
            return ((float) (DateTime.Now.Ticks - then)) / TicksPerMillisecond;
        }

        private void UpdateProfile(int photonPlayerID, Func<PingProfile, PingProfile> f) {
            pingProfiles[photonPlayerID] = f.Invoke(pingProfiles[photonPlayerID]);
        }

        private void ExpandArrayIfNecessary() {
            int greatestID = -1;

            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                greatestID = Math.Max(greatestID, PhotonNetwork.playerList[i].ID);

            if (greatestID + 1 > pingProfiles.Length)
                Array.Resize(ref pingProfiles, greatestID + 1);

        }

    }

}
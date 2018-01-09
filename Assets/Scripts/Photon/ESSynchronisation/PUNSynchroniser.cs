using UnityEngine;
using System;
using System.Collections.Generic;
using entitymanagement;
using UnityEngine.Assertions;
using photon.helpers;
using eventsourcing;
using System.Collections;
using UnityEngine.UI;

namespace photon.essynchronisation {

    // do commands only at certain time intervals that change depending on ping between all players
    // measure message back and forth time to get ping, if one player detects pong time large than interval alert other players to change
    // one player sends command, all players receive command and confirm that it will take place in the next interval
    // as long as that interval is not too close that the confirmation messages cannot be sent back in time for the sender
    // to realise that they can do the command too and does not have to cancel or move foreard the command. if the sender 
    // does not gey confitmation from all players in time, the command must be delayed til the next cycle.

    // Sends arbirary commands to be executed simultaneously by all players
    [RequireComponent(typeof(PhotonView))]
    public class PUNSynchroniser : Photon.MonoBehaviour {

        [Header("Parameters")]
        public int PingIntervalMS = 5000;
        public float DefaultIntervalMS = 100;

        [Header("Info")]
        public float MaxPing;
        public Text PingText;
        public bool PrintAllMods = false;

        public PhotonView View { get; private set; }
        public PUNHashChecker PUNHasher { get; private set; }
        public PUNStateExchanger PUNESRequester { get; private set; }
        public PUNGamePauser PUNPauser { get; private set; }
        public IntervalTimer IntervalTimer { get; private set; }
        public EventSource ES { get; private set; }
        public EntityManager EM { get; private set; }
        public NetworkInteractionTracker NetworkInteractionTracker { get; private set; }

        public PingSimulator PingSimulator; // TODO Remove

        private Dictionary<int, Action<int, bool>> ReturnModSentByRequestID;
        private PhotonRequest<bool> DependentRequest, IndependentRequest;
        private Action<IModifier, Action<IModifier>> DistributeBetweenPlayersAsyncFilter;

        // TODO Integrate a response timer to gather pings, can send an empty command to gather pings if needed, otherwise piggy back on existing commands
        // Keep track of and synchronise intervals at the end of which the commands are executed
        // Intervals are determined by the max ping over all clients
        // If a client fails to execute a command in the correct interval, it is broadcast that the command must be revert and replayed in the next interval instead
        //   This too must a command that can fail
        // Must check that the next end of interval is not too close that the confirmation can't make it back in time
        //   If so start a failure early to avoid letting the command sender deal with the failure a step later

        void Awake() {
            View = GetComponent<PhotonView>();
            EM = GetComponent<EntityManager>() ?? FindObjectOfType<EntityManager>() ?? gameObject.AddComponent<EntityManager>();
            ES = GetComponent<EventSource>() ?? FindObjectOfType<EventSource>() ?? gameObject.AddComponent<EventSource>();
            NetworkInteractionTracker = new NetworkInteractionTracker(PingIntervalMS, RequestIndividualPlayerPing);
            IntervalTimer = GetComponent<IntervalTimer>() ?? FindObjectOfType<IntervalTimer>() ?? gameObject.AddComponent<IntervalTimer>();
            PUNHasher = GetComponent<PUNHashChecker>() ?? FindObjectOfType<PUNHashChecker>() ?? gameObject.AddComponent<PUNHashChecker>();
            PUNESRequester = GetComponent<PUNStateExchanger>() ?? FindObjectOfType<PUNStateExchanger>() ?? gameObject.AddComponent<PUNStateExchanger>();
            PUNPauser = GetComponent<PUNGamePauser>() ?? FindObjectOfType<PUNGamePauser>() ?? gameObject.AddComponent<PUNGamePauser>();

            PingSimulator = gameObject.AddComponent<PingSimulator>();
            PingSimulator.Average = 500;
            PingSimulator.Variation = 200;
            //PingSimulator.enabled = false; // To disable

            // Does not pass value onto the original call, passes null instead which cancels it
            // The mod is then only applied through the RPC call where the filter is removed
            DistributeBetweenPlayersAsyncFilter = (e, a) => {
                if (e != null)
                    SendMod(e);
                a.Invoke(null);
            };

            if (PrintAllMods)
                EM.Filters.Add((e, a) => {
                    Debug.Log("Mod applied: " + e.GetType().Name);
                    a.Invoke(e);
                });

            IntervalTimer.Init(ES, EM, DistributeBetweenPlayersAsyncFilter);
            IntervalTimer.OnStep += s => { };

            DependentRequest = new AllPlayersPhotonRequest<bool>() {
                View = View,
                RPCName = "DoEntityDependentModWithConfirmation",
                DetermineHasReturned = x => x
            };

            IndependentRequest = new AllPlayersPhotonRequest<bool>() {
                View = View,
                RPCName = "DoIndependentModWithConfirmation",
                DetermineHasReturned = x => x
            };

            ReturnModSentByRequestID = new Dictionary<int, Action<int, bool>>();

        }

        private void Update() {
            if (!NetworkInteractionTracker.WaitingOnPingRequests) {
                MaxPing = NetworkInteractionTracker.MaxPing;
                if (PingText != null)
                    PingText.text = "Ping: " + MaxPing.ToString("0ms");
            }
        }

        IEnumerator PingCheck() {
            while (true) {
                if (!NetworkInteractionTracker.WaitingOnPingRequests)
                    NetworkInteractionTracker.PerformPeriodicPingCheck();

                yield return new WaitForSecondsRealtime(PingIntervalMS / 1000);
            }
        }

        public void SetupAndSynchronise(Action onSynchronisedAndReady) {
            if (!PhotonNetwork.inRoom)
                throw new Exception("Not connected to photon, could not start synchronisation");

            if (PhotonNetwork.otherPlayers.Length == 0) {
                Debug.Log("PhotonNetwork.otherPlayers.Length == 0");

                // Setup mod filter to intercept mods and distribute them before applying them to all instances at once
                EM.Filters.Add(DistributeBetweenPlayersAsyncFilter);

                IntervalTimer.StartTimer(DateTime.Now, TimeSpan.FromMilliseconds(DefaultIntervalMS));
                StartCoroutine(PingCheck());
                onSynchronisedAndReady.Invoke();
                return;
            }

            Action PauseAll = null, RequestES = null, CheckHash = null, SetupLocally = null, UnpauseAll = null;

            // Pause game for all players
            PauseAll = () => PUNPauser.RequestPause(() => NetworkInteractionTracker.WaitUntilCleared(RequestES));

            // Request ES data from one player
            RequestES = () => PUNESRequester.RequestESAndImport(CheckHash);

            // Do hashcode check on data for all players
            CheckHash = () => PUNHasher.RequestHashCheck(SetupLocally);

            SetupLocally = () => {

                // Setup mod filter to intercept mods and distribute them before applying them to all instances at once
                EM.Filters.Add(DistributeBetweenPlayersAsyncFilter);

                NetworkInteractionTracker.WaitUntilCleared(UnpauseAll);
            };

            // Unpause game for all players
            UnpauseAll = () => PUNPauser.RequestUnpause(() => {
                IntervalTimer.StartTimer(
                    ES.GetFirstEventOfType<IntervalStartedEvent>(),
                    ES.GetLastEventOfType<IntervalStepEvent>(),
                    ES.GetLastEventOfType<IntervalChangedEvent>()
                    );
                //IntervalTimer.StartTimer(DateTime.Now, TimeSpan.FromMilliseconds(DefaultIntervalMS));

                StartCoroutine(PingCheck());
                onSynchronisedAndReady();
            });

            // Start
            PauseAll.Invoke();

        }

        #region Individual ping request
        public void RequestMassPlayerPing() {
            int requestID = NetworkInteractionTracker.RecordMassPingRequestStarted();
            View.RPC("ReceivePingRequest", PhotonTargets.All, PhotonNetwork.player.ID, requestID);
        }

        private void RequestIndividualPlayerPing(int PhotonPlayerID) {
            int requestID = NetworkInteractionTracker.RecordPingRequestStartedForPlayer(PhotonPlayerID);
            View.RPC("ReceivePingRequest", PhotonHelper.GetPlayerByID(PhotonPlayerID), PhotonNetwork.player.ID, requestID);
        }

        [PunRPC]
        private void ReceivePingRequest(int sourcePlayerID, int requestID) {
            PingSimulator.Delay(() => {
                View.RPC("ConfirmPingRequest", PhotonNetwork.playerList[sourcePlayerID - 1], PhotonNetwork.player.ID, requestID);
            });
        }

        [PunRPC]
        private void ConfirmPingRequest(int sourcePlayerID, int requestID) {
            PingSimulator.Delay(() => {
                NetworkInteractionTracker.RecordPingReceived(sourcePlayerID, requestID);
            });
        }
        #endregion

        #region Send mod to all players with no confirmation
        public void SendMod(IModifier m) {
            if (m is IEntityModifier)
                SendEntityMod(m as IEntityModifier);
            else if (m is IIndependentModifier)
                SendIndependentMod(m as IIndependentModifier);
            else
                throw new Exception("Unknown mod type: " + m.GetType().Name);
        }

        public void SendEntityMod(IEntityModifier m) {
            object[] args = new object[] {
                Serialisation.ToBinary(m.e), // Assume that the entity is not serialised with the modifier
                Serialisation.ToBinary(m)
            };
            View.RPC("DoEntityDependentMod", PhotonTargets.All, args);
        }

        public void SendIndependentMod(IIndependentModifier m) {
            View.RPC("DoIndependentMod", PhotonTargets.All, Serialisation.ToBinary(m));
        }

        [PunRPC]
        private void DoEntityDependentMod(object[] args) {
            IEntity e;
            IEntityModifier m;
            Serialisation.ToObjects(args, out e, out m);

            PingSimulator.Delay(() => {
                m.e = e;
                EM.RefreshEntityOnModifierUsingUID(m);

                EM.ApplyMod(m, DistributeBetweenPlayersAsyncFilter);
            });
        }

        [PunRPC]
        private void DoIndependentMod(byte[] binary) {
            IIndependentModifier m = Serialisation.To<IIndependentModifier>(binary);
            PingSimulator.Delay(() => {
                EM.ApplyMod(m, DistributeBetweenPlayersAsyncFilter);
            });
        }
        #endregion

        #region Send mod to all players and get confirmation whether it succeeded or failed
        public void SendModAndConfirm(IModifier m, Action finished) {
            if (m is IEntityModifier)
                SendEntityModAndConfirm(m as IEntityModifier, finished);
            else if (m is IIndependentModifier)
                SendIndependentModAndConfirm(m as IIndependentModifier, finished);
            else
                throw new Exception("Unknown mod type: " + m.GetType().Name);
        }

        public void SendEntityModAndConfirm(IEntityModifier m, Action finished) {
            int requestID = NetworkInteractionTracker.RecordMassPingRequestStarted();

            PhotonHelper.RequestFromPlayers(
                DependentRequest,
                r => ReturnModSentByRequestID.Add(requestID, r),
                _ => {
                    Debug.Log("Mod sent to all players, should be done locally too");
                    ReturnModSentByRequestID.Remove(requestID);
                    if (finished != null)
                        finished.Invoke();
                },
                PhotonNetwork.player.ID, m, requestID
            );

        }

        public void SendIndependentModAndConfirm(IIndependentModifier m, Action finished) {
            int requestID = NetworkInteractionTracker.RecordMassPingRequestStarted();
            Action<int, bool> ret = null;
            ReturnModSentByRequestID[requestID] = ret;

            long targetStep = IntervalTimer.NextPossibleStepToTarget(NetworkInteractionTracker.MaxPing);

            PhotonHelper.RequestFromPlayers(
                IndependentRequest,
                r => ReturnModSentByRequestID.Add(requestID, r),
                _ => {
                    ReturnModSentByRequestID.Remove(requestID);
                    if (finished != null)
                        finished.Invoke();
                },
                PhotonNetwork.player.ID, m, requestID, targetStep
            );
        }

        [PunRPC]
        private void DoEntityDependentModWithConfirmation(object[] args) {
            int sourcePlayerID;
            IEntityModifier m;
            int requestID;
            long targetStep;
            Serialisation.ToObjects(args, out sourcePlayerID, out m, out requestID, out targetStep);

            PingSimulator.Delay(() => {

                // If the modifier holds an entity value, retrieve it from its local registry using its UID
                EM.RefreshEntityOnModifierUsingUID(m);

                EM.ApplyMod(m, DistributeBetweenPlayersAsyncFilter);
                View.RPC("ConfirmModExecuted", PhotonNetwork.playerList[sourcePlayerID - 1], PhotonNetwork.player.ID, requestID);

            });
        }

        [PunRPC]
        private void DoIndependentModWithConfirmation(object[] args) {
            int sourcePlayerID;
            IIndependentModifier m;
            int requestID;
            long targetStep;
            Serialisation.ToObjects(args, out sourcePlayerID, out m, out requestID, out targetStep);

            PingSimulator.Delay(() => {

                EM.ApplyMod(m, DistributeBetweenPlayersAsyncFilter);
                View.RPC("ConfirmModExecuted", PhotonNetwork.playerList[sourcePlayerID - 1], PhotonNetwork.player.ID, requestID);

            });
        }

        [PunRPC]
        private void ConfirmModExecuted(int sourcePlayerID, int requestID) {
            PingSimulator.Delay(() => {
                ReturnModSentByRequestID[requestID](sourcePlayerID - 1, true);
                NetworkInteractionTracker.RecordPingReceived(sourcePlayerID, requestID);
                //NetworkInteractionTracker.WaitUntilCleared()
                //if (!NetworkInteractionTracker.WaitingOnPingRequests) {
                //    onAllModsCleared.Invoke();
                //    onAllModsCleared = delegate { Debug.Log("Mods cleared"); };
                //}
            });
        }
        #endregion

    }

}
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.signal.api;
using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Assertions;
using static ListenDispatchExtensions;

public static class UniRxIntegration {

    #region Events
    public static UniRx.IObservable<object> ToObservable(this IEventDispatcher dispatcher, object[] requests, object[] returns) {
        UniRx.IObservable<object> observable = ToObservable(dispatcher, returns);

        if (requests != null && requests.Length > 0) {
            for (int i = 0; i < requests.Length; i++) {
                dispatcher.Dispatch(requests[i]);
            }
        }

        return observable;
    }

    public static UniRx.IObservable<object> ToObservable(this IEventDispatcher dispatcher, object[] returns) {
        if (returns == null || returns.Length == 0)
            throw new ArgumentNullException("returns not valid");

        Subject<object> notYetReturned = new Subject<object>();
        List<object> returnsLeft = new List<object>(returns);

        for (int i = 0; i < returns.Length; i++) {
            object ret = returns[i];

            EmptyCallback handler = null;
            handler = () => {
                if (!returnsLeft.Contains(ret))
                    throw new Exception("Something went wrong...");

                dispatcher.RemoveListener(ret, handler);
                returnsLeft.Remove(ret);
                notYetReturned.OnNext(ret);

                if (returnsLeft.Count == 0)
                    notYetReturned.OnCompleted();

            };

            dispatcher.AddListener(ret, handler);

        }

        return notYetReturned;
    }
    #endregion

    #region Signals
    public static UniRx.IObservable<object> ToObservable(this SignalRequester[] requests, BaseSignal[] returns) {
        if (returns == null || returns.Length == 0)
            throw new ArgumentNullException("returns not valid");

        Subject<object> notYetReturned = new Subject<object>();
        List<BaseSignal> returnsLeft = new List<BaseSignal>(returns);

        for (int i = 0; i < returns.Length; i++) {
            BaseSignal ret = returns[i];

            Action<IBaseSignal, object[]> handler = null;
            handler = (s, xs) => {
                if (!returnsLeft.Contains(ret))
                    throw new Exception("Something went wrong...");

                Assert.AreEqual(s, ret);

                ret.RemoveListener(handler);
                returnsLeft.Remove(ret);
                notYetReturned.OnNext(ret);

                if (returnsLeft.Count == 0)
                    notYetReturned.OnCompleted();

            };

            ret.AddListener(handler);

        }

        return notYetReturned;
    }

    public static UniRx.IObservable<object> ToObservable(this SignalRequester[] requests, BaseSignalListener[] returns) {
        if (returns == null || returns.Length == 0)
            throw new ArgumentNullException("returns not valid");

        Subject<object> notYetReturned = new Subject<object>();
        List<BaseSignalListener> returnsLeft = new List<BaseSignalListener>(returns);

        for (int i = 0; i < returns.Length; i++) {
            BaseSignalListener ret = returns[i];

            Action<IBaseSignal, object[]> handler = null;
            handler = (s, xs) => {
                if (!returnsLeft.Contains(ret))
                    throw new Exception("Something went wrong...");

                Assert.AreEqual(s, ret.listeningOn);

                ret.listeningOn.RemoveListener(handler);
                returnsLeft.Remove(ret);
                notYetReturned.OnNext(ret);

                ret.Callback(xs);

                if (returnsLeft.Count == 0)
                    notYetReturned.OnCompleted();

            };

            ret.listeningOn.AddListener(handler);

        }

        return notYetReturned;
    }
    #endregion

}

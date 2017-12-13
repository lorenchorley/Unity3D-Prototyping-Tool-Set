using System;
using UnityEngine;
using UnityEngine.Assertions;
using strange.extensions.signal.api;
using strange.extensions.signal.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

public static class ListenDispatchExtensions {

    #region Events

    #region ListenOnce with single listener
    public static void ListenOnceForOne(this IEventDispatcher dispatcher, object returnEvent, EmptyCallback callback) {
        EmptyCallback intermediary = null;
        intermediary =
            () => {
                dispatcher.RemoveListener(returnEvent, intermediary);
                callback.Invoke();
            };
        dispatcher.AddListener(returnEvent, intermediary);
    }

    public static void ListenOnceForOne(this IEventDispatcher dispatcher, object returnEvent, EventCallback callback) {
        EventCallback intermediary = null;
        intermediary =
            evt => {
                dispatcher.RemoveListener(returnEvent, intermediary);
                callback.Invoke(evt);
            };
        dispatcher.AddListener(returnEvent, intermediary);
    }

    public static void ListenOnceForOne<E>(this IEventDispatcher dispatcher, object returnEvent, Action<E> callback) {
        EventCallback intermediary = null;
        intermediary =
            evt => {
                dispatcher.RemoveListener(returnEvent, intermediary);
                callback.Invoke((E) evt.data);
            };
        dispatcher.AddListener(returnEvent, intermediary);
    }

    public static void ListenOnceAndRedirect(this IEventDispatcher dispatcher, object returnEvent, object redirectToEvent) {
        EventCallback intermediary = null;
        intermediary =
            evt => {
                dispatcher.RemoveListener(returnEvent, intermediary);
                dispatcher.Dispatch(redirectToEvent, evt.data);
            };
        dispatcher.AddListener(returnEvent, intermediary);
    }
    #endregion

    #region ListenOnce with multiple listeners
    public static void ListenOnceForOne(this IEventDispatcher dispatcher, params BaseEventListener[] listeners) {
        bool returned = false;
        for (int i = 0; i < listeners.Length; i++) {
            BaseEventListener listener = listeners[i];

            if (listener is DispatcherBaseEventListener)
                (listener as DispatcherBaseEventListener).SetDispatcher(dispatcher);

            listener.proxy =
                evt => {
                    if (returned)
                        return;
                    returned = true;

                    for (int j = 0; j < listeners.Length; j++) {
                        dispatcher.RemoveListener(listeners[j].returnEvent, listener.proxy);
                    }

                    listener.Callback(evt);
                };

            dispatcher.AddListener(listeners[i].returnEvent, listener.proxy);

        }
    }
    #endregion

    #region Dispatch with request and single listener
    public static void DispatchAndListen(this IEventDispatcher dispatcher, object requestEvent, object returnEvent, EmptyCallback callback) {
        dispatcher.ListenOnceForOne(returnEvent, callback);
        dispatcher.Dispatch(requestEvent);
    }

    public static void DispatchAndListen(this IEventDispatcher dispatcher, object requestEvent, object returnEvent, EventCallback callback) {
        dispatcher.ListenOnceForOne(returnEvent, callback);
        dispatcher.Dispatch(requestEvent);
    }

    public static void DispatchAndListen<E>(this IEventDispatcher dispatcher, object requestEvent, object returnEvent, Action<E> callback) {
        dispatcher.ListenOnceForOne(returnEvent, callback);
        dispatcher.Dispatch(requestEvent);
    }

    public static void DispatchAndRedirect<E>(this IEventDispatcher dispatcher, object requestEvent, object returnEvent, object redirectToEvent) {
        dispatcher.ListenOnceAndRedirect(returnEvent, redirectToEvent);
        dispatcher.Dispatch(requestEvent);
    }
    #endregion

    #region Dispatch with requester and single listener
    public static void DispatchAndListen(this IEventDispatcher dispatcher, ER requester, object returnEvent, EmptyCallback callback) {
        dispatcher.ListenOnceForOne(returnEvent, callback);
        requester.Dispatch(dispatcher);
    }

    public static void DispatchAndListen(this IEventDispatcher dispatcher, ER requester, object returnEvent, EventCallback callback) {
        dispatcher.ListenOnceForOne(returnEvent, callback);
        requester.Dispatch(dispatcher);
    }

    public static void DispatchAndListen<E>(this IEventDispatcher dispatcher, ER requester, object returnEvent, Action<E> callback) {
        dispatcher.ListenOnceForOne(returnEvent, callback);
        requester.Dispatch(dispatcher);
    }

    public static void DispatchAndRedirect(this IEventDispatcher dispatcher, ER requester, object returnEvent, object redirectToEvent) {
        dispatcher.ListenOnceAndRedirect(returnEvent, redirectToEvent);
        requester.Dispatch(dispatcher);
    }
    #endregion

    #region Dispatch with delegate request and single listener
    public static void DispatchAndListen(this IEventDispatcher dispatcher, Action requestAction, object returnEvent, EmptyCallback callback) {
        dispatcher.ListenOnceForOne(returnEvent, callback);
        requestAction.Invoke();
    }

    public static void DispatchAndListen(this IEventDispatcher dispatcher, Action requestAction, object returnEvent, EventCallback callback) {
        dispatcher.ListenOnceForOne(returnEvent, callback);
        requestAction.Invoke();
    }

    public static void DispatchAndListen<E>(this IEventDispatcher dispatcher, Action requestAction, object returnEvent, Action<E> callback) {
        dispatcher.ListenOnceForOne(returnEvent, callback);
        requestAction.Invoke();
    }

    public static void DispatchAndRedirect(this IEventDispatcher dispatcher, Action requestAction, object returnEvent, object redirectToEvent) {
        dispatcher.ListenOnceAndRedirect(returnEvent, redirectToEvent);
        requestAction.Invoke();
    }
    #endregion

    #region Dispatch with request and multiple listeners
    public static void DispatchAndListen(this IEventDispatcher dispatcher, object requestEvent, params BaseEventListener[] listeners) {
        dispatcher.ListenOnceForOne(listeners);
        dispatcher.Dispatch(requestEvent);
    }
    #endregion

    #region Dispatch with requester and multiple listeners
    public static void DispatchAndListen(this IEventDispatcher dispatcher, ER requester, params BaseEventListener[] listeners) {
        dispatcher.ListenOnceForOne(listeners);
        requester.Dispatch(dispatcher);
    }
    #endregion

    #region Dispatch with delegate request and multiple listeners
    public static void DispatchAndListen(this IEventDispatcher dispatcher, Action requestAction, params BaseEventListener[] listeners) {
        dispatcher.ListenOnceForOne(listeners);
        requestAction.Invoke();
    }
    #endregion

    #endregion

    #region Signals

    #region ListenOnce with multiple listeners
    public static void ListenOnceForOne(this BaseSignalListener[] listeners) {
        bool returned = false;
        for (int i = 0; i < listeners.Length; i++) {
            BaseSignalListener listener = listeners[i];

            listener.proxy =
                (s, xs) => {
                    if (returned)
                        return;
                    returned = true;

                    for (int j = 0; j < listeners.Length; j++) {
                        // TODO May need to check that i != j
                        listeners[j].listeningOn.OnceBaseListener -= listener.proxy;
                    }

                    listener.Callback(xs);
                };

            listeners[i].listeningOn.OnceBaseListener += listener.proxy;

        }
    }
    #endregion

    #region Dispatch multiple signal requests
    public static void Dispatch(this SignalRequester[] requesters) {
        for (int i = 0; i < requesters.Length; i++) {
            requesters[i].Dispatch();
        }
    }
    #endregion

    #region DispatchAndListen with multiple listeners
    public static void ListenAndStartWith(this BaseSignalListener[] listeners, Action startProcess) {
        listeners.ListenOnceForOne();
        startProcess.Invoke();
    }

    public static void DispatchAndListen(this Signal triggeringSignal, params BaseSignalListener[] listeners) {
        listeners.ListenOnceForOne();
        triggeringSignal.Dispatch();
    }

    public static void DispatchAndListen(this Signal triggeringSignal, BaseSignalRequester requester, params BaseSignalListener[] listeners) {
        listeners.ListenOnceForOne();
        requester.DispatchTo(triggeringSignal);
    }

    public static void DispatchAndListen<A>(this Signal<A> triggeringSignal, A first, params BaseSignalListener[] listeners) {
        listeners.ListenOnceForOne();
        triggeringSignal.Dispatch(first);
    }

    public static void DispatchAndListen<A, B>(this Signal<A, B> triggeringSignal, A first, B second, params BaseSignalListener[] listeners) {
        listeners.ListenOnceForOne();
        triggeringSignal.Dispatch(first, second);
    }

    public static void DispatchAndListen<A, B, C>(this Signal<A, B, C> triggeringSignal, A first, B second, C third, params BaseSignalListener[] listeners) {
        listeners.ListenOnceForOne();
        triggeringSignal.Dispatch(first, second, third);
    }

    public static void DispatchAndListen<A, B, C, D>(this Signal<A, B, C, D> triggeringSignal, A first, B second, C third, D fourth, params BaseSignalListener[] listeners) {
        listeners.ListenOnceForOne();
        triggeringSignal.Dispatch(first, second, third, fourth);
    }
    #endregion

    #region ListenAndStartWith
    public static void ListenAndStartWith(this Signal listeningSignal, Action startProcess, Action callback) {
        listeningSignal.AddOnce(callback);
        startProcess.Invoke();
    }

    public static void ListenAndStartWith<A>(this Signal<A> listeningSignal, Action startProcess, Action<A> callback) {
        listeningSignal.AddOnce(callback);
        startProcess.Invoke();
    }

    public static void ListenAndStartWith<A, B>(this Signal<A, B> listeningSignal, Action startProcess, Action<A, B> callback) {
        listeningSignal.AddOnce(callback);
        startProcess.Invoke();
    }

    public static void ListenAndStartWith<A, B, C>(this Signal<A, B, C> listeningSignal, Action startProcess, Action<A, B, C> callback) {
        listeningSignal.AddOnce(callback);
        startProcess.Invoke();
    }

    public static void ListenAndStartWith<A, B, C, D>(this Signal<A, B, C, D> listeningSignal, Action startProcess, Action<A, B, C, D> callback) {
        listeningSignal.AddOnce(callback);
        startProcess.Invoke();
    }
    #endregion

    #region DispatchAndListen
    public static void DispatchAndListen(this Signal dispatchingSignal, Signal listenOn, Action callback) {
        listenOn.AddOnce(callback);
        dispatchingSignal.Dispatch();
    }

    public static void DispatchAndListen<A>(this Signal dispatchingSignal, Signal<A> listenOn, Action<A> callback) {
        listenOn.AddOnce(callback);
        dispatchingSignal.Dispatch();
    }

    public static void DispatchAndListen<A, B>(this Signal dispatchingSignal, Signal<A, B> listenOn, Action<A, B> callback) {
        listenOn.AddOnce(callback);
        dispatchingSignal.Dispatch();
    }

    public static void DispatchAndListen<A, B, C>(this Signal dispatchingSignal, Signal<A, B, C> listenOn, Action<A, B, C> callback) {
        listenOn.AddOnce(callback);
        dispatchingSignal.Dispatch();
    }

    public static void DispatchAndListen<A, B, C, D>(this Signal dispatchingSignal, Signal<A, B, C, D> listenOn, Action<A, B, C, D> callback) {
        listenOn.AddOnce(callback);
        dispatchingSignal.Dispatch();
    }
    #endregion

    #region Multiple signal requests with multiple listeners
    public static void DispatchAndListen(this SignalRequester[] requesters, params BaseSignalListener[] listeners) {
        listeners.ListenOnceForOne();
        requesters.Dispatch();
    }
    #endregion

    #region UpdateListener
    public static void UpdateListener(this Signal signal, bool toAdd, Action callback) {
        if (toAdd)
            signal.AddListener(callback);
        else
            signal.RemoveListener(callback);
    }

    public static void UpdateListener<A>(this Signal<A> signal, bool toAdd, Action<A> callback) {
        if (toAdd)
            signal.AddListener(callback);
        else
            signal.RemoveListener(callback);
    }

    public static void UpdateListener<A, B>(this Signal<A, B> signal, bool toAdd, Action<A, B> callback) {
        if (toAdd)
            signal.AddListener(callback);
        else
            signal.RemoveListener(callback);
    }

    public static void UpdateListener<A, B, C>(this Signal<A, B, C> signal, bool toAdd, Action<A, B, C> callback) {
        if (toAdd)
            signal.AddListener(callback);
        else
            signal.RemoveListener(callback);
    }

    public static void UpdateListener<A, B, C, D>(this Signal<A, B, C, D> signal, bool toAdd, Action<A, B, C, D> callback) {
        if (toAdd)
            signal.AddListener(callback);
        else
            signal.RemoveListener(callback);
    }
    #endregion

    #endregion

    #region Listener and Requester classes

    #region Signals

    #region Requester parameter classes
    public abstract class SignalRequester {

        public abstract void Dispatch();

        public BaseSignal Signal;

        public SignalRequester(BaseSignal Signal) {
            this.Signal = Signal;
        }

    }

    public class CSR : SignalRequester {

        public CSR(Signal Signal) : base(Signal) { }

        public override void Dispatch() {
            if (!(Signal is Signal))
                throw new Exception("TODO");

            (Signal as Signal).Dispatch();
        }

    }

    public class CSR<A> : SignalRequester {

        public A First;

        public CSR(Signal<A> Signal, A First) : base(Signal) {
            this.First = First;
        }

        public override void Dispatch() {
            if (!(Signal is Signal<A>))
                throw new Exception("TODO");

            (Signal as Signal<A>).Dispatch(First);
        }

    }

    public class CSR<A, B> : SignalRequester {

        public A First;
        public B Second;

        public CSR(Signal<A, B> Signal, A First, B Second) : base(Signal) {
            this.First = First;
            this.Second = Second;
        }

        public override void Dispatch() {
            if (!(Signal is Signal<A, B>))
                throw new Exception("TODO");

            (Signal as Signal<A, B>).Dispatch(First, Second);
        }

    }

    public class CSR<A, B, C> : SignalRequester {

        public A First;
        public B Second;
        public C Third;

        public CSR(Signal<A, B, C> Signal, A First, B Second, C Third) : base(Signal) {
            this.First = First;
            this.Second = Second;
            this.Third = Third;
        }

        public override void Dispatch() {
            if (!(Signal is Signal<A, B, C>))
                throw new Exception("TODO");

            (Signal as Signal<A, B, C>).Dispatch(First, Second, Third);
        }

    }

    public class CSR<A, B, C, D> : SignalRequester {

        public A First;
        public B Second;
        public C Third;
        public D Fourth;

        public CSR(Signal<A, B, C, D> Signal, A First, B Second, C Third, D Fourth) : base(Signal) {
            this.First = First;
            this.Second = Second;
            this.Third = Third;
            this.Fourth = Fourth;
        }

        public override void Dispatch() {
            if (!(Signal is Signal<A, B, C, D>))
                throw new Exception("TODO");

            (Signal as Signal<A, B, C, D>).Dispatch(First, Second, Third, Fourth);
        }

    }


    public abstract class BaseSignalRequester {
        public abstract void DispatchTo(BaseSignal triggeringSignal);
    }

    public class SR<A> : BaseSignalRequester {

        public A First;

        public SR(A First) {
            this.First = First;
        }

        public override void DispatchTo(BaseSignal triggeringSignal) {
            if (!(triggeringSignal is Signal<A>))
                throw new Exception("TODO");

            (triggeringSignal as Signal<A>).Dispatch(First);
        }

    }

    public class SR<A, B> : BaseSignalRequester {

        public A First;
        public B Second;

        public SR(A First, B Second) {
            this.First = First;
            this.Second = Second;
        }

        public override void DispatchTo(BaseSignal triggeringSignal) {
            if (!(triggeringSignal is Signal<A, B>))
                throw new Exception("TODO");

            (triggeringSignal as Signal<A, B>).Dispatch(First, Second);
        }

    }

    public class SR<A, B, C> : BaseSignalRequester {

        public A First;
        public B Second;
        public C Third;

        public SR(A First, B Second, C Third) {
            this.First = First;
            this.Second = Second;
            this.Third = Third;
        }

        public override void DispatchTo(BaseSignal triggeringSignal) {
            if (!(triggeringSignal is Signal<A, B, C>))
                throw new Exception("TODO");

            (triggeringSignal as Signal<A, B, C>).Dispatch(First, Second, Third);
        }

    }

    public class SR<A, B, C, D> : BaseSignalRequester {

        public A First;
        public B Second;
        public C Third;
        public D Fourth;

        public SR(A First, B Second, C Third, D Fourth) {
            this.First = First;
            this.Second = Second;
            this.Third = Third;
            this.Fourth = Fourth;
        }

        public override void DispatchTo(BaseSignal triggeringSignal) {
            if (!(triggeringSignal is Signal<A, B, C, D>))
                throw new Exception("TODO");

            (triggeringSignal as Signal<A, B, C, D>).Dispatch(First, Second, Third, Fourth);
        }

    }
    #endregion

    #region Signal listener parameter classes
    public abstract class BaseSignalListener {

        public abstract void Callback(object[] xs);

        public BaseSignal listeningOn;
        public Action<IBaseSignal, object[]> proxy = null;

        public BaseSignalListener(BaseSignal listeningOn) {
            this.listeningOn = listeningOn;
        }

    }

    public abstract class DispatcherBaseSignalListener : BaseSignalListener {

        //protected IEventDispatcher dispatcher;

        public DispatcherBaseSignalListener(BaseSignal listeningOn) : base(listeningOn) { }

        //public void SetDispatcher(IEventDispatcher dispatcher) {
        //    this.dispatcher = dispatcher;
        //}

    }

    public class SL : BaseSignalListener {

        private EmptyCallback callback;

        public SL(BaseSignal listeningOn, EmptyCallback callback) : base(listeningOn) {
            this.callback = callback;
        }

        public override void Callback(object[] xs) {
            callback.Invoke();
        }

    }

    //public class FailureSL : BaseSignalListener {

    //    private string ErrorMessage;

    //    public FailureSL(BaseSignal listeningOn, string ErrorMessage) : base(listeningOn) {
    //        this.ErrorMessage = ErrorMessage;
    //    }

    //    public override void Callback(object[] xs) {
    //        if (evt.data == null)
    //            Debug.LogError(ErrorMessage);
    //        //else if (evt.data.GetType().IsArray)
    //        //    Debug.LogErrorFormat(ErrorMessage,   TODO   );
    //        else {
    //            Debug.LogErrorFormat(ErrorMessage, evt.data.ToString());
    //        }
    //    }

    //}

    //public class RedirectionSL : DispatcherBaseSignalListener {

    //    private object redirectToEvent;

    //    public RedirectionSL(BaseSignal listeningOn, object redirectToEvent) : base(listeningOn) {
    //        this.redirectToEvent = redirectToEvent;
    //    }

    //    public override void Callback(object[] xs) {
    //        dispatcher.Dispatch(redirectToEvent, evt.data);
    //    }

    //}

    //public class GeneralSL : BaseSignalListener {

    //    private EventCallback callback;

    //    public GeneralSL(BaseSignal listeningOn, EventCallback callback) : base(listeningOn) {
    //        this.callback = callback;
    //    }

    //    public override void Callback(object[] xs) {
    //        Assert.IsTrue(xs.Length == 1);
    //        callback.Invoke(xs[0]);
    //    }

    //}

    public class SL<A> : BaseSignalListener {

        private Action<A> callback;

        public SL(Signal<A> listeningOn, Action<A> callback) : base(listeningOn) {
            this.callback = callback;
        }

        public override void Callback(object[] xs) {
            Assert.IsTrue(xs.Length == 1);
            callback.Invoke((A) xs[0]);
        }

    }

    public class SL<A, B> : BaseSignalListener {

        private Action<A, B> callback;

        public SL(Signal<A, B> listeningOn, Action<A, B> callback) : base(listeningOn) {
            this.callback = callback;
        }

        public override void Callback(object[] xs) {
            Assert.IsTrue(xs.Length == 2);
            callback.Invoke(
                (A) xs[0],
                (B) xs[1]
            );
        }

    }

    public class SL<A, B, C> : BaseSignalListener {

        private Action<A, B, C> callback;

        public SL(Signal<A, B, C> listeningOn, Action<A, B, C> callback) : base(listeningOn) {
            this.callback = callback;
        }

        public override void Callback(object[] xs) {
            Assert.IsTrue(xs.Length == 3);
            callback.Invoke(
                (A) xs[0],
                (B) xs[1],
                (C) xs[2]
            );
        }

    }

    public class SL<A, B, C, D> : BaseSignalListener {

        private Action<A, B, C, D> callback;

        public SL(Signal<A, B, C, D> listeningOn, Action<A, B, C, D> callback) : base(listeningOn) {
            this.callback = callback;
        }

        public override void Callback(object[] xs) {
            Assert.IsTrue(xs.Length == 4);
            callback.Invoke(
                (A) xs[0],
                (B) xs[1],
                (C) xs[2],
                (D) xs[3]
            );
        }

    }
    #endregion

    #endregion

    #region Events

    #region Event requester parameter classes
    public class ER {

        public object RequestEvent;
        public object Argument;

        public ER(object RequestEvent, object Argument) {
            this.RequestEvent = RequestEvent;
            this.Argument = Argument;
        }

        public void Dispatch(IEventDispatcher dispatcher) {
            if (Argument == null)
                dispatcher.Dispatch(RequestEvent);
            else
                dispatcher.Dispatch(RequestEvent, Argument);
        }

    }
    #endregion

    #region Event listener parameter classes
    public abstract class BaseEventListener {

        public abstract void Callback(IEvent evt);

        public object returnEvent;
        public EventCallback proxy = null;

        public BaseEventListener(object returnEvent) {
            this.returnEvent = returnEvent;
        }

    }

    public abstract class DispatcherBaseEventListener : BaseEventListener {

        protected IEventDispatcher dispatcher;

        public DispatcherBaseEventListener(object returnEvent) : base(returnEvent) { }

        public void SetDispatcher(IEventDispatcher dispatcher) {
            this.dispatcher = dispatcher;
        }

    }

    public class EL : BaseEventListener {

        private EmptyCallback callback;

        public EL(object returnEvent, EmptyCallback callback) : base(returnEvent) {
            this.callback = callback;
        }

        public override void Callback(IEvent evt) {
            callback.Invoke();
        }

    }

    public class FailureEL : BaseEventListener {

        private string ErrorMessage;

        public FailureEL(object returnEvent, string ErrorMessage) : base(returnEvent) {
            this.ErrorMessage = ErrorMessage;
        }

        public override void Callback(IEvent evt) {
            if (evt.data == null)
                Debug.LogError(ErrorMessage);
            //else if (evt.data.GetType().IsArray)
            //    Debug.LogErrorFormat(ErrorMessage,   TODO   );
            else {
                Debug.LogErrorFormat(ErrorMessage, evt.data.ToString());
            }
        }

    }

    public class RedirectionEL : DispatcherBaseEventListener {

        private object redirectToEvent;

        public RedirectionEL(object returnEvent, object redirectToEvent) : base(returnEvent) {
            this.redirectToEvent = redirectToEvent;
        }

        public override void Callback(IEvent evt) {
            dispatcher.Dispatch(redirectToEvent, evt.data);
        }

    }

    public class GeneralEL : BaseEventListener {

        private EventCallback callback;

        public GeneralEL(object returnEvent, EventCallback callback) : base(returnEvent) {
            this.callback = callback;
        }

        public override void Callback(IEvent evt) {
            callback.Invoke(evt);
        }

    }

    public class EL<A> : BaseEventListener {

        private Action<A> callback;

        public EL(object returnEvent, Action<A> callback) : base(returnEvent) {
            this.callback = callback;
        }

        public override void Callback(IEvent evt) {
            callback.Invoke((A) evt.data);
        }

    }

    public class EL<A, B> : BaseEventListener {

        private Action<A, B> callback;

        public EL(object returnEvent, Action<A, B> callback) : base(returnEvent) {
            this.callback = callback;
        }

        public override void Callback(IEvent evt) {
            object[] xs = (object[]) evt.data;
            Assert.IsTrue(xs.Length >= 2);
            callback.Invoke(
                (A) xs[0],
                (B) xs[1]
            );
        }

    }

    public class EL<A, B, C> : BaseEventListener {

        private Action<A, B, C> callback;

        public EL(object returnEvent, Action<A, B, C> callback) : base(returnEvent) {
            this.callback = callback;
        }

        public override void Callback(IEvent evt) {
            object[] xs = (object[]) evt.data;
            Assert.IsTrue(xs.Length >= 3);
            callback.Invoke(
                (A) xs[0],
                (B) xs[1],
                (C) xs[2]
            );
        }

    }

    public class EL<A, B, C, D> : BaseEventListener {

        private Action<A, B, C, D> callback;

        public EL(object returnEvent, Action<A, B, C, D> callback) : base(returnEvent) {
            this.callback = callback;
        }

        public override void Callback(IEvent evt) {
            object[] xs = (object[]) evt.data;
            Assert.IsTrue(xs.Length >= 4);
            callback.Invoke(
                (A) xs[0],
                (B) xs[1],
                (C) xs[2],
                (D) xs[3]
            );
        }

    }
    #endregion

    #endregion

    #endregion

}

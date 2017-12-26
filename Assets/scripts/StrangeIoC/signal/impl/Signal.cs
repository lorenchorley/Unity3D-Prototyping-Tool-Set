using strange.extensions.context.impl;
using strange.extensions.signal.api;
using System;
using System.Collections.Generic;

namespace strange.extensions.signal.impl {
    public class Signal : BaseSignal {

        public event Action Listener = delegate { };
        public event Action OnceListener = delegate { };
        public void AddListener(Action callback) { Listener += callback; }
        public void AddOnce(Action callback) { OnceListener += callback; }
        public void RemoveListener(Action callback) { Listener -= callback; }
        public override List<Type> GetTypes() { return new List<Type>(); }

        public void Dispatch() {
#if UNITY_EDITOR
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount);
#endif
            Listener();
            OnceListener();
            OnceListener = delegate { };
            base.Dispatch(null);
        }

        public void UpdateListener(bool toAdd, Action callback) {
            if (toAdd)
                AddListener(callback);
            else
                RemoveListener(callback);
        }

        public int SubscriptionCount {
            get {
                return Listener.GetInvocationList().Length + OnceListener.GetInvocationList().Length - 2;
            }
        }

    }

    public class Signal<T> : BaseSignal {

        public event Action<T> Listener = delegate { };
        public event Action<T> OnceListener = delegate { };
        public void AddListener(Action<T> callback) { Listener += callback; }
        public void AddOnce(Action<T> callback) { OnceListener += callback; }
        public void RemoveListener(Action<T> callback) { Listener -= callback; }

        public override List<Type> GetTypes() {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            return retv;
        }

        public void Dispatch(T type1) {
#if UNITY_EDITOR
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1);
#endif
            Listener(type1);
            OnceListener(type1);
            OnceListener = delegate { };
            object[] outv = { type1 };
            base.Dispatch(outv);
        }

        public void UpdateListener(bool toAdd, Action<T> callback) {
            if (toAdd)
                AddListener(callback);
            else
                RemoveListener(callback);
        }

        public int SubscriptionCount {
            get {
                return Listener.GetInvocationList().Length + OnceListener.GetInvocationList().Length - 2;
            }
        }

    }

    public class Signal<T, U> : BaseSignal {

        public event Action<T, U> Listener = delegate { };
        public event Action<T, U> OnceListener = delegate { };
        public void AddListener(Action<T, U> callback) { Listener += callback; }
        public void AddOnce(Action<T, U> callback) { OnceListener += callback; }
        public void RemoveListener(Action<T, U> callback) { Listener -= callback; }

        public override List<Type> GetTypes() {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            retv.Add(typeof(U));
            return retv;
        }

        public void Dispatch(T type1, U type2) {
#if UNITY_EDITOR
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1, type2);
#endif
            Listener(type1, type2);
            OnceListener(type1, type2);
            OnceListener = delegate { };
            object[] outv = { type1, type2 };
            base.Dispatch(outv);
        }

        public void UpdateListener(bool toAdd, Action<T, U> callback) {
            if (toAdd)
                AddListener(callback);
            else
                RemoveListener(callback);
        }

        public int SubscriptionCount {
            get {
                return Listener.GetInvocationList().Length + OnceListener.GetInvocationList().Length - 2;
            }
        }

    }

    public class Signal<T, U, V> : BaseSignal {

        public event Action<T, U, V> Listener = delegate { };
        public event Action<T, U, V> OnceListener = delegate { };
        public void AddListener(Action<T, U, V> callback) { Listener += callback; }
        public void AddOnce(Action<T, U, V> callback) { OnceListener += callback; }
        public void RemoveListener(Action<T, U, V> callback) { Listener -= callback; }

        public override List<Type> GetTypes() {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            retv.Add(typeof(U));
            retv.Add(typeof(V));
            return retv;
        }

        public void Dispatch(T type1, U type2, V type3) {
#if UNITY_EDITOR
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1, type2, type3);
#endif
            Listener(type1, type2, type3);
            OnceListener(type1, type2, type3);
            OnceListener = delegate { };
            object[] outv = { type1, type2, type3 };
            base.Dispatch(outv);
        }

        public void UpdateListener(bool toAdd, Action<T, U, V> callback) {
            if (toAdd)
                AddListener(callback);
            else
                RemoveListener(callback);
        }

        public int SubscriptionCount {
            get {
                return Listener.GetInvocationList().Length + OnceListener.GetInvocationList().Length - 2;
            }
        }

    }

    public class Signal<T, U, V, W> : BaseSignal {

        public event Action<T, U, V, W> Listener = delegate { };
        public event Action<T, U, V, W> OnceListener = delegate { };
        public void AddListener(Action<T, U, V, W> callback) { Listener += callback; }
        public void AddOnce(Action<T, U, V, W> callback) { OnceListener += callback; }
        public void RemoveListener(Action<T, U, V, W> callback) { Listener -= callback; }

        public override List<Type> GetTypes() {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            retv.Add(typeof(U));
            retv.Add(typeof(V));
            retv.Add(typeof(W));
            return retv;
        }

        public void Dispatch(T type1, U type2, V type3, W type4) {
#if UNITY_EDITOR
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1, type2, type3, type4);
#endif
            Listener(type1, type2, type3, type4);
            OnceListener(type1, type2, type3, type4);
            OnceListener = delegate { };
            object[] outv = { type1, type2, type3, type4 };
            base.Dispatch(outv);
        }

        public void UpdateListener(bool toAdd, Action<T, U, V, W> callback) {
            if (toAdd)
                AddListener(callback);
            else
                RemoveListener(callback);
        }

        public int SubscriptionCount {
            get {
                return Listener.GetInvocationList().Length + OnceListener.GetInvocationList().Length - 2;
            }
        }

    }

    // Strict Signals, for speed, don't have once listeners and don't use the base signal's events
    // Cannot be bound to a command using a command binder!
    // Can be used as fast links between views and mediators
    // Can be injected for fast interaction between and within contexts
    public class StrictBaseSignal : BaseSignal {

        public override void Dispatch(object[] args) {
            throw new Exception("StrictBaseSignal does not allow use of the general dispatch method");
        }

        public override void AddListener(Action<IBaseSignal, object[]> callback) {
            throw new Exception("Strict signals do not support BaseSignal functions (" + GetType().Name + ")");
        }

        public override void AddOnce(Action<IBaseSignal, object[]> callback) {
            throw new Exception("Strict signals do not support BaseSignal functions (" + GetType().Name + ")");
        }

        public override void RemoveListener(Action<IBaseSignal, object[]> callback) {
            throw new Exception("Strict signals do not support BaseSignal functions (" + GetType().Name + ")");
        }

        public override void RemoveOnceListener(Action<IBaseSignal, object[]> callback) {
            throw new Exception("Strict signals do not support BaseSignal functions (" + GetType().Name + ")");
        }

    }

    public class StrictSignal : StrictBaseSignal {

        public event Action Listener = delegate { };
        public void AddListener(Action callback) { Listener += callback; }
        public void RemoveListener(Action callback) { Listener -= callback; }
        public override List<Type> GetTypes() { return new List<Type>(); }

        public void Dispatch() {
#if UNITY_EDITOR
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount);
#endif
            Listener();
        }

        public void UpdateListener(bool toAdd, Action callback) {
            if (toAdd)
                AddListener(callback);
            else
                RemoveListener(callback);
        }

        public int SubscriptionCount {
            get {
                return Listener.GetInvocationList().Length - 1;
            }
        }
        
    }

    public class StrictSignal<T> : StrictBaseSignal {

        public event Action<T> Listener = delegate { };
        public void AddListener(Action<T> callback) { Listener += callback; }
        public void RemoveListener(Action<T> callback) { Listener -= callback; }

        public override List<Type> GetTypes() {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            return retv;
        }

        public void Dispatch(T type1) {
#if UNITY_EDITOR
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1);
#endif
            Listener(type1);
        }

        public void UpdateListener(bool toAdd, Action<T> callback) {
            if (toAdd)
                AddListener(callback);
            else
                RemoveListener(callback);
        }

        public int SubscriptionCount {
            get {
                return Listener.GetInvocationList().Length - 1;
            }
        }
        
    }

    public class StrictSignal<T, U> : StrictBaseSignal {

        public event Action<T, U> Listener = delegate { };
        public void AddListener(Action<T, U> callback) { Listener += callback; }
        public void RemoveListener(Action<T, U> callback) { Listener -= callback; }

        public override List<Type> GetTypes() {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            retv.Add(typeof(U));
            return retv;
        }

        public void Dispatch(T type1, U type2) {
#if UNITY_EDITOR
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1, type2);
#endif
            Listener(type1, type2);
        }

        public void UpdateListener(bool toAdd, Action<T, U> callback) {
            if (toAdd)
                AddListener(callback);
            else
                RemoveListener(callback);
        }

        public int SubscriptionCount {
            get {
                return Listener.GetInvocationList().Length - 1;
            }
        }
        
    }

    public class StrictSignal<T, U, V> : StrictBaseSignal {

        public event Action<T, U, V> Listener = delegate { };
        public void AddListener(Action<T, U, V> callback) { Listener += callback; }
        public void RemoveListener(Action<T, U, V> callback) { Listener -= callback; }

        public override List<Type> GetTypes() {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            retv.Add(typeof(U));
            retv.Add(typeof(V));
            return retv;
        }

        public void Dispatch(T type1, U type2, V type3) {
#if UNITY_EDITOR
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1, type2, type3);
#endif
            Listener(type1, type2, type3);
        }

        public void UpdateListener(bool toAdd, Action<T, U, V> callback) {
            if (toAdd)
                AddListener(callback);
            else
                RemoveListener(callback);
        }

        public int SubscriptionCount {
            get {
                return Listener.GetInvocationList().Length - 1;
            }
        }
        
    }

    public class StrictSignal<T, U, V, W> : StrictBaseSignal {

        public event Action<T, U, V, W> Listener = delegate { };
        public void AddListener(Action<T, U, V, W> callback) { Listener += callback; }
        public void RemoveListener(Action<T, U, V, W> callback) { Listener -= callback; }

        public override List<Type> GetTypes() {
            List<Type> retv = new List<Type>();
            retv.Add(typeof(T));
            retv.Add(typeof(U));
            retv.Add(typeof(V));
            retv.Add(typeof(W));
            return retv;
        }

        public void Dispatch(T type1, U type2, V type3, W type4) {
#if UNITY_EDITOR
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1, type2, type3, type4);
#endif
            Listener(type1, type2, type3, type4);
        }

        public void UpdateListener(bool toAdd, Action<T, U, V, W> callback) {
            if (toAdd)
                AddListener(callback);
            else
                RemoveListener(callback);
        }

        public int SubscriptionCount {
            get {
                return Listener.GetInvocationList().Length - 1;
            }
        }
        
    }

}

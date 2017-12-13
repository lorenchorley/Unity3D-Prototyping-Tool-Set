using strange.extensions.context.impl;
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
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount);
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
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1);
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
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1, type2);
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
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1, type2, type3);
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
            ContextDebugging.OnSignalDispatch(this, SubscriptionCount, type1, type2, type3, type4);
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

}

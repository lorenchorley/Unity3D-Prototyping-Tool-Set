using System;
using strange.extensions.signal.api;
using System.Collections.Generic;

namespace strange.extensions.signal.impl {
    public class BaseSignal : IBaseSignal {

        private event Action<IBaseSignal, object[]> BaseListener = delegate { };
        private event Action<IBaseSignal, object[]> OnceBaseListener = delegate { };

        public virtual void Dispatch(object[] args) {
            BaseListener(this, args);
            OnceBaseListener(this, args);
            OnceBaseListener = delegate { };
        }

        public virtual List<Type> GetTypes() { return new List<Type>(); }

        public virtual void AddListener(Action<IBaseSignal, object[]> callback) {
            foreach (Delegate del in BaseListener.GetInvocationList()) {
                Action<IBaseSignal, object[]> action = (Action<IBaseSignal, object[]>) del;
                if (callback.Equals(action)) //If this callback exists already, ignore this addlistener
                    return;
            }

            BaseListener += callback;
        }

        public virtual void AddOnce(Action<IBaseSignal, object[]> callback) {
            foreach (Delegate del in OnceBaseListener.GetInvocationList()) {
                Action<IBaseSignal, object[]> action = (Action<IBaseSignal, object[]>) del;
                if (callback.Equals(action)) //If this callback exists already, ignore this addlistener
                    return;
            }
            OnceBaseListener += callback;
        }

        public virtual void RemoveListener(Action<IBaseSignal, object[]> callback) { BaseListener -= callback; }

        public virtual void RemoveOnceListener(Action<IBaseSignal, object[]> callback) { OnceBaseListener -= callback; }

        public int BaseSubscriptionCount {
            get {
                return BaseListener.GetInvocationList().Length + OnceBaseListener.GetInvocationList().Length - 2;
            }
        }

    }
}


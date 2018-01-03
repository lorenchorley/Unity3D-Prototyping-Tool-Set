/// Example mediator
/// =====================
/// Make your Mediator as thin as possible. Its function is to mediate
/// between view and app. Don't load it up with behavior that belongs in
/// the View (listening to/controlling interface), Commands (business logic),
/// Models (maintaining state) or Services (reaching out for data).
using strange.extensions.mediation.impl;

namespace menusystem {

    public abstract class MenuMediator<V> : EventMediator<V> where V : MenuView {

        protected abstract void Init();
        protected abstract void UpdateListeners(bool value);

        public override void OnRegister() {
            view.Init();
            Init();
            UpdateListeners(true);
        }

        public override void OnRemove() {
            UpdateListeners(false);
        }

    }
}


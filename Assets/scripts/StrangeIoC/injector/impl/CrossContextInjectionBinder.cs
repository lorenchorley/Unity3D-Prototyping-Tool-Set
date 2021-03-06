using strange.extensions.injector.impl;
using strange.extensions.injector.api;
using strange.framework.api;
using UnityEngine;

namespace strange.extensions.injector.impl {
    public class CrossContextInjectionBinder : InjectionBinder, ICrossContextInjectionBinder {

        /// Cross Context Injector is shared with all child contexts.
        public IInjectionBinder CrossContextBinder { get; set; }

        public CrossContextInjectionBinder() {
        }

        public override IInjectionBinding GetBinding<T>() {
            return GetBinding(typeof(T), null);
        }

        public override IInjectionBinding GetBinding(object key, object name) {

            IInjectionBinding binding = base.GetBinding(key, name) as IInjectionBinding;


            if (binding == null) //Attempt to get this from the cross context. Cross context is always SECOND PRIORITY. Local injections always override
            {
                if (CrossContextBinder != null) {
                    binding = CrossContextBinder.GetBinding(key, name) as IInjectionBinding;
                }
            }

            return binding;
        }

        override public void ResolveBinding(IBinding binding, object key) {
            //Decide whether to resolve locally or not
            if (binding is IInjectionBinding) {
                InjectionBinding injectionBinding = (InjectionBinding) binding;
                if (injectionBinding.isCrossContext) {

                    if (CrossContextBinder == null) //We are a crosscontextbinder
                    {
                        base.ResolveBinding(binding, key);
                    } else {
                        Unbind(key); //remove this cross context binding from the local binder
                        CrossContextBinder.ResolveBinding(binding, key);

                        //Need to bring over any values from the local injector factory
                        // This is the local injector
                        object value = injector.factory.Get(injectionBinding);

                        if (value != null) {

                            // Move bound value to cross context
                            CrossContextBinder.injector.factory.ManualSet(injectionBinding, value);

                        }

                    }
                } else {
                    base.ResolveBinding(binding, key);
                }
            }
        }

        protected override IInjector GetInjectorForBinding(IInjectionBinding binding) {
            if (binding.isCrossContext && CrossContextBinder != null) {
                return CrossContextBinder.injector;
            } else {
                return injector;
            }
        }
    }
}
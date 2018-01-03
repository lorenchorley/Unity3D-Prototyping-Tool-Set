using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.api;
using System;
using strange.extensions.injector.api;
using strange.framework.api;
using System.Collections.Generic;
using strange.extensions.injector.impl;

public class CrossContext : Context, ICrossContextCapable {

    private static int CrossContextCounter = 0;

    private ICrossContextInjectionBinder _injectionBinder;
    /// A Binder that handles dependency injection binding and instantiation
    public ICrossContextInjectionBinder injectionBinder {
        get {
            if (_injectionBinder == null) {
                _injectionBinder = new CrossContextInjectionBinder();
                _injectionBinder.SetBinderName("InjectionBinder for " + GetType().Name);
            }
            return _injectionBinder;
        }
        set {
            _injectionBinder = value;
        }
    }

    public IInjectionBinding BindSignalCrossContext<S>() {
        IInjectionBinding binding = injectionBinder.GetBinding<S>() ?? injectionBinder.Bind<S>();
        binding.CrossContext();

        object instance = injectionBinder.GetInstance<S>();
        binding.ToValue(instance);

        return binding;
    }

    public IInjectionBinding BindCrossContext(Type key) {
        return (injectionBinder.GetBinding(key) ?? injectionBinder.Bind(key)).CrossContext();
    }

    public void BindCrossContext(object key) {
        IInjectionBinding binding = injectionBinder.GetBinding(key);

        if (binding == null)
            throw new Exception("Object " + key.ToString() + " not yet bound, cannot make cross context");

        binding.CrossContext();
    }

    /// A specific instance of EventDispatcher that communicates 
    /// across multiple contexts. An event sent across this 
    /// dispatcher will be re-dispatched by the various context-wide 
    /// dispatchers. So a dispatch to other contexts is simply 
    /// 
    /// `crossContextDispatcher.Dispatch(MY_EVENT, payload)`;
    /// 
    /// Other contexts don't need to listen to the cross-context dispatcher
    /// as such, just map the necessary event to your local context
    /// dispatcher and you'll receive it.
    protected IEventDispatcher _crossContextDispatcher;

    public CrossContext() { }

    public CrossContext(object view, bool autoStartup, bool useSignals) : base(view, autoStartup, useSignals) {
    }

    protected override void addCoreComponents() {
        base.addCoreComponents();

        if (injectionBinder.CrossContextBinder == null)  //Only null if it could not find a parent context / firstContext
        {
            injectionBinder.CrossContextBinder = new CrossContextInjectionBinder();
            injectionBinder.CrossContextBinder.SetBinderName("CrossContext " + CrossContextCounter++);
        }

        if (firstContext == this) {
            injectionBinder.Bind<IEventDispatcher>().To<EventDispatcher>().ToSingleton().ToName(ContextKeys.CROSS_CONTEXT_DISPATCHER);
        } else if (crossContextDispatcher != null) {
            injectionBinder.Bind<IEventDispatcher>().ToValue(crossContextDispatcher).ToName(ContextKeys.CROSS_CONTEXT_DISPATCHER);
        }
    }

    protected override void instantiateCoreComponents() {
        base.instantiateCoreComponents();

        IEventDispatcher dispatcher = injectionBinder.GetInstance<IEventDispatcher>(ContextKeys.CONTEXT_DISPATCHER) as IEventDispatcher;

        if (dispatcher != null) {
            crossContextDispatcher = injectionBinder.GetInstance<IEventDispatcher>(ContextKeys.CROSS_CONTEXT_DISPATCHER) as IEventDispatcher;
            (crossContextDispatcher as ITriggerProvider).AddTriggerable(dispatcher as ITriggerable);
        }
    }

    List<ICrossContextCapable> ConnectedContexts;

    override public IContext AddContext(IContext context) {
        base.AddContext(context);
        if (context is ICrossContextCapable) {
            AssignCrossContext((ICrossContextCapable) context);
        }

        return this;
    }

    virtual public void AssignCrossContext(ICrossContextCapable childContext) {
        childContext.crossContextDispatcher = crossContextDispatcher;
        childContext.injectionBinder.CrossContextBinder = injectionBinder.CrossContextBinder;

        if (ConnectedContexts == null)
            ConnectedContexts = new List<ICrossContextCapable>();

        ConnectedContexts.Add(childContext);

    }

    virtual public void RemoveCrossContext(ICrossContextCapable childContext) {
        if (childContext.crossContextDispatcher != null) {
            ((childContext.crossContextDispatcher) as ITriggerProvider).RemoveTriggerable(childContext.GetComponent<IEventDispatcher>(ContextKeys.CONTEXT_DISPATCHER) as ITriggerable);
            childContext.crossContextDispatcher = null;
        }

        if (ConnectedContexts != null)
            ConnectedContexts.Remove(childContext);

    }

    IEnumerable<ICrossContextCapable> ICrossContextCapable.Contexts => ConnectedContexts;

    override public IContext RemoveContext(IContext context) {
        if (context is ICrossContextCapable) {
            RemoveCrossContext((ICrossContextCapable) context);
        }

        return base.RemoveContext(context);
    }

    virtual public IDispatcher crossContextDispatcher {
        get {
            return _crossContextDispatcher;
        }
        set {
            _crossContextDispatcher = value as IEventDispatcher;
        }
    }

}
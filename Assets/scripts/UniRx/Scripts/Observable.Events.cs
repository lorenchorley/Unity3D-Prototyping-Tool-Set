using System;
using UniRx.Operators;

namespace UniRx {
    public static partial class Observable {

        /// <summary>
        /// <para>The FromEventPattern operator is similar to FromEvent, except that instead of 
        /// taking an element and an event name as parameters, it takes two functions as 
        /// parameters. The first function attaches an event listener to a variety of events 
        /// on a variety of elements; the second function removes this set of listeners. In this 
        /// way you can establish a single Observable that emits items representing a variety of 
        /// events and a variety of target elements.</para>
        /// 
        /// <para>Set of similar extensions: FromEvent FromEventPattern</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/from.html"/>
        public static IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
            where TEventArgs : EventArgs {
            return new FromEventPatternObservable<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);
        }

        /// <summary>
        /// <para>The FromEvent operator takes an �element� and an event name as parameters, 
        /// and it then listens for events of that name taking place on that element. It returns 
        /// an Observable that emits those events.</para>
        /// 
        /// <para>Set of similar extensions: FromEvent FromEventPattern</para>
        /// 
        /// <para>This operator also takes an optional third parameter: a function that accepts 
        /// the arguments from the event handler as parameters and returns an item to be emitted 
        /// by the resulting Observable in place of the event.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/from.html"/>
        public static IObservable<Unit> FromEvent<TDelegate>(Func<Action, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler) {
            return new FromEventObservable<TDelegate>(conversion, addHandler, removeHandler);
        }

        /// <summary>
        /// <para>The FromEvent operator takes an �element� and an event name as parameters, 
        /// and it then listens for events of that name taking place on that element. It returns 
        /// an Observable that emits those events.</para>
        /// 
        /// <para>Set of similar extensions: FromEvent FromEventPattern</para>
        /// 
        /// <para>This operator also takes an optional third parameter: a function that accepts 
        /// the arguments from the event handler as parameters and returns an item to be emitted 
        /// by the resulting Observable in place of the event.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/from.html"/>
        public static IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler) {
            return new FromEventObservable<TDelegate, TEventArgs>(conversion, addHandler, removeHandler);
        }

        /// <summary>
        /// <para>The FromEvent operator takes an �element� and an event name as parameters, 
        /// and it then listens for events of that name taking place on that element. It returns 
        /// an Observable that emits those events.</para>
        /// 
        /// <para>Set of similar extensions: FromEvent FromEventPattern</para>
        /// 
        /// <para>This operator also takes an optional third parameter: a function that accepts 
        /// the arguments from the event handler as parameters and returns an item to be emitted 
        /// by the resulting Observable in place of the event.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/from.html"/>
        public static IObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler) {
            return new FromEventObservable(addHandler, removeHandler);
        }

        /// <summary>
        /// <para>The FromEvent operator takes an �element� and an event name as parameters, 
        /// and it then listens for events of that name taking place on that element. It returns 
        /// an Observable that emits those events.</para>
        /// 
        /// <para>Set of similar extensions: FromEvent FromEventPattern</para>
        /// 
        /// <para>This operator also takes an optional third parameter: a function that accepts 
        /// the arguments from the event handler as parameters and returns an item to be emitted 
        /// by the resulting Observable in place of the event.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/from.html"/>
        public static IObservable<T> FromEvent<T>(Action<Action<T>> addHandler, Action<Action<T>> removeHandler) {
            return new FromEventObservable_<T>(addHandler, removeHandler);
        }
    }
}
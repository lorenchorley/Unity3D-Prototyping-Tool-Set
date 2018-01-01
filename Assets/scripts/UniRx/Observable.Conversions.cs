using System;
using System.Collections.Generic;
using UniRx.Operators;

namespace UniRx {
    public static partial class Observable {

        /// <summary>
        /// <para>Convert various other objects and data types into Observables (Sometimes called From)</para>
        /// 
        /// <para>When you work with Observables, it can be more convenient if all of the 
        /// data you mean to work with can be represented as Observables, rather than as 
        /// a mixture of Observables and other types. This allows you to use a single set 
        /// of operators to govern the entire lifespan of the data stream.</para>
        /// 
        /// <para>Iterables, for example, can be thought of as a sort of synchronous Observable; 
        /// Futures, as a sort of Observable that always emits only a single item. By explicitly 
        /// converting such objects to Observables, you allow them to interact as peers with 
        /// other Observables.</para>
        /// 
        /// <para>For this reason, most ReactiveX implementations have methods that allow you 
        /// to convert certain language-specific objects and data structures into Observables.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/from.html"/>
        public static IObservable<T> AsObservable<T>(this IObservable<T> source) {
            if (source == null)
                throw new ArgumentNullException("source");

            // optimize, don't double wrap
            if (source is UniRx.Operators.AsObservableObservable<T>) {
                return source;
            }

            return new AsObservableObservable<T>(source);
        }

        /// <summary>
        /// <para>Convert various other objects and data types into Observables (Sometimes called From)</para>
        /// 
        /// <para>When you work with Observables, it can be more convenient if all of the 
        /// data you mean to work with can be represented as Observables, rather than as 
        /// a mixture of Observables and other types. This allows you to use a single set 
        /// of operators to govern the entire lifespan of the data stream.</para>
        /// 
        /// <para>Iterables, for example, can be thought of as a sort of synchronous Observable; 
        /// Futures, as a sort of Observable that always emits only a single item. By explicitly 
        /// converting such objects to Observables, you allow them to interact as peers with 
        /// other Observables.</para>
        /// 
        /// <para>For this reason, most ReactiveX implementations have methods that allow you 
        /// to convert certain language-specific objects and data structures into Observables.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/from.html"/>
        public static IObservable<T> ToObservable<T>(this IEnumerable<T> source) {
            return ToObservable(source, Scheduler.DefaultSchedulers.Iteration);
        }

        /// <summary>
        /// <para>Convert various other objects and data types into Observables (Sometimes called From)</para>
        /// 
        /// <para>When you work with Observables, it can be more convenient if all of the 
        /// data you mean to work with can be represented as Observables, rather than as 
        /// a mixture of Observables and other types. This allows you to use a single set 
        /// of operators to govern the entire lifespan of the data stream.</para>
        /// 
        /// <para>Iterables, for example, can be thought of as a sort of synchronous Observable; 
        /// Futures, as a sort of Observable that always emits only a single item. By explicitly 
        /// converting such objects to Observables, you allow them to interact as peers with 
        /// other Observables.</para>
        /// 
        /// <para>For this reason, most ReactiveX implementations have methods that allow you 
        /// to convert certain language-specific objects and data structures into Observables.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/from.html"/>
        public static IObservable<T> ToObservable<T>(this IEnumerable<T> source, IScheduler scheduler) {
            return new ToObservableObservable<T>(source, scheduler);
        }

        /// <summary>
        /// The cast operator is a specialized version of Select that transforms each item from the source 
        /// Observable by casting it into a particular Class before reemitting it.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/map.html"/>
        public static IObservable<TResult> Cast<TSource, TResult>(this IObservable<TSource> source) {
            return new CastObservable<TSource, TResult>(source);
        }

        /// <summary>
        /// The cast operator is a specialized version of Select that transforms each item from the source 
        /// Observable by casting it into a particular Class before reemitting it. Witness is for type inference.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/map.html"/>
        public static IObservable<TResult> Cast<TSource, TResult>(this IObservable<TSource> source, TResult witness) {
            return new CastObservable<TSource, TResult>(source);
        }

        /// <summary>
        /// <para>There is also a specialized form of the Filter operator in RxGroovy that filters an 
        /// Observable so that it only emits items of a particular class.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static IObservable<TResult> OfType<TSource, TResult>(this IObservable<TSource> source) {
            return new OfTypeObservable<TSource, TResult>(source);
        }

        /// <summary>
        /// <para>There is also a specialized form of the Filter operator in RxGroovy that filters an 
        /// Observable so that it only emits items of a particular class.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static IObservable<TResult> OfType<TSource, TResult>(this IObservable<TSource> source, TResult witness) {
            return new OfTypeObservable<TSource, TResult>(source);
        }

        /// <summary>
        /// Equivalent to .Select(_ => Unit.Default)
        /// </summary>
        public static IObservable<Unit> AsUnitObservable<T>(this IObservable<T> source) {
            return new AsUnitObservableObservable<T>(source);
        }

        /// <summary>
        /// Equivalent to LastOrDefault().AsUnitObservable()
        /// </summary>
        public static IObservable<Unit> AsSingleUnitObservable<T>(this IObservable<T> source) {
            return new AsSingleUnitObservableObservable<T>(source);
        }
    }
}
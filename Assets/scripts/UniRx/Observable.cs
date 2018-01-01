using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx.Operators;
using UnityEngine;

namespace UniRx
{
    // Standard Query Operators

    // onNext implementation guide. enclose otherFunc but onNext is not catch.
    // try{ otherFunc(); } catch { onError() }
    // onNext();

    public static partial class Observable
    {
        static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, -1); // from .NET 4.5

        /// <summary>
        /// <para>Transform the items emitted by an Observable by applying a function to each item (More often called Map)</para>
        /// 
        /// <para>The Map/Select operator applies a function of your choosing to each item emitted 
        /// by the source Observable, and returns an Observable that emits the results of these 
        /// function applications.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/map.html"/>
        public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, TR> selector)
        {
            // sometimes cause "which no ahead of time (AOT) code was generated." on IL2CPP...

            //var select = source as ISelect<T>;
            //if (select != null)
            //{
            //    return select.CombineSelector(selector);
            //}

            // optimized path
            var whereObservable = source as UniRx.Operators.WhereObservable<T>;
            if (whereObservable != null)
            {
                return whereObservable.CombineSelector<TR>(selector);
            }

            return new SelectObservable<T, TR>(source, selector);
        }

        /// <summary>
        /// <para>Transform the items emitted by an Observable by applying a function to each item (More often called Map)</para>
        /// 
        /// <para>The Map/Select operator applies a function of your choosing to each item emitted 
        /// by the source Observable, and returns an Observable that emits the results of these 
        /// function applications.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/map.html"/>
        public static IObservable<TR> Select<T, TR>(this IObservable<T> source, Func<T, int, TR> selector)
        {
            return new SelectObservable<T, TR>(source, selector);
        }

        /// <summary>
        /// <para>Emit only those items from an Observable that pass a predicate test (Sometimes call Filter)</para>
        /// 
        /// <para>The Filter operator filters an Observable by only allowing items through that pass a test that 
        /// you specify in the form of a predicate function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/filter.html"/>
        public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            // optimized path
            var whereObservable = source as UniRx.Operators.WhereObservable<T>;
            if (whereObservable != null)
            {
                return whereObservable.CombinePredicate(predicate);
            }

            var selectObservable = source as UniRx.Operators.ISelect<T>;
            if (selectObservable != null)
            {
                return selectObservable.CombinePredicate(predicate);
            }

            return new WhereObservable<T>(source, predicate);
        }

        /// <summary>
        /// <para>Emit only those items from an Observable that pass a predicate test (Sometimes call Filter)</para>
        /// 
        /// <para>The Filter operator filters an Observable by only allowing items through that pass a test that 
        /// you specify in the form of a predicate function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/filter.html"/>
        public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, int, bool> predicate)
        {
            return new WhereObservable<T>(source, predicate);
        }

        /// <summary>
        /// Lightweight SelectMany for Single Async Operation.
        /// </summary>
        public static IObservable<TR> ContinueWith<T, TR>(this IObservable<T> source, IObservable<TR> other)
        {
            return ContinueWith(source, _ => other);
        }

        /// <summary>
        /// Lightweight SelectMany for Single Async Operation.
        /// </summary>
        public static IObservable<TR> ContinueWith<T, TR>(this IObservable<T> source, Func<T, IObservable<TR>> selector)
        {
            return new ContinueWithObservable<T, TR>(source, selector);
        }

        /// <summary>
        /// <para>Transform the items emitted by an Observable into Observables, 
        /// then flatten the emissions from those into a single Observable (Sometimes called FlatMap)</para>
        /// 
        /// <para>The FlatMap operator transforms an Observable by applying a function that you specify 
        /// to each item emitted by the source Observable, where that function returns an Observable 
        /// that itself emits items. FlatMap then merges the emissions of these resulting Observables, 
        /// emitting these merged results as its own sequence.</para>
        /// 
        /// <para>This method is useful, for example, when you have an Observable that emits a series of 
        /// items that themselves have Observable members or are in other ways transformable into 
        /// Observables, so that you can create a new Observable that emits the complete collection 
        /// of items emitted by the sub-Observables of these items.</para>
        /// 
        /// <para>Note that FlatMap merges the emissions of these Observables, so that they may interleave.</para>
        /// 
        /// <para>In several of the language-specific implementations there is also an operator that does 
        /// not interleave the emissions from the transformed Observables, but instead emits these 
        /// emissions in strict order, often called ConcatMap or something similar.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/flatmap.html"/>
        public static IObservable<TR> SelectMany<T, TR>(this IObservable<T> source, IObservable<TR> other)
        {
            return SelectMany(source, _ => other);
        }

        /// <summary>
        /// <para>Transform the items emitted by an Observable into Observables, 
        /// then flatten the emissions from those into a single Observable (Sometimes called FlatMap)</para>
        /// 
        /// <para>The FlatMap operator transforms an Observable by applying a function that you specify 
        /// to each item emitted by the source Observable, where that function returns an Observable 
        /// that itself emits items. FlatMap then merges the emissions of these resulting Observables, 
        /// emitting these merged results as its own sequence.</para>
        /// 
        /// <para>This method is useful, for example, when you have an Observable that emits a series of 
        /// items that themselves have Observable members or are in other ways transformable into 
        /// Observables, so that you can create a new Observable that emits the complete collection 
        /// of items emitted by the sub-Observables of these items.</para>
        /// 
        /// <para>Note that FlatMap merges the emissions of these Observables, so that they may interleave.</para>
        /// 
        /// <para>In several of the language-specific implementations there is also an operator that does 
        /// not interleave the emissions from the transformed Observables, but instead emits these 
        /// emissions in strict order, often called ConcatMap or something similar.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/flatmap.html"/>
        public static IObservable<TR> SelectMany<T, TR>(this IObservable<T> source, Func<T, IObservable<TR>> selector)
        {
            return new SelectManyObservable<T, TR>(source, selector);
        }

        /// <summary>
        /// <para>Transform the items emitted by an Observable into Observables, 
        /// then flatten the emissions from those into a single Observable (Sometimes called FlatMap)</para>
        /// 
        /// <para>The FlatMap operator transforms an Observable by applying a function that you specify 
        /// to each item emitted by the source Observable, where that function returns an Observable 
        /// that itself emits items. FlatMap then merges the emissions of these resulting Observables, 
        /// emitting these merged results as its own sequence.</para>
        /// 
        /// <para>This method is useful, for example, when you have an Observable that emits a series of 
        /// items that themselves have Observable members or are in other ways transformable into 
        /// Observables, so that you can create a new Observable that emits the complete collection 
        /// of items emitted by the sub-Observables of these items.</para>
        /// 
        /// <para>Note that FlatMap merges the emissions of these Observables, so that they may interleave.</para>
        /// 
        /// <para>In several of the language-specific implementations there is also an operator that does 
        /// not interleave the emissions from the transformed Observables, but instead emits these 
        /// emissions in strict order, often called ConcatMap or something similar.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/flatmap.html"/>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
        {
            return new SelectManyObservable<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// <para>Transform the items emitted by an Observable into Observables, 
        /// then flatten the emissions from those into a single Observable (Sometimes called FlatMap)</para>
        /// 
        /// <para>The FlatMap operator transforms an Observable by applying a function that you specify 
        /// to each item emitted by the source Observable, where that function returns an Observable 
        /// that itself emits items. FlatMap then merges the emissions of these resulting Observables, 
        /// emitting these merged results as its own sequence.</para>
        /// 
        /// <para>This method is useful, for example, when you have an Observable that emits a series of 
        /// items that themselves have Observable members or are in other ways transformable into 
        /// Observables, so that you can create a new Observable that emits the complete collection 
        /// of items emitted by the sub-Observables of these items.</para>
        /// 
        /// <para>Note that FlatMap merges the emissions of these Observables, so that they may interleave.</para>
        /// 
        /// <para>In several of the language-specific implementations there is also an operator that does 
        /// not interleave the emissions from the transformed Observables, but instead emits these 
        /// emissions in strict order, often called ConcatMap or something similar.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/flatmap.html"/>
        public static IObservable<TR> SelectMany<T, TC, TR>(this IObservable<T> source, Func<T, IObservable<TC>> collectionSelector, Func<T, TC, TR> resultSelector)
        {
            return new SelectManyObservable<T, TC, TR>(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// <para>Transform the items emitted by an Observable into Observables, 
        /// then flatten the emissions from those into a single Observable (Sometimes called FlatMap)</para>
        /// 
        /// <para>The FlatMap operator transforms an Observable by applying a function that you specify 
        /// to each item emitted by the source Observable, where that function returns an Observable 
        /// that itself emits items. FlatMap then merges the emissions of these resulting Observables, 
        /// emitting these merged results as its own sequence.</para>
        /// 
        /// <para>This method is useful, for example, when you have an Observable that emits a series of 
        /// items that themselves have Observable members or are in other ways transformable into 
        /// Observables, so that you can create a new Observable that emits the complete collection 
        /// of items emitted by the sub-Observables of these items.</para>
        /// 
        /// <para>Note that FlatMap merges the emissions of these Observables, so that they may interleave.</para>
        /// 
        /// <para>In several of the language-specific implementations there is also an operator that does 
        /// not interleave the emissions from the transformed Observables, but instead emits these 
        /// emissions in strict order, often called ConcatMap or something similar.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/flatmap.html"/>
        public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            return new SelectManyObservable<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// <para>Transform the items emitted by an Observable into Observables, 
        /// then flatten the emissions from those into a single Observable (Sometimes called FlatMap)</para>
        /// 
        /// <para>The FlatMap operator transforms an Observable by applying a function that you specify 
        /// to each item emitted by the source Observable, where that function returns an Observable 
        /// that itself emits items. FlatMap then merges the emissions of these resulting Observables, 
        /// emitting these merged results as its own sequence.</para>
        /// 
        /// <para>This method is useful, for example, when you have an Observable that emits a series of 
        /// items that themselves have Observable members or are in other ways transformable into 
        /// Observables, so that you can create a new Observable that emits the complete collection 
        /// of items emitted by the sub-Observables of these items.</para>
        /// 
        /// <para>Note that FlatMap merges the emissions of these Observables, so that they may interleave.</para>
        /// 
        /// <para>In several of the language-specific implementations there is also an operator that does 
        /// not interleave the emissions from the transformed Observables, but instead emits these 
        /// emissions in strict order, often called ConcatMap or something similar.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/flatmap.html"/>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return new SelectManyObservable<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// <para>Transform the items emitted by an Observable into Observables, 
        /// then flatten the emissions from those into a single Observable (Sometimes called FlatMap)</para>
        /// 
        /// <para>The FlatMap operator transforms an Observable by applying a function that you specify 
        /// to each item emitted by the source Observable, where that function returns an Observable 
        /// that itself emits items. FlatMap then merges the emissions of these resulting Observables, 
        /// emitting these merged results as its own sequence.</para>
        /// 
        /// <para>This method is useful, for example, when you have an Observable that emits a series of 
        /// items that themselves have Observable members or are in other ways transformable into 
        /// Observables, so that you can create a new Observable that emits the complete collection 
        /// of items emitted by the sub-Observables of these items.</para>
        /// 
        /// <para>Note that FlatMap merges the emissions of these Observables, so that they may interleave.</para>
        /// 
        /// <para>In several of the language-specific implementations there is also an operator that does 
        /// not interleave the emissions from the transformed Observables, but instead emits these 
        /// emissions in strict order, often called ConcatMap or something similar.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/flatmap.html"/>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            return new SelectManyObservable<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// <para>Transform the items emitted by an Observable into Observables, 
        /// then flatten the emissions from those into a single Observable (Sometimes called FlatMap)</para>
        /// 
        /// <para>The FlatMap operator transforms an Observable by applying a function that you specify 
        /// to each item emitted by the source Observable, where that function returns an Observable 
        /// that itself emits items. FlatMap then merges the emissions of these resulting Observables, 
        /// emitting these merged results as its own sequence.</para>
        /// 
        /// <para>This method is useful, for example, when you have an Observable that emits a series of 
        /// items that themselves have Observable members or are in other ways transformable into 
        /// Observables, so that you can create a new Observable that emits the complete collection 
        /// of items emitted by the sub-Observables of these items.</para>
        /// 
        /// <para>Note that FlatMap merges the emissions of these Observables, so that they may interleave.</para>
        /// 
        /// <para>In several of the language-specific implementations there is also an operator that does 
        /// not interleave the emissions from the transformed Observables, but instead emits these 
        /// emissions in strict order, often called ConcatMap or something similar.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/flatmap.html"/>
        public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return new SelectManyObservable<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// <para>Transform the items emitted by an Observable into Observables, 
        /// then flatten the emissions from those into a single Observable (Sometimes called FlatMap)</para>
        /// 
        /// <para>The FlatMap operator transforms an Observable by applying a function that you specify 
        /// to each item emitted by the source Observable, where that function returns an Observable 
        /// that itself emits items. FlatMap then merges the emissions of these resulting Observables, 
        /// emitting these merged results as its own sequence.</para>
        /// 
        /// <para>This method is useful, for example, when you have an Observable that emits a series of 
        /// items that themselves have Observable members or are in other ways transformable into 
        /// Observables, so that you can create a new Observable that emits the complete collection 
        /// of items emitted by the sub-Observables of these items.</para>
        /// 
        /// <para>Note that FlatMap merges the emissions of these Observables, so that they may interleave.</para>
        /// 
        /// <para>In several of the language-specific implementations there is also an operator that does 
        /// not interleave the emissions from the transformed Observables, but instead emits these 
        /// emissions in strict order, often called ConcatMap or something similar.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/flatmap.html"/>
        public static IObservable<TResult> SelectMany<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            return new SelectManyObservable<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// <para>Convert an Observable into another object or data structure</para>
        /// 
        /// <para>The various language-specific implementations of ReactiveX have a variety of operators 
        /// that you can use to convert an Observable, or a sequence of items emitted by an Observable, 
        /// into another variety of object or data structure. Some of these block until the Observable 
        /// terminates and then produce an equivalent object or data structure; others return an 
        /// Observable that emits such an object or data structure.</para>
        /// 
        /// <para>In some implementations of ReactiveX, there is also an operator that converts an Observable 
        /// into a �Blocking� Observable.A Blocking Observable extends the ordinary Observable by 
        /// providing a set of methods, operating on the items emitted by the Observable, that block.
        /// Some of the To operators are in this Blocking Obsevable set of extended operations.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/to.html"/>
        public static IObservable<T[]> ToArray<T>(this IObservable<T> source)
        {
            return new ToArrayObservable<T>(source);
        }

        /// <summary>
        /// <para>Convert an Observable into another object or data structure</para>
        /// 
        /// <para>The various language-specific implementations of ReactiveX have a variety of operators 
        /// that you can use to convert an Observable, or a sequence of items emitted by an Observable, 
        /// into another variety of object or data structure. Some of these block until the Observable 
        /// terminates and then produce an equivalent object or data structure; others return an 
        /// Observable that emits such an object or data structure.</para>
        /// 
        /// <para>In some implementations of ReactiveX, there is also an operator that converts an Observable 
        /// into a �Blocking� Observable.A Blocking Observable extends the ordinary Observable by 
        /// providing a set of methods, operating on the items emitted by the Observable, that block.
        /// Some of the To operators are in this Blocking Obsevable set of extended operations.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/to.html"/>
        public static IObservable<IList<T>> ToList<T>(this IObservable<T> source)
        {
            return new ToListObservable<T>(source);
        }

        /// <summary>
        /// The Do operator allows you to establish a callback that the resulting Observable will call each time it emits an item.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> Do<T>(this IObservable<T> source, IObserver<T> observer)
        {
            return new DoObserverObservable<T>(source, observer);
        }

        /// <summary>
        /// The Do operator allows you to establish a callback that the resulting Observable will call each time it emits an item.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext)
        {
            return new DoObservable<T>(source, onNext, Stubs.Throw, Stubs.Nop);
        }

        /// <summary>
        /// The Do operator allows you to establish a callback that the resulting Observable will call each time it emits an item.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError)
        {
            return new DoObservable<T>(source, onNext, onError, Stubs.Nop);
        }

        /// <summary>
        /// The Do operator allows you to establish a callback that the resulting Observable will call each time it emits an item.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted)
        {
            return new DoObservable<T>(source, onNext, Stubs.Throw, onCompleted);
        }

        /// <summary>
        /// The Do operator allows you to establish a callback that the resulting Observable will call each time it emits an item.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> Do<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            return new DoObservable<T>(source, onNext, onError, onCompleted);
        }

        /// <summary>
        /// The DoOnError operator registers an Action which will be called if the resulting Observable terminates abnormally, 
        /// calling onError. This Action will be passed the Throwable representing the error.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> DoOnError<T>(this IObservable<T> source, Action<Exception> onError)
        {
            return new DoOnErrorObservable<T>(source, onError);
        }

        /// <summary>
        /// The DoOnCompleted operator registers an Action which will be called if the resulting Observable terminates 
        /// normally, calling onCompleted.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> DoOnCompleted<T>(this IObservable<T> source, Action onCompleted)
        {
            return new DoOnCompletedObservable<T>(source, onCompleted);
        }

        /// <summary>
        /// The DoOnTerminate operator registers an Action which will be called just before the resulting Observable 
        /// terminates, whether normally or with an error.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> DoOnTerminate<T>(this IObservable<T> source, Action onTerminate)
        {
            return new DoOnTerminateObservable<T>(source, onTerminate);
        }

        /// <summary>
        /// The DoOnSubscribe operator registers an Action which will be called whenever an observer subscribes to the resulting Observable.
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> DoOnSubscribe<T>(this IObservable<T> source, Action onSubscribe)
        {
            return new DoOnSubscribeObservable<T>(source, onSubscribe);
        }

        /// <summary>
        /// Register an action to take upon a variety of Observable lifecycle events
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> DoOnCancel<T>(this IObservable<T> source, Action onCancel)
        {
            return new DoOnCancelObservable<T>(source, onCancel);
        }

        /// <summary>
        /// <para>represent both the items emitted and the notifications sent as emitted items, or reverse 
        /// this process</para>
        /// 
        /// <para>A well-formed, finite Observable will invoke its observer�s onNext method zero or more times, 
        /// and then will invoke either the onCompleted or onError method exactly once. The Materialize 
        /// operator converts this series of invocations � both the original onNext notifications and 
        /// the terminal onCompleted or onError notification � into a series of items emitted by an Observable.</para>
        /// 
        /// <para>The Dematerialize operator reverses this process. It operates on an Observable that has 
        /// previously been transformed by Materialize and returns it to its original form.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/materialize-dematerialize.html"/>
        public static IObservable<Notification<T>> Materialize<T>(this IObservable<T> source)
        {
            return new MaterializeObservable<T>(source);
        }

        /// <summary>
        /// <para>represent both the items emitted and the notifications sent as emitted items, or reverse 
        /// this process</para>
        /// 
        /// <para>A well-formed, finite Observable will invoke its observer�s onNext method zero or more times, 
        /// and then will invoke either the onCompleted or onError method exactly once. The Materialize 
        /// operator converts this series of invocations � both the original onNext notifications and 
        /// the terminal onCompleted or onError notification � into a series of items emitted by an Observable.</para>
        /// 
        /// <para>The Dematerialize operator reverses this process. It operates on an Observable that has 
        /// previously been transformed by Materialize and returns it to its original form.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/materialize-dematerialize.html"/>
        public static IObservable<T> Dematerialize<T>(this IObservable<Notification<T>> source)
        {
            return new DematerializeObservable<T>(source);
        }

        /// <summary>
        /// <para>Emit items from the source Observable, or a default item if the source Observable emits nothing</para>
        /// 
        /// <para>The DefaultIfEmpty operator simply mirrors the source Observable exactly if the source 
        /// Observable emits any items. If the source Observable terminates normally (with an onComplete) 
        /// without emitting any items, the Observable returned from DefaultIfEmpty will instead emit a 
        /// default item of your choosing before it too completes.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/defaultifempty.html"/>
        public static IObservable<T> DefaultIfEmpty<T>(this IObservable<T> source)
        {
            return new DefaultIfEmptyObservable<T>(source, default(T));
        }

        /// <summary>
        /// <para>Emit items from the source Observable, or a default item if the source Observable emits nothing</para>
        /// 
        /// <para>The DefaultIfEmpty operator simply mirrors the source Observable exactly if the source 
        /// Observable emits any items. If the source Observable terminates normally (with an onComplete) 
        /// without emitting any items, the Observable returned from DefaultIfEmpty will instead emit a 
        /// default item of your choosing before it too completes.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/defaultifempty.html"/>
        public static IObservable<T> DefaultIfEmpty<T>(this IObservable<T> source, T defaultValue)
        {
            return new DefaultIfEmptyObservable<T>(source, defaultValue);
        }

        /// <summary>
        /// <para>Suppress duplicate items emitted by an Observable</para>
        /// 
        /// <para>The Distinct operator filters an Observable by only allowing items through that have not 
        /// already been emitted.</para>
        /// 
        /// <para>In some implementations there are variants that allow you to adjust the criteria by 
        /// which two items are considered �distinct.� In some, there is a variant of the operator 
        /// that only compares an item against its immediate predecessor for distinctness, thereby 
        /// filtering only consecutive duplicate items from the sequence.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/distinct.html"/>
        public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source)
        {
#if !UniRxLibrary
            var comparer = UnityEqualityComparer.GetDefault<TSource>();
#else
            var comparer = EqualityComparer<TSource>.Default;
#endif

            return new DistinctObservable<TSource>(source, comparer);
        }

        /// <summary>
        /// <para>Suppress duplicate items emitted by an Observable</para>
        /// 
        /// <para>The Distinct operator filters an Observable by only allowing items through that have not 
        /// already been emitted.</para>
        /// 
        /// <para>In some implementations there are variants that allow you to adjust the criteria by 
        /// which two items are considered �distinct.� In some, there is a variant of the operator 
        /// that only compares an item against its immediate predecessor for distinctness, thereby 
        /// filtering only consecutive duplicate items from the sequence.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/distinct.html"/>
        public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return new DistinctObservable<TSource>(source, comparer);
        }

        /// <summary>
        /// <para>Suppress duplicate items emitted by an Observable</para>
        /// 
        /// <para>The Distinct operator filters an Observable by only allowing items through that have not 
        /// already been emitted.</para>
        /// 
        /// <para>In some implementations there are variants that allow you to adjust the criteria by 
        /// which two items are considered �distinct.� In some, there is a variant of the operator 
        /// that only compares an item against its immediate predecessor for distinctness, thereby 
        /// filtering only consecutive duplicate items from the sequence.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/distinct.html"/>
        public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
#if !UniRxLibrary
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
#else
            var comparer = EqualityComparer<TKey>.Default;
#endif

            return new DistinctObservable<TSource, TKey>(source, keySelector, comparer);
        }

        /// <summary>
        /// <para>Suppress duplicate items emitted by an Observable</para>
        /// 
        /// <para>The Distinct operator filters an Observable by only allowing items through that have not 
        /// already been emitted.</para>
        /// 
        /// <para>In some implementations there are variants that allow you to adjust the criteria by 
        /// which two items are considered �distinct.� In some, there is a variant of the operator 
        /// that only compares an item against its immediate predecessor for distinctness, thereby 
        /// filtering only consecutive duplicate items from the sequence.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/distinct.html"/>
        public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctObservable<TSource, TKey>(source, keySelector, comparer);
        }

        /// <summary>
        /// <para>Suppress duplicate items emitted by an Observable. DistinctUntilChanged only compares emitted 
        /// items from the source Observable against their immediate predecessors in order to determine 
        /// whether or not they are distinct. It takes the same two optional parameters as the distinct operator.</para>
        /// 
        /// <para>The Distinct operator filters an Observable by only allowing items through that have not 
        /// already been emitted.</para>
        /// 
        /// <para>In some implementations there are variants that allow you to adjust the criteria by 
        /// which two items are considered �distinct.� In some, there is a variant of the operator 
        /// that only compares an item against its immediate predecessor for distinctness, thereby 
        /// filtering only consecutive duplicate items from the sequence.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/distinct.html"/>
        public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source)
        {
#if !UniRxLibrary
            var comparer = UnityEqualityComparer.GetDefault<T>();
#else
            var comparer = EqualityComparer<T>.Default;
#endif

            return new DistinctUntilChangedObservable<T>(source, comparer);
        }

        /// <summary>
        /// <para>Suppress duplicate items emitted by an Observable. DistinctUntilChanged only compares emitted 
        /// items from the source Observable against their immediate predecessors in order to determine 
        /// whether or not they are distinct. It takes the same two optional parameters as the distinct operator.</para>
        /// 
        /// <para>The Distinct operator filters an Observable by only allowing items through that have not 
        /// already been emitted.</para>
        /// 
        /// <para>In some implementations there are variants that allow you to adjust the criteria by 
        /// which two items are considered �distinct.� In some, there is a variant of the operator 
        /// that only compares an item against its immediate predecessor for distinctness, thereby 
        /// filtering only consecutive duplicate items from the sequence.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/distinct.html"/>
        public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source, IEqualityComparer<T> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new DistinctUntilChangedObservable<T>(source, comparer);
        }

        /// <summary>
        /// <para>Suppress duplicate items emitted by an Observable. DistinctUntilChanged only compares emitted 
        /// items from the source Observable against their immediate predecessors in order to determine 
        /// whether or not they are distinct. It takes the same two optional parameters as the distinct operator.</para>
        /// 
        /// <para>The Distinct operator filters an Observable by only allowing items through that have not 
        /// already been emitted.</para>
        /// 
        /// <para>In some implementations there are variants that allow you to adjust the criteria by 
        /// which two items are considered �distinct.� In some, there is a variant of the operator 
        /// that only compares an item against its immediate predecessor for distinctness, thereby 
        /// filtering only consecutive duplicate items from the sequence.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/distinct.html"/>
        public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector)
        {
#if !UniRxLibrary
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
#else
            var comparer = EqualityComparer<TKey>.Default;
#endif

            return new DistinctUntilChangedObservable<T, TKey>(source, keySelector, comparer);
        }

        /// <summary>
        /// <para>Suppress duplicate items emitted by an Observable. DistinctUntilChanged only compares emitted 
        /// items from the source Observable against their immediate predecessors in order to determine 
        /// whether or not they are distinct. It takes the same two optional parameters as the distinct operator.</para>
        /// 
        /// <para>The Distinct operator filters an Observable by only allowing items through that have not 
        /// already been emitted.</para>
        /// 
        /// <para>In some implementations there are variants that allow you to adjust the criteria by 
        /// which two items are considered �distinct.� In some, there is a variant of the operator 
        /// that only compares an item against its immediate predecessor for distinctness, thereby 
        /// filtering only consecutive duplicate items from the sequence.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/distinct.html"/>
        public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new DistinctUntilChangedObservable<T, TKey>(source, keySelector, comparer);
        }

        /// <summary>
        /// <para>Do not emit any items from an Observable but mirror its termination notification</para>
        /// 
        /// <para>The IgnoreElements operator suppresses all of the items emitted by the source 
        /// Observable, but allows its termination notification (either onError or onCompleted) 
        /// to pass through unchanged.</para>
        /// 
        /// <para>If you do not care about the items being emitted by an Observable, but you do 
        /// want to be notified when it completes or when it terminates with an error, you 
        /// can apply the ignoreElements operator to the Observable, which will ensure that 
        /// it will never call its observers� onNext handlers.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/ignoreelements.html"/>
        public static IObservable<T> IgnoreElements<T>(this IObservable<T> source)
        {
            return new IgnoreElementsObservable<T>(source);
        }

        /// <summary>
        /// <para>ForEachAsync is an asynchronous version of subscribe, where a delegate is registered 
        /// and called after an object is emitted from the observable</para>
        /// 
        /// <para>May not be a perfect description, found on another implementations documentation!</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/subscribe.html"/>
        public static IObservable<Unit> ForEachAsync<T>(this IObservable<T> source, Action<T> onNext)
        {
            return new ForEachAsyncObservable<T>(source, onNext);
        }

        /// <summary>
        /// <para>ForEachAsync is an asynchronous version of subscribe, where a delegate is registered 
        /// and called after an object is emitted from the observable</para>
        /// 
        /// <para>May not be a perfect description, found on another implementations documentation!</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/subscribe.html"/>
        public static IObservable<Unit> ForEachAsync<T>(this IObservable<T> source, Action<T, int> onNext)
        {
            return new ForEachAsyncObservable<T>(source, onNext);
        }
        
    }
}
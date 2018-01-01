using System;
using System.Collections.Generic;
using UniRx.Operators;

namespace UniRx {
    public static partial class Observable {

        /// <summary>
        /// <para>Create an Observable from scratch by means of a function (Original: Create 
        /// anonymous observable. Observer has exception durability. This is recommended 
        /// for making operators and events like generators (HotObservable). )</para>
        /// 
        /// <para>You can create an Observable from scratch by using the Create operator. 
        /// You pass this operator a function that accepts the observer as its parameter. 
        /// Write this function so that it behaves as an Observable � by calling the 
        /// observer�s onNext, onError, and onCompleted methods appropriately.</para>
        /// 
        /// <para>A well-formed finite Observable must attempt to call either the observer�s 
        /// onCompleted method exactly once or its onError method exactly once, and 
        /// must not thereafter attempt to call any of the observer�s other methods.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/create.html"/>
        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe) {
            if (subscribe == null)
                throw new ArgumentNullException("subscribe");

            return new CreateObservable<T>(subscribe);
        }

        /// <summary>
        /// <para>Create an Observable from scratch by means of a function (Original: Create 
        /// anonymous observable. Observer has exception durability. This is recommended 
        /// for making operators and events like generators (HotObservable). )</para>
        /// 
        /// <para>You can create an Observable from scratch by using the Create operator. 
        /// You pass this operator a function that accepts the observer as its parameter. 
        /// Write this function so that it behaves as an Observable � by calling the 
        /// observer�s onNext, onError, and onCompleted methods appropriately.</para>
        /// 
        /// <para>A well-formed finite Observable must attempt to call either the observer�s 
        /// onCompleted method exactly once or its onError method exactly once, and 
        /// must not thereafter attempt to call any of the observer�s other methods.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/create.html"/>
        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread) {
            if (subscribe == null)
                throw new ArgumentNullException("subscribe");

            return new CreateObservable<T>(subscribe, isRequiredSubscribeOnCurrentThread);
        }

        /// <summary>
        /// <para>Create an Observable from scratch by means of a function (Original: Create 
        /// anonymous observable. Observer has exception durability. This is recommended 
        /// for making operators and events like generators.)</para>
        /// 
        /// <para>You can create an Observable from scratch by using the Create operator. 
        /// You pass this operator a function that accepts the observer as its parameter. 
        /// Write this function so that it behaves as an Observable � by calling the 
        /// observer�s onNext, onError, and onCompleted methods appropriately.</para>
        /// 
        /// <para>A well-formed finite Observable must attempt to call either the observer�s 
        /// onCompleted method exactly once or its onError method exactly once, and 
        /// must not thereafter attempt to call any of the observer�s other methods.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/create.html"/>
        public static IObservable<T> CreateWithState<T, TState>(TState state, Func<TState, IObserver<T>, IDisposable> subscribe) {
            if (subscribe == null)
                throw new ArgumentNullException("subscribe");

            return new CreateObservable<T, TState>(state, subscribe);
        }

        /// <summary>
        /// <para>Create an Observable from scratch by means of a function (Original: Create 
        /// anonymous observable. Observer has exception durability. This is recommended 
        /// for making operators and events like generators.)</para>
        /// 
        /// <para>You can create an Observable from scratch by using the Create operator. 
        /// You pass this operator a function that accepts the observer as its parameter. 
        /// Write this function so that it behaves as an Observable � by calling the 
        /// observer�s onNext, onError, and onCompleted methods appropriately.</para>
        /// 
        /// <para>A well-formed finite Observable must attempt to call either the observer�s 
        /// onCompleted method exactly once or its onError method exactly once, and 
        /// must not thereafter attempt to call any of the observer�s other methods.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/create.html"/>
        public static IObservable<T> CreateWithState<T, TState>(TState state, Func<TState, IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread) {
            if (subscribe == null)
                throw new ArgumentNullException("subscribe");

            return new CreateObservable<T, TState>(state, subscribe, isRequiredSubscribeOnCurrentThread);
        }

        /// <summary>
        /// <para>Create an Observable from scratch by means of a function (Original: Create 
        /// anonymous observable. Safe means auto detach when an error is raised in onNext 
        /// pipeline. This is recommended to make generators (ColdObservable).)</para>
        /// 
        /// <para>You can create an Observable from scratch by using the Create operator. 
        /// You pass this operator a function that accepts the observer as its parameter. 
        /// Write this function so that it behaves as an Observable � by calling the 
        /// observer�s onNext, onError, and onCompleted methods appropriately.</para>
        /// 
        /// <para>A well-formed finite Observable must attempt to call either the observer�s 
        /// onCompleted method exactly once or its onError method exactly once, and 
        /// must not thereafter attempt to call any of the observer�s other methods.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/create.html"/>
        public static IObservable<T> CreateSafe<T>(Func<IObserver<T>, IDisposable> subscribe) {
            if (subscribe == null)
                throw new ArgumentNullException("subscribe");

            return new CreateSafeObservable<T>(subscribe);
        }

        /// <summary>
        /// <para>Create an Observable from scratch by means of a function (Original: Create 
        /// anonymous observable. Safe means auto detach when an error is raised in onNext 
        /// pipeline. This is recommended to make generators (ColdObservable).)</para>
        /// 
        /// <para>You can create an Observable from scratch by using the Create operator. 
        /// You pass this operator a function that accepts the observer as its parameter. 
        /// Write this function so that it behaves as an Observable � by calling the 
        /// observer�s onNext, onError, and onCompleted methods appropriately.</para>
        /// 
        /// <para>A well-formed finite Observable must attempt to call either the observer�s 
        /// onCompleted method exactly once or its onError method exactly once, and 
        /// must not thereafter attempt to call any of the observer�s other methods.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/create.html"/>
        public static IObservable<T> CreateSafe<T>(Func<IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread) {
            if (subscribe == null)
                throw new ArgumentNullException("subscribe");

            return new CreateSafeObservable<T>(subscribe, isRequiredSubscribeOnCurrentThread);
        }

        /// <summary>
        /// <para>Create an Observable that emits no items but terminates normally</para>
        /// 
        /// <para>The Empty, Never, and Throw operators generate Observables with very specific and 
        /// limited behavior. These are useful for testing purposes, and sometimes also for 
        /// combining with other Observables or as parameters to operators that expect other 
        /// Observables as parameters.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/empty-never-throw.html"/>
        public static IObservable<T> Empty<T>() {
            return Empty<T>(Scheduler.DefaultSchedulers.ConstantTimeOperations);
        }

        /// <summary>
        /// <para>Create an Observable that emits no items but terminates normally. Returns only OnCompleted on specified scheduler.</para>
        /// 
        /// <para>The Empty, Never, and Throw operators generate Observables with very specific and 
        /// limited behavior. These are useful for testing purposes, and sometimes also for 
        /// combining with other Observables or as parameters to operators that expect other 
        /// Observables as parameters.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/empty-never-throw.html"/>
        public static IObservable<T> Empty<T>(IScheduler scheduler) {
            if (scheduler == Scheduler.Immediate) {
                return ImmutableEmptyObservable<T>.Instance;
            } else {
                return new EmptyObservable<T>(scheduler);
            }
        }

        /// <summary>
        /// <para>Create an Observable that emits no items but terminates normally. Returns only OnCompleted. 
        /// Witness is for type inference.</para>
        /// 
        /// <para>The Empty, Never, and Throw operators generate Observables with very specific and 
        /// limited behavior. These are useful for testing purposes, and sometimes also for 
        /// combining with other Observables or as parameters to operators that expect other 
        /// Observables as parameters.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/empty-never-throw.html"/>
        public static IObservable<T> Empty<T>(T witness) {
            return Empty<T>(Scheduler.DefaultSchedulers.ConstantTimeOperations);
        }

        /// <summary>
        /// <para>Create an Observable that emits no items but terminates normally. Returns only OnCompleted on specified scheduler. 
        /// Witness is for type inference.</para>
        /// 
        /// <para>The Empty, Never, and Throw operators generate Observables with very specific and 
        /// limited behavior. These are useful for testing purposes, and sometimes also for 
        /// combining with other Observables or as parameters to operators that expect other 
        /// Observables as parameters.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/empty-never-throw.html"/>
        public static IObservable<T> Empty<T>(IScheduler scheduler, T witness) {
            return Empty<T>(scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits no items and does not terminate</para>
        /// 
        /// <para>The Empty, Never, and Throw operators generate Observables with very specific and 
        /// limited behavior. These are useful for testing purposes, and sometimes also for 
        /// combining with other Observables or as parameters to operators that expect other 
        /// Observables as parameters.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/empty-never-throw.html"/>
        public static IObservable<T> Never<T>() {
            return ImmutableNeverObservable<T>.Instance;
        }

        /// <summary>
        /// <para>Create an Observable that emits no items and does not terminate. Witness is for type inference.</para>
        /// 
        /// <para>The Empty, Never, and Throw operators generate Observables with very specific and 
        /// limited behavior. These are useful for testing purposes, and sometimes also for 
        /// combining with other Observables or as parameters to operators that expect other 
        /// Observables as parameters.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/empty-never-throw.html"/>
        public static IObservable<T> Never<T>(T witness) {
            return ImmutableNeverObservable<T>.Instance;
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item (Sometimes called Just)</para>
        /// 
        /// <para>The Just operator converts an item into an Observable that emits that item.</para>
        /// 
        /// <para>Just is similar to From, but note that From will dive into an array or an iterable 
        /// or something of that sort to pull out items to emit, while Just will simply emit the 
        /// array or iterable or what-have-you as it is, unchanged, as a single item.</para>
        /// 
        /// <para>Note that if you pass null to Just, it will return an Observable that emits null as 
        /// an item.Do not make the mistake of assuming that this will return an empty Observable 
        /// (one that emits no items at all). For that, you will need the Empty operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/just.html"/>
        public static IObservable<T> Return<T>(T value) {
            return Return<T>(value, Scheduler.DefaultSchedulers.ConstantTimeOperations);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item. (Sometimes called Just) Return single sequence on specified scheduler.</para>
        /// 
        /// <para>The Just operator converts an item into an Observable that emits that item.</para>
        /// 
        /// <para>Just is similar to From, but note that From will dive into an array or an iterable 
        /// or something of that sort to pull out items to emit, while Just will simply emit the 
        /// array or iterable or what-have-you as it is, unchanged, as a single item.</para>
        /// 
        /// <para>Note that if you pass null to Just, it will return an Observable that emits null as 
        /// an item.Do not make the mistake of assuming that this will return an empty Observable 
        /// (one that emits no items at all). For that, you will need the Empty operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/just.html"/>
        public static IObservable<T> Return<T>(T value, IScheduler scheduler) {
            if (scheduler == Scheduler.Immediate) {
                return new ImmediateReturnObservable<T>(value);
            } else {
                return new ReturnObservable<T>(value, scheduler);
            }
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item. (Sometimes called Just) Optimized for Unit (Doesn't allocate memory).</para>
        ///
        /// <para>The Just operator converts an item into an Observable that emits that item.</para>
        /// 
        /// <para>Just is similar to From, but note that From will dive into an array or an iterable 
        /// or something of that sort to pull out items to emit, while Just will simply emit the 
        /// array or iterable or what-have-you as it is, unchanged, as a single item.</para>
        /// 
        /// <para>Note that if you pass null to Just, it will return an Observable that emits null as 
        /// an item.Do not make the mistake of assuming that this will return an empty Observable 
        /// (one that emits no items at all). For that, you will need the Empty operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/just.html"/>
        public static IObservable<Unit> Return(Unit value) {
            return ImmutableReturnUnitObservable.Instance;
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item. (Sometimes called Just) Optimized for Boolean (Doesn't allocate memory).</para>
        /// 
        /// <para>The Just operator converts an item into an Observable that emits that item.</para>
        /// 
        /// <para>Just is similar to From, but note that From will dive into an array or an iterable 
        /// or something of that sort to pull out items to emit, while Just will simply emit the 
        /// array or iterable or what-have-you as it is, unchanged, as a single item.</para>
        /// 
        /// <para>Note that if you pass null to Just, it will return an Observable that emits null as 
        /// an item.Do not make the mistake of assuming that this will return an empty Observable 
        /// (one that emits no items at all). For that, you will need the Empty operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/just.html"/>
        public static IObservable<bool> Return(bool value) {
            return (value == true)
                ? (IObservable<bool>) ImmutableReturnTrueObservable.Instance
                : (IObservable<bool>) ImmutableReturnFalseObservable.Instance;
        }

        /// <summary>
        /// <para>Same as Observable.Return(Unit.Default), but doesn't allocate memory.</para>
        /// 
        /// <para>The Just operator converts an item into an Observable that emits that item.</para>
        /// 
        /// <para>Just is similar to From, but note that From will dive into an array or an iterable 
        /// or something of that sort to pull out items to emit, while Just will simply emit the 
        /// array or iterable or what-have-you as it is, unchanged, as a single item.</para>
        /// 
        /// <para>Note that if you pass null to Just, it will return an Observable that emits null as 
        /// an item.Do not make the mistake of assuming that this will return an empty Observable 
        /// (one that emits no items at all). For that, you will need the Empty operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/just.html"/>
        public static IObservable<Unit> ReturnUnit() {
            return ImmutableReturnUnitObservable.Instance;
        }

        /// <summary>
        /// <para>Create an Observable that emits no items and terminates with an error. Returns only onError.</para>
        /// 
        /// <para>The Empty, Never, and Throw operators generate Observables with very specific and 
        /// limited behavior. These are useful for testing purposes, and sometimes also for 
        /// combining with other Observables or as parameters to operators that expect other 
        /// Observables as parameters.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/empty-never-throw.html"/>
        public static IObservable<T> Throw<T>(Exception error) {
            return Throw<T>(error, Scheduler.DefaultSchedulers.ConstantTimeOperations);
        }

        /// <summary>
        /// <para>Create an Observable that emits no items and terminates with an error. Returns only onError. Witness if for Type inference.</para>
        /// 
        /// <para>The Empty, Never, and Throw operators generate Observables with very specific and 
        /// limited behavior. These are useful for testing purposes, and sometimes also for 
        /// combining with other Observables or as parameters to operators that expect other 
        /// Observables as parameters.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/empty-never-throw.html"/>
        public static IObservable<T> Throw<T>(Exception error, T witness) {
            return Throw<T>(error, Scheduler.DefaultSchedulers.ConstantTimeOperations);
        }

        /// <summary>
        /// <para>Create an Observable that emits no items and terminates with an error. Returns only onError. 
        /// Returns only onError on specified scheduler.</para>
        /// 
        /// <para>The Empty, Never, and Throw operators generate Observables with very specific and 
        /// limited behavior. These are useful for testing purposes, and sometimes also for 
        /// combining with other Observables or as parameters to operators that expect other 
        /// Observables as parameters.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/empty-never-throw.html"/>
        public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler) {
            return new ThrowObservable<T>(error, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits no items and terminates with an error. Returns only onError. 
        /// Returns only onError on specified scheduler. Witness if for Type inference.</para>
        /// 
        /// <para>The Empty, Never, and Throw operators generate Observables with very specific and 
        /// limited behavior. These are useful for testing purposes, and sometimes also for 
        /// combining with other Observables or as parameters to operators that expect other 
        /// Observables as parameters.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/empty-never-throw.html"/>
        public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler, T witness) {
            return Throw<T>(error, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular range of sequential integers</para>
        /// 
        /// <para>The Range operator emits a range of sequential integers, in order, where you select the start of the range and its length.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/range.html"/>
        public static IObservable<int> Range(int start, int count) {
            return Range(start, count, Scheduler.DefaultSchedulers.Iteration);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular range of sequential integers</para>
        /// 
        /// <para>The Range operator emits a range of sequential integers, in order, where you select the start of the range and its length.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/range.html"/>
        public static IObservable<int> Range(int start, int count, IScheduler scheduler) {
            return new RangeObservable(start, count, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item multiple times</para>
        /// 
        /// <para>The Repeat operator emits an item repeatedly. Some implementations of this operator allow you to 
        /// repeat a sequence of items, and some permit you to limit the number of repetitions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/repeat.html"/>
        public static IObservable<T> Repeat<T>(T value) {
            return Repeat(value, Scheduler.DefaultSchedulers.Iteration);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item multiple times</para>
        /// 
        /// <para>The Repeat operator emits an item repeatedly. Some implementations of this operator allow you to 
        /// repeat a sequence of items, and some permit you to limit the number of repetitions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/repeat.html"/>
        public static IObservable<T> Repeat<T>(T value, IScheduler scheduler) {
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return new RepeatObservable<T>(value, null, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item multiple times</para>
        /// 
        /// <para>The Repeat operator emits an item repeatedly. Some implementations of this operator allow you to 
        /// repeat a sequence of items, and some permit you to limit the number of repetitions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/repeat.html"/>
        public static IObservable<T> Repeat<T>(T value, int repeatCount) {
            return Repeat(value, repeatCount, Scheduler.DefaultSchedulers.Iteration);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item multiple times</para>
        /// 
        /// <para>The Repeat operator emits an item repeatedly. Some implementations of this operator allow you to 
        /// repeat a sequence of items, and some permit you to limit the number of repetitions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/repeat.html"/>
        public static IObservable<T> Repeat<T>(T value, int repeatCount, IScheduler scheduler) {
            if (repeatCount < 0)
                throw new ArgumentOutOfRangeException("repeatCount");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return new RepeatObservable<T>(value, repeatCount, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item multiple times</para>
        /// 
        /// <para>The Repeat operator emits an item repeatedly. Some implementations of this operator allow you to 
        /// repeat a sequence of items, and some permit you to limit the number of repetitions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/repeat.html"/>
        public static IObservable<T> Repeat<T>(this IObservable<T> source) {
            return RepeatInfinite(source).Concat();
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item multiple times</para>
        /// 
        /// <para>The Repeat operator emits an item repeatedly. Some implementations of this operator allow you to 
        /// repeat a sequence of items, and some permit you to limit the number of repetitions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/repeat.html"/>
        static IEnumerable<IObservable<T>> RepeatInfinite<T>(this IObservable<T> source) {
            while (true) {
                yield return source;
            }
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item multiple times. Stops on OnComplete</para>
        /// 
        /// <para>The Repeat operator emits an item repeatedly. Some implementations of this operator allow you to 
        /// repeat a sequence of items, and some permit you to limit the number of repetitions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/repeat.html"/>
        public static IObservable<T> RepeatSafe<T>(this IObservable<T> source) {
            return new RepeatSafeObservable<T>(RepeatInfinite(source), source.IsRequiredSubscribeOnCurrentThread());
        }

        /// <summary>
        /// <para>Do not create the Observable until the observer subscribes, and create a fresh Observable 
        /// for each observer</para>
        /// 
        /// <para>The Defer operator waits until an observer subscribes to it, and then it generates an 
        /// Observable, typically with an Observable factory function. It does this afresh for each 
        /// subscriber, so although each subscriber may think it is subscribing to the same Observable, 
        /// in fact each subscriber gets its own individual sequence.</para>
        ///
        /// <para>In some circumstances, waiting until the last minute(that is, until subscription time) 
        /// to generate the Observable can ensure that this Observable contains the freshest data.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/defer.html"/>
        public static IObservable<T> Defer<T>(this Func<IObservable<T>> observableFactory) {
            return new DeferObservable<T>(observableFactory);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static IObservable<T> Start<T>(this Func<T> function) {
            return new StartObservable<T>(function, null, Scheduler.DefaultSchedulers.AsyncConversions);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static IObservable<T> Start<T>(this Func<T> function, TimeSpan timeSpan) {
            return new StartObservable<T>(function, timeSpan, Scheduler.DefaultSchedulers.AsyncConversions);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static IObservable<T> Start<T>(this Func<T> function, IScheduler scheduler) {
            return new StartObservable<T>(function, null, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static IObservable<T> Start<T>(this Func<T> function, TimeSpan timeSpan, IScheduler scheduler) {
            return new StartObservable<T>(function, timeSpan, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static IObservable<Unit> Start(this Action action) {
            return new StartObservable<Unit>(action, null, Scheduler.DefaultSchedulers.AsyncConversions);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static IObservable<Unit> Start(this Action action, TimeSpan timeSpan) {
            return new StartObservable<Unit>(action, timeSpan, Scheduler.DefaultSchedulers.AsyncConversions);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static IObservable<Unit> Start(this Action action, IScheduler scheduler) {
            return new StartObservable<Unit>(action, null, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static IObservable<Unit> Start(this Action action, TimeSpan timeSpan, IScheduler scheduler) {
            return new StartObservable<Unit>(action, timeSpan, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive asynchronously</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static Func<IObservable<T>> ToAsync<T>(this Func<T> function) {
            return ToAsync(function, Scheduler.DefaultSchedulers.AsyncConversions);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive asynchronously</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static Func<IObservable<T>> ToAsync<T>(this Func<T> function, IScheduler scheduler) {
            return () => {
                var subject = new AsyncSubject<T>();

                scheduler.Schedule(() => {
                    var result = default(T);
                    try {
                        result = function();
                    } catch (Exception exception) {
                        subject.OnError(exception);
                        return;
                    }
                    subject.OnNext(result);
                    subject.OnCompleted();
                });

                return subject.AsObservable();
            };
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive asynchronously</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static Func<IObservable<Unit>> ToAsync(this Action action) {
            return ToAsync(action, Scheduler.DefaultSchedulers.AsyncConversions);
        }

        /// <summary>
        /// <para>Create an Observable that emits the return value of a function-like directive asynchronously</para>
        /// 
        /// <para>There are a number of ways that programming languages have for obtaining values as 
        /// the result of calculations, with names like functions, futures, actions, callables, 
        /// runnables, and so forth. The operators grouped here under the Start operator category 
        /// make these things behave like Observables so that they can be chained with other 
        /// Observables in an Observable cascade</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/start.html"/>
        public static Func<IObservable<Unit>> ToAsync(this Action action, IScheduler scheduler) {
            return () => {
                var subject = new AsyncSubject<Unit>();

                scheduler.Schedule(() => {
                    try {
                        action();
                    } catch (Exception exception) {
                        subject.OnError(exception);
                        return;
                    }
                    subject.OnNext(Unit.Default);
                    subject.OnCompleted();
                });

                return subject.AsObservable();
            };
        }

    }
}
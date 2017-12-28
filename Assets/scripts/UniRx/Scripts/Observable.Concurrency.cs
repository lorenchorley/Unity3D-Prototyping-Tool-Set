using System;
using System.Collections.Generic;
using System.Text;
using UniRx.Operators;

namespace UniRx {
    public static partial class Observable {

        /// <summary>
        /// <para>Force an Observable to make serialized calls and to be well-behaved</para>
        /// 
        /// <para>It is possible for an Observable to invoke its observers� methods asynchronously, 
        /// perhaps from different threads.This could make such an Observable violate the Observable contract, 
        /// in that it might try to send an OnCompleted or OnError notification before one of its 
        /// OnNext notifications, or it might make an OnNext notification from two different threads 
        /// concurrently.You can force such an Observable to be well-behaved and synchronous by applying 
        /// the Serialize operator to it.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/serialize.html"/>
        public static IObservable<T> Synchronize<T>(this IObservable<T> source) {
            return new SynchronizeObservable<T>(source, new object());
        }

        /// <summary>
        /// <para>Force an Observable to make serialized calls and to be well-behaved</para>
        /// 
        /// <para>It is possible for an Observable to invoke its observers� methods asynchronously, 
        /// perhaps from different threads.This could make such an Observable violate the Observable contract, 
        /// in that it might try to send an OnCompleted or OnError notification before one of its 
        /// OnNext notifications, or it might make an OnNext notification from two different threads 
        /// concurrently.You can force such an Observable to be well-behaved and synchronous by applying 
        /// the Serialize operator to it.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/serialize.html"/>
        public static IObservable<T> Synchronize<T>(this IObservable<T> source, object gate) {
            return new SynchronizeObservable<T>(source, gate);
        }

        /// <summary>
        /// <para>Specify the Scheduler on which an observer will observe this Observable</para>
        /// 
        /// <para>Note that ObserveOn will forward an onError termination notification immediately if it receives one, and will not wait for a slow-consuming observer to receive any not-yet-emitted items that it is aware of first.This may mean that the onError notification jumps ahead of (and swallows) items emitted by the source Observable, as in the diagram above.</para>
        /// 
        /// <para>Many implementations of ReactiveX use �Schedulers� to govern an Observable�s transitions between threads in a multi-threaded environment. You can instruct an Observable to send its notifications to observers on a particular Scheduler by means of the ObserveOn operator.</para>
        /// 
        /// <para>The SubscribeOn operator is similar, but it instructs the Observable to itself operate on the specified Scheduler, as well as notifying its observers on that Scheduler.</para>
        /// 
        /// <para>By default, an Observable and the chain of operators that you apply to it will do its work, and will notify its observers, on the same thread on which its Subscribe method is called.The SubscribeOn operator changes this behavior by specifying a different Scheduler on which the Observable should operate. The ObserveOn operator specifies a different Scheduler that the Observable will use to send notifications to its observers.</para>
        /// 
        /// <para>As shown in this illustration, the SubscribeOn operator designates which thread the Observable will begin operating on, no matter at what point in the chain of operators that operator is called.ObserveOn, on the other hand, affects the thread that the Observable will use below where that operator appears.For this reason, you may call ObserveOn multiple times at various points during the chain of Observable operators in order to change on which threads certain of those operators operate.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/observeon.html"/>
        public static IObservable<T> ObserveOn<T>(this IObservable<T> source, IScheduler scheduler) {
            return new ObserveOnObservable<T>(source, scheduler);
        }

        /// <summary>
        /// <para>Specify the Scheduler on which to subscribe (Variant of ObserveOn)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/observeon.html"/>
        public static IObservable<T> SubscribeOn<T>(this IObservable<T> source, IScheduler scheduler) {
            return new SubscribeOnObservable<T>(source, scheduler);
        }

        /// <summary>
        /// <para>Delay the subscription to the source Observable (Variant of Delay)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/delay.html"/>
        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime) {
            return new DelaySubscriptionObservable<T>(source, dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Delay the subscription to the source Observable (Variant of Delay)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/delay.html"/>
        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler) {
            return new DelaySubscriptionObservable<T>(source, dueTime, scheduler);
        }

        /// <summary>
        /// <para>Delay the subscription to the source Observable (Variant of Delay)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/delay.html"/>
        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime) {
            return new DelaySubscriptionObservable<T>(source, dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Delay the subscription to the source Observable (Variant of Delay)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/delay.html"/>
        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler) {
            return new DelaySubscriptionObservable<T>(source, dueTime, scheduler);
        }

        /// <summary>
        /// <para>Given two or more source Observables, emit all of the items from only the 
        /// first of these Observables to emit an item or notification</para>
        /// 
        /// <para>When you pass a number of source Observables to Amb, it will pass through 
        /// the emissions and notifications of exactly one of these Observables: the first one 
        /// that sends a notification to Amb, either by emitting an item or sending an onError 
        /// or onCompleted notification. Amb will ignore and discard the emissions and notifications 
        /// of all of the other source Observables.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/amb.html"/>
        public static IObservable<T> Amb<T>(params IObservable<T>[] sources) {
            return Amb((IEnumerable<IObservable<T>>) sources);
        }

        /// <summary>
        /// <para>Given two or more source Observables, emit all of the items from only the 
        /// first of these Observables to emit an item or notification</para>
        /// 
        /// <para>When you pass a number of source Observables to Amb, it will pass through 
        /// the emissions and notifications of exactly one of these Observables: the first one 
        /// that sends a notification to Amb, either by emitting an item or sending an onError 
        /// or onCompleted notification. Amb will ignore and discard the emissions and notifications 
        /// of all of the other source Observables.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/amb.html"/>
        public static IObservable<T> Amb<T>(IEnumerable<IObservable<T>> sources) {
            var result = Observable.Never<T>();
            foreach (var item in sources) {
                var second = item;
                result = result.Amb(second);
            }
            return result;
        }

        /// <summary>
        /// <para>Given two or more source Observables, emit all of the items from only the 
        /// first of these Observables to emit an item or notification</para>
        /// 
        /// <para>When you pass a number of source Observables to Amb, it will pass through 
        /// the emissions and notifications of exactly one of these Observables: the first one 
        /// that sends a notification to Amb, either by emitting an item or sending an onError 
        /// or onCompleted notification. Amb will ignore and discard the emissions and notifications 
        /// of all of the other source Observables.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/amb.html"/>
        public static IObservable<T> Amb<T>(this IObservable<T> source, IObservable<T> second) {
            return new AmbObservable<T>(source, second);
        }
    }
}
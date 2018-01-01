using System;
using System.Collections.Generic;
using UniRx.Operators;

namespace UniRx {
    // Timer, Interval, etc...
    public static partial class Observable {

        /// <summary>
        /// <para>Create an Observable that emits a sequence of integers spaced by a given time interval</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Interval operator returns an Observable that emits an infinite sequence of ascending integers,
        /// with a constant interval of time of your choosing between emissions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/interval.html"/>
        public static IObservable<long> Interval(TimeSpan period) {
            return new TimerObservable(period, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Create an Observable that emits a sequence of integers spaced by a given time interval</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Interval operator returns an Observable that emits an infinite sequence of ascending integers,
        /// with a constant interval of time of your choosing between emissions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/interval.html"/>
        public static IObservable<long> Interval(TimeSpan period, IScheduler scheduler) {
            return new TimerObservable(period, period, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item after a given delay</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timer operator creates an Observable that emits one particular item after a span of time that you specify.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timer.html"/>
        public static IObservable<long> Timer(TimeSpan dueTime) {
            return new TimerObservable(dueTime, null, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item after a given delay</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timer operator creates an Observable that emits one particular item after a span of time that you specify.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timer.html"/>
        public static IObservable<long> Timer(DateTimeOffset dueTime) {
            return new TimerObservable(dueTime, null, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item after a given delay</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timer operator creates an Observable that emits one particular item after a span of time that you specify.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timer.html"/>
        public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period) {
            return new TimerObservable(dueTime, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item after a given delay</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timer operator creates an Observable that emits one particular item after a span of time that you specify.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timer.html"/>
        public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period) {
            return new TimerObservable(dueTime, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item after a given delay</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timer operator creates an Observable that emits one particular item after a span of time that you specify.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timer.html"/>
        public static IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler) {
            return new TimerObservable(dueTime, null, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item after a given delay</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timer operator creates an Observable that emits one particular item after a span of time that you specify.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timer.html"/>
        public static IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler) {
            return new TimerObservable(dueTime, null, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item after a given delay</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timer operator creates an Observable that emits one particular item after a span of time that you specify.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timer.html"/>
        public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler) {
            return new TimerObservable(dueTime, period, scheduler);
        }

        /// <summary>
        /// <para>Create an Observable that emits a particular item after a given delay</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timer operator creates an Observable that emits one particular item after a span of time that you specify.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timer.html"/>
        public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler) {
            return new TimerObservable(dueTime, period, scheduler);
        }

        /// <summary>
        /// <para>Attach a timestamp to each item emitted by an Observable indicating when it was emitted</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timestamp operator attaches a timestamp to each item emitted by the source Observable before reemitting that item in its 
        /// own sequence. The timestamp indicates at what time the item was emitted.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timestamp.html"/>
        public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source) {
            return Timestamp<TSource>(source, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Attach a timestamp to each item emitted by an Observable indicating when it was emitted</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timestamp operator attaches a timestamp to each item emitted by the source Observable before reemitting 
        /// that item in its own sequence. The timestamp indicates at what time the item was emitted.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timestamp.html"/>
        public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source, IScheduler scheduler) {
            return new TimestampObservable<TSource>(source, scheduler);
        }

        /// <summary>
        /// <para>Convert an Observable that emits items into one that emits indications of the amount of time elapsed between 
        /// those emissions</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The TimeInterval operator intercepts the items from the source Observable and emits in their place objects that 
        /// indicate the amount of time that elapsed between pairs of emissions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timeinterval.html"/>
        public static IObservable<UniRx.TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source) {
            return TimeInterval(source, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Convert an Observable that emits items into one that emits indications of the amount of time elapsed between 
        /// those emissions</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The TimeInterval operator intercepts the items from the source Observable and emits in their place objects that 
        /// indicate the amount of time that elapsed between pairs of emissions.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timeinterval.html"/>
        public static IObservable<UniRx.TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source, IScheduler scheduler) {
            return new UniRx.Operators.TimeIntervalObservable<TSource>(source, scheduler);
        }

        /// <summary>
        /// <para>Shift the emissions from an Observable forward in time by a particular amount</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Delay operator modifies its source Observable by pausing for a particular increment of time 
        /// (that you specify) before emitting each of the source Observable�s items. This has the effect of shifting
        /// the entire sequence of items emitted by the Observable forward in time by that specified increment.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/delay.html"/>
        public static IObservable<T> Delay<T>(this IObservable<T> source, TimeSpan dueTime) {
            return source.Delay(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Shift the emissions from an Observable forward in time by a particular amount</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Delay operator modifies its source Observable by pausing for a particular increment of time 
        /// (that you specify) before emitting each of the source Observable�s items. This has the effect of shifting
        /// the entire sequence of items emitted by the Observable forward in time by that specified increment.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/delay.html"/>
        public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler) {
            return new DelayObservable<TSource>(source, dueTime, scheduler);
        }

        /// <summary>
        /// <para>Emit the most recent items emitted by an Observable within periodic time intervals</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Sample operator periodically looks at an Observable and emits whichever item it has most
        /// recently emitted since the previous sampling.</para>
        ///
        /// <para>In some implementations, there is also a ThrottleFirst operator that is similar, but emits not
        /// the most-recently emitted item in the sample period, but the first item that was emitted during that
        /// period.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/sample.html"/>
        public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval) {
            return source.Sample(interval, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Emit the most recent items emitted by an Observable within periodic time intervals</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Sample operator periodically looks at an Observable and emits whichever item it has most
        /// recently emitted since the previous sampling.</para>
        ///
        /// <para>In some implementations, there is also a ThrottleFirst operator that is similar, but emits not
        /// the most-recently emitted item in the sample period, but the first item that was emitted during that
        /// period.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/sample.html"/>
        public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval, IScheduler scheduler) {
            return new SampleObservable<T>(source, interval, scheduler);
        }

        /// <summary>
        /// <para>Only emit an item from an Observable if a particular timespan has passed without it emitting 
        /// another item (Sometimes called Debounce)</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Debounce operator filters out items emitted by the source Observable that are rapidly followed 
        /// by another emitted item.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/debounce.html"/>
        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime) {
            return source.Throttle(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Only emit an item from an Observable if a particular timespan has passed without it emitting 
        /// another item (Sometimes called Debounce)</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Debounce operator filters out items emitted by the source Observable that are rapidly followed 
        /// by another emitted item.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/debounce.html"/>
        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler) {
            return new ThrottleObservable<TSource>(source, dueTime, scheduler);
        }

        /// <summary>
        /// <para>Throtter only the first item to be emitted</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/debounce.html"/>
        public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime) {
            return source.ThrottleFirst(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Throtter only the first item to be emitted</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/debounce.html"/>
        public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler) {
            return new ThrottleFirstObservable<TSource>(source, dueTime, scheduler);
        }

        /// <summary>
        /// <para>Mirror the source Observable, but issue an error notification if a particular period of
        /// time elapses without any emitted items</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timeout operator allows you to abort an Observable with an onError termination if that 
        /// Observable fails to emit any items during a specified span of time.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timeout.html"/>
        public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime) {
            return source.Timeout(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Mirror the source Observable, but issue an error notification if a particular period of
        /// time elapses without any emitted items</para>
        /// 
        /// <para>Set of similar extensions: Delay Interval Sample Throttle ThrottleFirst TimeInterval Timeout Timer Timestamp</para>
        /// 
        /// <para>The Timeout operator allows you to abort an Observable with an onError termination if that 
        /// Observable fails to emit any items during a specified span of time.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timeout.html"/>
        public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler) {
            return new TimeoutObservable<T>(source, dueTime, scheduler);
        }

        /// <summary>
        /// <para>Mirror the source Observable, but issue an error notification if a particular period of
        /// time elapses without any emitted items</para>
        /// 
        /// <para>Set of similar extensions: <similarextensions></para>
        /// 
        /// <para>The Timeout operator allows you to abort an Observable with an onError termination if that 
        /// Observable fails to emit any items during a specified span of time.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timeout.html"/>
        public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime) {
            return source.Timeout(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Mirror the source Observable, but issue an error notification if a particular period of
        /// time elapses without any emitted items</para>
        /// 
        /// <para>Set of similar extensions: <similarextensions></para>
        /// 
        /// <para>The Timeout operator allows you to abort an Observable with an onError termination if that 
        /// Observable fails to emit any items during a specified span of time.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/timeout.html"/>
        public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler) {
            return new TimeoutObservable<T>(source, dueTime, scheduler);
        }
    }
}
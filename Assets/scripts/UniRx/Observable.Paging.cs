using System;
using System.Collections.Generic;
using System.Text;
using UniRx.Operators;

namespace UniRx {
    // Take, Skip, etc..
    public static partial class Observable {

        /// <summary>
        /// <para>Emit only the first n items emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>You can emit only the first n items emitted by an Observable and then 
        /// complete while ignoring the remainder, by modifying the Observable with the Take operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/take.html"/>
        public static IObservable<T> Take<T>(this IObservable<T> source, int count) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (count == 0)
                return Empty<T>();

            // optimize .Take(count).Take(count)
            var take = source as TakeObservable<T>;
            if (take != null && take.scheduler == null) {
                return take.Combine(count);
            }

            return new TakeObservable<T>(source, count);
        }

        /// <summary>
        /// <para>Emit only the first n items emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>You can emit only the first n items emitted by an Observable and then 
        /// complete while ignoring the remainder, by modifying the Observable with the Take operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/take.html"/>
        public static IObservable<T> Take<T>(this IObservable<T> source, TimeSpan duration) {
            return Take(source, duration, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Emit only the first n items emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>You can emit only the first n items emitted by an Observable and then 
        /// complete while ignoring the remainder, by modifying the Observable with the Take operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/take.html"/>
        public static IObservable<T> Take<T>(this IObservable<T> source, TimeSpan duration, IScheduler scheduler) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            // optimize .Take(duration).Take(duration)
            var take = source as TakeObservable<T>;
            if (take != null && take.scheduler == scheduler) {
                return take.Combine(duration);
            }

            return new TakeObservable<T>(source, duration, scheduler);
        }

        /// <summary>
        /// <para>Mirror items emitted by an Observable until a specified condition becomes false</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>The TakeWhile mirrors the source Observable until such time as some condition you 
        /// specify becomes false, at which point TakeWhile stops mirroring the source Observable 
        /// and terminates its own Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/takewhile.html"/>
        public static IObservable<T> TakeWhile<T>(this IObservable<T> source, Func<T, bool> predicate) {
            return new TakeWhileObservable<T>(source, predicate);
        }

        /// <summary>
        /// <para>Mirror items emitted by an Observable until a specified condition becomes false</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>The TakeWhile mirrors the source Observable until such time as some condition you 
        /// specify becomes false, at which point TakeWhile stops mirroring the source Observable 
        /// and terminates its own Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/takewhile.html"/>
        public static IObservable<T> TakeWhile<T>(this IObservable<T> source, Func<T, int, bool> predicate) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return new TakeWhileObservable<T>(source, predicate);
        }

        /// <summary>
        /// <para>Mirror items emitted by an Observable until a specified condition becomes false</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>The TakeWhile mirrors the source Observable until such time as some condition you 
        /// specify becomes false, at which point TakeWhile stops mirroring the source Observable 
        /// and terminates its own Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/takeuntil.html"/>
        public static IObservable<T> TakeUntil<T, TOther>(this IObservable<T> source, IObservable<TOther> other) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (other == null)
                throw new ArgumentNullException("other");

            return new TakeUntilObservable<T, TOther>(source, other);
        }

        /// <summary>
        /// <para>Emit only the final n items emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>You can emit only the final n items emitted by an Observable and 
        /// ignore those items that come before them, by modifying the Observable with
        /// the TakeLast operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/takelast.html"/>
        public static IObservable<T> TakeLast<T>(this IObservable<T> source, int count) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return new TakeLastObservable<T>(source, count);
        }

        /// <summary>
        /// <para>Emit only the final n items emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>You can emit only the final n items emitted by an Observable and 
        /// ignore those items that come before them, by modifying the Observable with
        /// the TakeLast operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/takelast.html"/>
        public static IObservable<T> TakeLast<T>(this IObservable<T> source, TimeSpan duration) {
            return TakeLast<T>(source, duration, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Emit only the final n items emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>You can emit only the final n items emitted by an Observable and 
        /// ignore those items that come before them, by modifying the Observable with
        /// the TakeLast operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/takelast.html"/>
        public static IObservable<T> TakeLast<T>(this IObservable<T> source, TimeSpan duration, IScheduler scheduler) {
            if (source == null)
                throw new ArgumentNullException("source");

            return new TakeLastObservable<T>(source, duration, scheduler);
        }

        /// <summary>
        /// <para>Suppress the first n items emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>You can ignore the first n items emitted by an Observable and attend 
        /// only to those items that come after, by modifying the Observable with the 
        /// Skip operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/skip.html"/>
        public static IObservable<T> Skip<T>(this IObservable<T> source, int count) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            // optimize .Skip(count).Skip(count)
            var skip = source as SkipObservable<T>;
            if (skip != null && skip.scheduler == null) {
                return skip.Combine(count);
            }

            return new SkipObservable<T>(source, count);
        }

        /// <summary>
        /// <para>Suppress the first n items emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>You can ignore the first n items emitted by an Observable and attend 
        /// only to those items that come after, by modifying the Observable with the 
        /// Skip operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/skip.html"/>
        public static IObservable<T> Skip<T>(this IObservable<T> source, TimeSpan duration) {
            return Skip(source, duration, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Suppress the first n items emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>You can ignore the first n items emitted by an Observable and attend 
        /// only to those items that come after, by modifying the Observable with the 
        /// Skip operator.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/skip.html"/>
        public static IObservable<T> Skip<T>(this IObservable<T> source, TimeSpan duration, IScheduler scheduler) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            // optimize .Skip(duration).Skip(duration)
            var skip = source as SkipObservable<T>;
            if (skip != null && skip.scheduler == scheduler) {
                return skip.Combine(duration);
            }

            return new SkipObservable<T>(source, duration, scheduler);
        }

        /// <summary>
        /// <para>Discard items emitted by an Observable until a specified condition becomes false</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>The SkipWhile subscribes to the source Observable, but ignores its emissions until 
        /// such time as some condition you specify becomes false, at which point SkipWhile begins to 
        /// mirror the source Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/skipwhile.html"/>
        public static IObservable<T> SkipWhile<T>(this IObservable<T> source, Func<T, bool> predicate) {
            return new SkipWhileObservable<T>(source, predicate);
        }

        /// <summary>
        /// <para>Discard items emitted by an Observable until a specified condition becomes false</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>The SkipWhile subscribes to the source Observable, but ignores its emissions until 
        /// such time as some condition you specify becomes false, at which point SkipWhile begins to 
        /// mirror the source Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/skipwhile.html"/>
        public static IObservable<T> SkipWhile<T>(this IObservable<T> source, Func<T, int, bool> predicate) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return new SkipWhileObservable<T>(source, predicate);
        }

        /// <summary>
        /// <para>Discard items emitted by an Observable until a second Observable emits an item</para>
        /// 
        /// <para>Set of similar extensions: Last Take TakeLast TakeUntil TakeWhile Skip SkipUntil SkipWhile</para>
        /// 
        /// <para>The SkipUntil subscribes to the source Observable, but ignores its emissions until such time as a second Observable emits an item, at which point SkipUntil begins to mirror the source Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/skipuntil.html"/>
        public static IObservable<T> SkipUntil<T, TOther>(this IObservable<T> source, IObservable<TOther> other) {
            return new SkipUntilObservable<T, TOther>(source, other);
        }

        /// <summary>
        /// <para>Periodically gather items emitted by an Observable into bundles and emit these 
        /// bundles rather than emitting the items one at a time</para>
        /// 
        /// <para>The Buffer operator transforms an Observable that emits items into an Observable 
        /// that emits buffered collections of those items. There are a number of variants in the 
        /// various language-specific implementations of Buffer that differ in how they choose 
        /// which items go in which buffers.</para>
        /// 
        /// <para>Note that if the source Observable issues an onError notification, Buffer will
        /// pass on this notification immediately without first emitting the buffer it is in the
        /// process of assembling, even if that buffer contains items that were emitted by the 
        /// source Observable before it issued the error notification.</para>
        /// 
        /// <para>The Window operator is similar to Buffer but collects items into separate Observables
        /// rather than into data structures before reemitting them.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/buffer.html"/>
        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, int count) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count <= 0");

            return new BufferObservable<T>(source, count, 0);
        }

        /// <summary>
        /// <para>Periodically gather items emitted by an Observable into bundles and emit these 
        /// bundles rather than emitting the items one at a time</para>
        /// 
        /// <para>The Buffer operator transforms an Observable that emits items into an Observable 
        /// that emits buffered collections of those items. There are a number of variants in the 
        /// various language-specific implementations of Buffer that differ in how they choose 
        /// which items go in which buffers.</para>
        /// 
        /// <para>Note that if the source Observable issues an onError notification, Buffer will
        /// pass on this notification immediately without first emitting the buffer it is in the
        /// process of assembling, even if that buffer contains items that were emitted by the 
        /// source Observable before it issued the error notification.</para>
        /// 
        /// <para>The Window operator is similar to Buffer but collects items into separate Observables
        /// rather than into data structures before reemitting them.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/buffer.html"/>
        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, int count, int skip) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count <= 0");
            if (skip <= 0)
                throw new ArgumentOutOfRangeException("skip <= 0");

            return new BufferObservable<T>(source, count, skip);
        }

        /// <summary>
        /// <para>Periodically gather items emitted by an Observable into bundles and emit these 
        /// bundles rather than emitting the items one at a time</para>
        /// 
        /// <para>The Buffer operator transforms an Observable that emits items into an Observable 
        /// that emits buffered collections of those items. There are a number of variants in the 
        /// various language-specific implementations of Buffer that differ in how they choose 
        /// which items go in which buffers.</para>
        /// 
        /// <para>Note that if the source Observable issues an onError notification, Buffer will
        /// pass on this notification immediately without first emitting the buffer it is in the
        /// process of assembling, even if that buffer contains items that were emitted by the 
        /// source Observable before it issued the error notification.</para>
        /// 
        /// <para>The Window operator is similar to Buffer but collects items into separate Observables
        /// rather than into data structures before reemitting them.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/buffer.html"/>
        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan) {
            return Buffer(source, timeSpan, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Periodically gather items emitted by an Observable into bundles and emit these 
        /// bundles rather than emitting the items one at a time</para>
        /// 
        /// <para>The Buffer operator transforms an Observable that emits items into an Observable 
        /// that emits buffered collections of those items. There are a number of variants in the 
        /// various language-specific implementations of Buffer that differ in how they choose 
        /// which items go in which buffers.</para>
        /// 
        /// <para>Note that if the source Observable issues an onError notification, Buffer will
        /// pass on this notification immediately without first emitting the buffer it is in the
        /// process of assembling, even if that buffer contains items that were emitted by the 
        /// source Observable before it issued the error notification.</para>
        /// 
        /// <para>The Window operator is similar to Buffer but collects items into separate Observables
        /// rather than into data structures before reemitting them.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/buffer.html"/>
        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, IScheduler scheduler) {
            if (source == null)
                throw new ArgumentNullException("source");

            return new BufferObservable<T>(source, timeSpan, timeSpan, scheduler);
        }

        /// <summary>
        /// <para>Periodically gather items emitted by an Observable into bundles and emit these 
        /// bundles rather than emitting the items one at a time</para>
        /// 
        /// <para>The Buffer operator transforms an Observable that emits items into an Observable 
        /// that emits buffered collections of those items. There are a number of variants in the 
        /// various language-specific implementations of Buffer that differ in how they choose 
        /// which items go in which buffers.</para>
        /// 
        /// <para>Note that if the source Observable issues an onError notification, Buffer will
        /// pass on this notification immediately without first emitting the buffer it is in the
        /// process of assembling, even if that buffer contains items that were emitted by the 
        /// source Observable before it issued the error notification.</para>
        /// 
        /// <para>The Window operator is similar to Buffer but collects items into separate Observables
        /// rather than into data structures before reemitting them.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/buffer.html"/>
        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, int count) {
            return Buffer(source, timeSpan, count, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Periodically gather items emitted by an Observable into bundles and emit these 
        /// bundles rather than emitting the items one at a time</para>
        /// 
        /// <para>The Buffer operator transforms an Observable that emits items into an Observable 
        /// that emits buffered collections of those items. There are a number of variants in the 
        /// various language-specific implementations of Buffer that differ in how they choose 
        /// which items go in which buffers.</para>
        /// 
        /// <para>Note that if the source Observable issues an onError notification, Buffer will
        /// pass on this notification immediately without first emitting the buffer it is in the
        /// process of assembling, even if that buffer contains items that were emitted by the 
        /// source Observable before it issued the error notification.</para>
        /// 
        /// <para>The Window operator is similar to Buffer but collects items into separate Observables
        /// rather than into data structures before reemitting them.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/buffer.html"/>
        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, int count, IScheduler scheduler) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count <= 0");

            return new BufferObservable<T>(source, timeSpan, count, scheduler);
        }

        /// <summary>
        /// <para>Periodically gather items emitted by an Observable into bundles and emit these 
        /// bundles rather than emitting the items one at a time</para>
        /// 
        /// <para>The Buffer operator transforms an Observable that emits items into an Observable 
        /// that emits buffered collections of those items. There are a number of variants in the 
        /// various language-specific implementations of Buffer that differ in how they choose 
        /// which items go in which buffers.</para>
        /// 
        /// <para>Note that if the source Observable issues an onError notification, Buffer will
        /// pass on this notification immediately without first emitting the buffer it is in the
        /// process of assembling, even if that buffer contains items that were emitted by the 
        /// source Observable before it issued the error notification.</para>
        /// 
        /// <para>The Window operator is similar to Buffer but collects items into separate Observables
        /// rather than into data structures before reemitting them.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/buffer.html"/>
        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, TimeSpan timeShift) {
            return new BufferObservable<T>(source, timeSpan, timeShift, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// <para>Periodically gather items emitted by an Observable into bundles and emit these 
        /// bundles rather than emitting the items one at a time</para>
        /// 
        /// <para>The Buffer operator transforms an Observable that emits items into an Observable 
        /// that emits buffered collections of those items. There are a number of variants in the 
        /// various language-specific implementations of Buffer that differ in how they choose 
        /// which items go in which buffers.</para>
        /// 
        /// <para>Note that if the source Observable issues an onError notification, Buffer will
        /// pass on this notification immediately without first emitting the buffer it is in the
        /// process of assembling, even if that buffer contains items that were emitted by the 
        /// source Observable before it issued the error notification.</para>
        /// 
        /// <para>The Window operator is similar to Buffer but collects items into separate Observables
        /// rather than into data structures before reemitting them.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/buffer.html"/>
        public static IObservable<IList<T>> Buffer<T>(this IObservable<T> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler) {
            if (source == null)
                throw new ArgumentNullException("source");

            return new BufferObservable<T>(source, timeSpan, timeShift, scheduler);
        }

        /// <summary>
        /// <para>Periodically gather items emitted by an Observable into bundles and emit these 
        /// bundles rather than emitting the items one at a time</para>
        /// 
        /// <para>The Buffer operator transforms an Observable that emits items into an Observable 
        /// that emits buffered collections of those items. There are a number of variants in the 
        /// various language-specific implementations of Buffer that differ in how they choose 
        /// which items go in which buffers.</para>
        /// 
        /// <para>Note that if the source Observable issues an onError notification, Buffer will
        /// pass on this notification immediately without first emitting the buffer it is in the
        /// process of assembling, even if that buffer contains items that were emitted by the 
        /// source Observable before it issued the error notification.</para>
        /// 
        /// <para>The Window operator is similar to Buffer but collects items into separate Observables
        /// rather than into data structures before reemitting them.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/buffer.html"/>
        public static IObservable<IList<TSource>> Buffer<TSource, TWindowBoundary>(this IObservable<TSource> source, IObservable<TWindowBoundary> windowBoundaries) {
            return new BufferObservable<TSource, TWindowBoundary>(source, windowBoundaries);
        }

        /// <summary>
        /// Emit previous and new element each time a new element is emitted.
        /// </summary>
        public static IObservable<Pair<T>> Pairwise<T>(this IObservable<T> source) {
            return new PairwiseObservable<T>(source);
        }

        /// <summary>
        /// Emit previous and new element each time a new element is emitted.
        /// </summary>
        public static IObservable<TR> Pairwise<T, TR>(this IObservable<T> source, Func<T, T, TR> selector) {
            return new PairwiseObservable<T, TR>(source, selector);
        }

        /// <summary>
        /// <para>Emit only the last item (or the last item that meets some condition) emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// <para>If you are only interested in the last item emitted by an Observable, or the last item that meets 
        /// some criteria, you can filter the Observable with the Last operator.</para>
        /// 
        /// <para>In some implementations, Last is not implemented as a filtering operator that returns an Observable, 
        /// but as a blocking function that returns a particular item when the source Observable terminates.In those
        /// implementations, if you instead want a filtering operator, you may have better luck with TakeLast(1).</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/last.html"/>
        public static IObservable<T> Last<T>(this IObservable<T> source) {
            return new LastObservable<T>(source, false);
        }

        /// <summary>
        /// <para>Emit only the last item (or the last item that meets some condition) emitted by an Observable</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// <para>If you are only interested in the last item emitted by an Observable, or the last item that meets 
        /// some criteria, you can filter the Observable with the Last operator.</para>
        /// 
        /// <para>In some implementations, Last is not implemented as a filtering operator that returns an Observable, 
        /// but as a blocking function that returns a particular item when the source Observable terminates.In those
        /// implementations, if you instead want a filtering operator, you may have better luck with TakeLast(1).</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/last.html"/>
        public static IObservable<T> Last<T>(this IObservable<T> source, Func<T, bool> predicate) {
            return new LastObservable<T>(source, predicate, false);
        }

        /// <summary>
        /// <para>The LastOrDefault operator is similar to last, but you pass it a default item that it can 
        /// emit if the source Observable fails to emit any items.</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/last.html"/>
        public static IObservable<T> LastOrDefault<T>(this IObservable<T> source) {
            return new LastObservable<T>(source, true);
        }

        /// <summary>
        /// <para>The LastOrDefault operator is similar to last, but you pass it a default item that it can 
        /// emit if the source Observable fails to emit any items.</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/last.html"/>
        public static IObservable<T> LastOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate) {
            return new LastObservable<T>(source, predicate, true);
        }

        /// <summary>
        /// <para>Emit only the first item (or the first item that meets some condition) emitted by an Observable.</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// <para>If you are only interested in the first item emitted by an Observable, or the first item that meets 
        /// some criteria, you can filter the Observable with the First operator.</para>
        /// 
        /// <para>In some implementations, First is not implemented as a filtering operator that returns an Observable, 
        /// but as a blocking function that returns a particular item at such time as the source Observable emits that 
        /// item.In those implementations, if you instead want a filtering operator, you may have better luck with Take(1)
        /// or ElementAt(0).</para>
        /// 
        /// <para>In some implementations there is also a Single operator. It behaves similarly to First except that 
        /// it waits until the source Observable terminates in order to guarantee that it only emits a single item 
        /// (otherwise, rather than emitting that item, it terminates with an error). You can use this to not only take 
        /// the first item from the source Observable but to also guarantee that there was only one item.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/first.html"/>
        public static IObservable<T> First<T>(this IObservable<T> source) {
            return new FirstObservable<T>(source, false);
        }

        /// <summary>
        /// <para>Emit only the first item (or the first item that meets some condition) emitted by an Observable.</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// <para>If you are only interested in the first item emitted by an Observable, or the first item that meets 
        /// some criteria, you can filter the Observable with the First operator.</para>
        /// 
        /// <para>In some implementations, First is not implemented as a filtering operator that returns an Observable, 
        /// but as a blocking function that returns a particular item at such time as the source Observable emits that 
        /// item.In those implementations, if you instead want a filtering operator, you may have better luck with Take(1)
        /// or ElementAt(0).</para>
        /// 
        /// <para>In some implementations there is also a Single operator. It behaves similarly to First except that 
        /// it waits until the source Observable terminates in order to guarantee that it only emits a single item 
        /// (otherwise, rather than emitting that item, it terminates with an error). You can use this to not only take 
        /// the first item from the source Observable but to also guarantee that there was only one item.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/first.html"/>
        public static IObservable<T> First<T>(this IObservable<T> source, Func<T, bool> predicate) {
            return new FirstObservable<T>(source, predicate, false);
        }

        /// <summary>
        /// <para>The FirstOrDefault operator is similar to first, but you pass it a default item that it can emit 
        /// if the source Observable fails to emit any items.</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/first.html"/>
        public static IObservable<T> FirstOrDefault<T>(this IObservable<T> source) {
            return new FirstObservable<T>(source, true);
        }

        /// <summary>
        /// <para>The FirstOrDefault operator is similar to first, but you pass it a default item that it can emit 
        /// if the source Observable fails to emit any items.</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/first.html"/>
        public static IObservable<T> FirstOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate) {
            return new FirstObservable<T>(source, predicate, true);
        }

        /// <summary>
        /// <para>The single operator is similar to First, except that it only emits its item once the source
        /// Observable successfully completes after emitting one item (or one item that matches the predicate).
        /// If it emits either no such items or more than one such item, single will terminate with an onError 
        /// notitifcation (�Sequence contains no elements.�).</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/first.html"/>
        public static IObservable<T> Single<T>(this IObservable<T> source) {
            return new SingleObservable<T>(source, false);
        }

        /// <summary>
        /// <para>The single operator is similar to First, except that it only emits its item once the source
        /// Observable successfully completes after emitting one item (or one item that matches the predicate).
        /// If it emits either no such items or more than one such item, single will terminate with an onError 
        /// notitifcation (�Sequence contains no elements.�).</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/first.html"/>
        public static IObservable<T> Single<T>(this IObservable<T> source, Func<T, bool> predicate) {
            return new SingleObservable<T>(source, predicate, false);
        }

        /// <summary>
        /// <para>Emits a default item if the source Observable is empty, although it will still notify of 
        /// an error if the source Observable emits more than one item.</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/first.html"/>
        public static IObservable<T> SingleOrDefault<T>(this IObservable<T> source) {
            return new SingleObservable<T>(source, true);
        }

        /// <summary>
        /// <para>Emits a default item if the source Observable is empty, although it will still notify of 
        /// an error if the source Observable emits more than one item.</para>
        /// 
        /// <para>Set of similar extensions: First Last Single FirstOrDefault LastOrDefault SingleOrDefault</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/first.html"/>
        public static IObservable<T> SingleOrDefault<T>(this IObservable<T> source, Func<T, bool> predicate) {
            return new SingleObservable<T>(source, predicate, true);
        }

        /// <summary>
        /// <para>Divide an Observable into a set of Observables that each emit a different subset of items from 
        /// the original Observable</para>
        /// 
        /// <para>The GroupBy operator divides an Observable that emits items into an Observable that emits Observables, 
        /// each one of which emits some subset of the items from the original source Observable. Which items end 
        /// up on which Observable is typically decided by a discriminating function that evaluates each item and 
        /// assigns it a key. All items with the same key are emitted by the same Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/groupby.html"/>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector) {
            return GroupBy(source, keySelector, Stubs<TSource>.Identity);
        }

        /// <summary>
        /// <para>Divide an Observable into a set of Observables that each emit a different subset of items from 
        /// the original Observable</para>
        /// 
        /// <para>The GroupBy operator divides an Observable that emits items into an Observable that emits Observables, 
        /// each one of which emits some subset of the items from the original source Observable. Which items end 
        /// up on which Observable is typically decided by a discriminating function that evaluates each item and 
        /// assigns it a key. All items with the same key are emitted by the same Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/groupby.html"/>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) {
            return GroupBy(source, keySelector, Stubs<TSource>.Identity, comparer);
        }

        /// <summary>
        /// <para>Divide an Observable into a set of Observables that each emit a different subset of items from 
        /// the original Observable</para>
        /// 
        /// <para>The GroupBy operator divides an Observable that emits items into an Observable that emits Observables, 
        /// each one of which emits some subset of the items from the original source Observable. Which items end 
        /// up on which Observable is typically decided by a discriminating function that evaluates each item and 
        /// assigns it a key. All items with the same key are emitted by the same Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/groupby.html"/>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) {
#if !UniRxLibrary
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
#else
            var comparer = EqualityComparer<TKey>.Default;
#endif

            return GroupBy(source, keySelector, elementSelector, comparer);
        }

        /// <summary>
        /// <para>Divide an Observable into a set of Observables that each emit a different subset of items from 
        /// the original Observable</para>
        /// 
        /// <para>The GroupBy operator divides an Observable that emits items into an Observable that emits Observables, 
        /// each one of which emits some subset of the items from the original source Observable. Which items end 
        /// up on which Observable is typically decided by a discriminating function that evaluates each item and 
        /// assigns it a key. All items with the same key are emitted by the same Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/groupby.html"/>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer) {
            return new GroupByObservable<TSource, TKey, TElement>(source, keySelector, elementSelector, null, comparer);
        }

        /// <summary>
        /// <para>Divide an Observable into a set of Observables that each emit a different subset of items from 
        /// the original Observable</para>
        /// 
        /// <para>The GroupBy operator divides an Observable that emits items into an Observable that emits Observables, 
        /// each one of which emits some subset of the items from the original source Observable. Which items end 
        /// up on which Observable is typically decided by a discriminating function that evaluates each item and 
        /// assigns it a key. All items with the same key are emitted by the same Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/groupby.html"/>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity) {
            return GroupBy(source, keySelector, Stubs<TSource>.Identity, capacity);
        }

        /// <summary>
        /// <para>Divide an Observable into a set of Observables that each emit a different subset of items from 
        /// the original Observable</para>
        /// 
        /// <para>The GroupBy operator divides an Observable that emits items into an Observable that emits Observables, 
        /// each one of which emits some subset of the items from the original source Observable. Which items end 
        /// up on which Observable is typically decided by a discriminating function that evaluates each item and 
        /// assigns it a key. All items with the same key are emitted by the same Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/groupby.html"/>
        public static IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity, IEqualityComparer<TKey> comparer) {
            return GroupBy(source, keySelector, Stubs<TSource>.Identity, capacity, comparer);
        }

        /// <summary>
        /// <para>Divide an Observable into a set of Observables that each emit a different subset of items from 
        /// the original Observable</para>
        /// 
        /// <para>The GroupBy operator divides an Observable that emits items into an Observable that emits Observables, 
        /// each one of which emits some subset of the items from the original source Observable. Which items end 
        /// up on which Observable is typically decided by a discriminating function that evaluates each item and 
        /// assigns it a key. All items with the same key are emitted by the same Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/groupby.html"/>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity) {
#if !UniRxLibrary
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
#else
            var comparer = EqualityComparer<TKey>.Default;
#endif

            return GroupBy(source, keySelector, elementSelector, capacity, comparer);
        }

        /// <summary>
        /// <para>Divide an Observable into a set of Observables that each emit a different subset of items from 
        /// the original Observable</para>
        /// 
        /// <para>The GroupBy operator divides an Observable that emits items into an Observable that emits Observables, 
        /// each one of which emits some subset of the items from the original source Observable. Which items end 
        /// up on which Observable is typically decided by a discriminating function that evaluates each item and 
        /// assigns it a key. All items with the same key are emitted by the same Observable.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/groupby.html"/>
        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity, IEqualityComparer<TKey> comparer) {
            return new GroupByObservable<TSource, TKey, TElement>(source, keySelector, elementSelector, capacity, comparer);
        }
    }
}
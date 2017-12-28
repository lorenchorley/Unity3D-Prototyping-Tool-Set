using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UniRx.Operators;

namespace UniRx {
    // concatenate multiple observables
    // merge, concat, zip...
    public static partial class Observable {

        /// <summary>
        /// <para>Iterate over at least two observables</para>
        /// </summary>
        static IEnumerable<IObservable<T>> CombineSources<T>(IObservable<T> first, params IObservable<T>[] extra) {
            yield return first;
            for (int i = 0; i < extra.Length; i++) {
                yield return extra[i];
            }
        }

        /// <summary>
        /// <para>Emit the emissions from two or more Observables without interleaving them</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Concat operator concatenates the output of multiple Observables so that they act 
        /// like a single Observable, with all of the items emitted by the first Observable being 
        /// emitted before any of the items emitted by the second Observable (and so forth, if there 
        /// are more than two).</para>
        /// 
        /// <para>Concat waits to subscribe to each additional Observable that you pass to it until 
        /// the previous Observable completes.Note that because of this, if you try to concatenate 
        /// a �hot� Observable, that is, one that begins emitting items immediately and before it 
        /// is subscribed to, Concat will not see, and therefore will not emit, any items that Observable 
        /// emits before all previous Observables complete and Concat subscribes to the �hot� Observable.</para>
        /// 
        /// <para>In some ReactiveX implementations there is also a ConcatMap operator (a.k.a.concat_all, 
        /// concat_map, concatMapObserver, for, forIn/for_in, mapcat, selectConcat, or selectConcatObserver) 
        /// that transforms the items emitted by a source Observable into corresponding Observables 
        /// and then concatenates the items emitted by each of these Observables in the order in which 
        /// they are observed and transformed.</para>
        /// 
        /// <para>The StartWith operator is similar to Concat, but prepends, rather than appends, items 
        /// or emissions of items to those emitted by a source Observable.</para>
        /// 
        /// <para>The Merge operator is also similar. It combines the emissions of two or more Observables, 
        /// but may interleave them, whereas Concat never interleaves the emissions from multiple Observables.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/concat.html"/>
        public static IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources) {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return new ConcatObservable<TSource>(sources);
        }

        /// <summary>
        /// <para>Emit the emissions from two or more Observables without interleaving them</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Concat operator concatenates the output of multiple Observables so that they act 
        /// like a single Observable, with all of the items emitted by the first Observable being 
        /// emitted before any of the items emitted by the second Observable (and so forth, if there 
        /// are more than two).</para>
        /// 
        /// <para>Concat waits to subscribe to each additional Observable that you pass to it until 
        /// the previous Observable completes.Note that because of this, if you try to concatenate 
        /// a �hot� Observable, that is, one that begins emitting items immediately and before it 
        /// is subscribed to, Concat will not see, and therefore will not emit, any items that Observable 
        /// emits before all previous Observables complete and Concat subscribes to the �hot� Observable.</para>
        /// 
        /// <para>In some ReactiveX implementations there is also a ConcatMap operator (a.k.a.concat_all, 
        /// concat_map, concatMapObserver, for, forIn/for_in, mapcat, selectConcat, or selectConcatObserver) 
        /// that transforms the items emitted by a source Observable into corresponding Observables 
        /// and then concatenates the items emitted by each of these Observables in the order in which 
        /// they are observed and transformed.</para>
        /// 
        /// <para>The StartWith operator is similar to Concat, but prepends, rather than appends, items 
        /// or emissions of items to those emitted by a source Observable.</para>
        /// 
        /// <para>The Merge operator is also similar. It combines the emissions of two or more Observables, 
        /// but may interleave them, whereas Concat never interleaves the emissions from multiple Observables.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/concat.html"/>
        public static IObservable<TSource> Concat<TSource>(this IEnumerable<IObservable<TSource>> sources) {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return new ConcatObservable<TSource>(sources);
        }

        /// <summary>
        /// <para>Emit the emissions from two or more Observables without interleaving them</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Concat operator concatenates the output of multiple Observables so that they act 
        /// like a single Observable, with all of the items emitted by the first Observable being 
        /// emitted before any of the items emitted by the second Observable (and so forth, if there 
        /// are more than two).</para>
        /// 
        /// <para>Concat waits to subscribe to each additional Observable that you pass to it until 
        /// the previous Observable completes.Note that because of this, if you try to concatenate 
        /// a �hot� Observable, that is, one that begins emitting items immediately and before it 
        /// is subscribed to, Concat will not see, and therefore will not emit, any items that Observable 
        /// emits before all previous Observables complete and Concat subscribes to the �hot� Observable.</para>
        /// 
        /// <para>In some ReactiveX implementations there is also a ConcatMap operator (a.k.a.concat_all, 
        /// concat_map, concatMapObserver, for, forIn/for_in, mapcat, selectConcat, or selectConcatObserver) 
        /// that transforms the items emitted by a source Observable into corresponding Observables 
        /// and then concatenates the items emitted by each of these Observables in the order in which 
        /// they are observed and transformed.</para>
        /// 
        /// <para>The StartWith operator is similar to Concat, but prepends, rather than appends, items 
        /// or emissions of items to those emitted by a source Observable.</para>
        /// 
        /// <para>The Merge operator is also similar. It combines the emissions of two or more Observables, 
        /// but may interleave them, whereas Concat never interleaves the emissions from multiple Observables.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/concat.html"/>
        public static IObservable<TSource> Concat<TSource>(this IObservable<IObservable<TSource>> sources) {
            return sources.Merge(maxConcurrent: 1);
        }

        /// <summary>
        /// <para>Emit the emissions from two or more Observables without interleaving them</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Concat operator concatenates the output of multiple Observables so that they act 
        /// like a single Observable, with all of the items emitted by the first Observable being 
        /// emitted before any of the items emitted by the second Observable (and so forth, if there 
        /// are more than two).</para>
        /// 
        /// <para>Concat waits to subscribe to each additional Observable that you pass to it until 
        /// the previous Observable completes.Note that because of this, if you try to concatenate 
        /// a �hot� Observable, that is, one that begins emitting items immediately and before it 
        /// is subscribed to, Concat will not see, and therefore will not emit, any items that Observable 
        /// emits before all previous Observables complete and Concat subscribes to the �hot� Observable.</para>
        /// 
        /// <para>In some ReactiveX implementations there is also a ConcatMap operator (a.k.a.concat_all, 
        /// concat_map, concatMapObserver, for, forIn/for_in, mapcat, selectConcat, or selectConcatObserver) 
        /// that transforms the items emitted by a source Observable into corresponding Observables 
        /// and then concatenates the items emitted by each of these Observables in the order in which 
        /// they are observed and transformed.</para>
        /// 
        /// <para>The StartWith operator is similar to Concat, but prepends, rather than appends, items 
        /// or emissions of items to those emitted by a source Observable.</para>
        /// 
        /// <para>The Merge operator is also similar. It combines the emissions of two or more Observables, 
        /// but may interleave them, whereas Concat never interleaves the emissions from multiple Observables.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/concat.html"/>
        public static IObservable<TSource> Concat<TSource>(this IObservable<TSource> first, params IObservable<TSource>[] extra) {
            if (first == null)
                throw new ArgumentNullException("first");
            if (extra == null)
                throw new ArgumentNullException("extra");

            var concat = first as ConcatObservable<TSource>;
            if (concat != null) {
                return concat.Combine(extra);
            }

            return Concat(CombineSources(first, extra));
        }

        /// <summary>
        /// <para>Combine multiple Observables into one by merging their emissions</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>You can combine the output of multiple Observables so that they act like 
        /// a single Observable, by using the Merge operator.</para>
        /// 
        /// <para>Merge may interleave the items emitted by the merged Observables(a similar 
        /// operator, Concat, does not interleave items, but emits all of each source Observable�s 
        /// items in turn before beginning to emit items from the next source Observable).</para>
        /// 
        /// <para>As shown in the above diagram, an onError notification from any of the source 
        /// Observables will immediately be passed through to observers and will terminate the 
        /// merged Observable.</para>
        /// 
        /// <para>In many ReactiveX implementations there is a second operator, MergeDelayError, 
        /// that changes this behavior � reserving onError notifications until all of the merged 
        /// Observables complete and only then passing it along to the observers</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/merge.html"/>
        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources) {
            return Merge(sources, Scheduler.DefaultSchedulers.ConstantTimeOperations);
        }

        /// <summary>
        /// <para>Combine multiple Observables into one by merging their emissions</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>You can combine the output of multiple Observables so that they act like 
        /// a single Observable, by using the Merge operator.</para>
        /// 
        /// <para>Merge may interleave the items emitted by the merged Observables(a similar 
        /// operator, Concat, does not interleave items, but emits all of each source Observable�s 
        /// items in turn before beginning to emit items from the next source Observable).</para>
        /// 
        /// <para>As shown in the above diagram, an onError notification from any of the source 
        /// Observables will immediately be passed through to observers and will terminate the 
        /// merged Observable.</para>
        /// 
        /// <para>In many ReactiveX implementations there is a second operator, MergeDelayError, 
        /// that changes this behavior � reserving onError notifications until all of the merged 
        /// Observables complete and only then passing it along to the observers</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/merge.html"/>
        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, IScheduler scheduler) {
            return new MergeObservable<TSource>(sources.ToObservable(scheduler), scheduler == Scheduler.CurrentThread);
        }

        /// <summary>
        /// <para>Combine multiple Observables into one by merging their emissions</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>You can combine the output of multiple Observables so that they act like 
        /// a single Observable, by using the Merge operator.</para>
        /// 
        /// <para>Merge may interleave the items emitted by the merged Observables(a similar 
        /// operator, Concat, does not interleave items, but emits all of each source Observable�s 
        /// items in turn before beginning to emit items from the next source Observable).</para>
        /// 
        /// <para>As shown in the above diagram, an onError notification from any of the source 
        /// Observables will immediately be passed through to observers and will terminate the 
        /// merged Observable.</para>
        /// 
        /// <para>In many ReactiveX implementations there is a second operator, MergeDelayError, 
        /// that changes this behavior � reserving onError notifications until all of the merged 
        /// Observables complete and only then passing it along to the observers</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/merge.html"/>
        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent) {
            return Merge(sources, maxConcurrent, Scheduler.DefaultSchedulers.ConstantTimeOperations);
        }

        /// <summary>
        /// <para>Combine multiple Observables into one by merging their emissions</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>You can combine the output of multiple Observables so that they act like 
        /// a single Observable, by using the Merge operator.</para>
        /// 
        /// <para>Merge may interleave the items emitted by the merged Observables(a similar 
        /// operator, Concat, does not interleave items, but emits all of each source Observable�s 
        /// items in turn before beginning to emit items from the next source Observable).</para>
        /// 
        /// <para>As shown in the above diagram, an onError notification from any of the source 
        /// Observables will immediately be passed through to observers and will terminate the 
        /// merged Observable.</para>
        /// 
        /// <para>In many ReactiveX implementations there is a second operator, MergeDelayError, 
        /// that changes this behavior � reserving onError notifications until all of the merged 
        /// Observables complete and only then passing it along to the observers</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/merge.html"/>
        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources, int maxConcurrent, IScheduler scheduler) {
            return new MergeObservable<TSource>(sources.ToObservable(scheduler), maxConcurrent, scheduler == Scheduler.CurrentThread);
        }

        /// <summary>
        /// <para>Combine multiple Observables into one by merging their emissions</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>You can combine the output of multiple Observables so that they act like 
        /// a single Observable, by using the Merge operator.</para>
        /// 
        /// <para>Merge may interleave the items emitted by the merged Observables(a similar 
        /// operator, Concat, does not interleave items, but emits all of each source Observable�s 
        /// items in turn before beginning to emit items from the next source Observable).</para>
        /// 
        /// <para>As shown in the above diagram, an onError notification from any of the source 
        /// Observables will immediately be passed through to observers and will terminate the 
        /// merged Observable.</para>
        /// 
        /// <para>In many ReactiveX implementations there is a second operator, MergeDelayError, 
        /// that changes this behavior � reserving onError notifications until all of the merged 
        /// Observables complete and only then passing it along to the observers</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/merge.html"/>
        public static IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources) {
            return Merge(Scheduler.DefaultSchedulers.ConstantTimeOperations, sources);
        }

        /// <summary>
        /// <para>Combine multiple Observables into one by merging their emissions</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>You can combine the output of multiple Observables so that they act like 
        /// a single Observable, by using the Merge operator.</para>
        /// 
        /// <para>Merge may interleave the items emitted by the merged Observables(a similar 
        /// operator, Concat, does not interleave items, but emits all of each source Observable�s 
        /// items in turn before beginning to emit items from the next source Observable).</para>
        /// 
        /// <para>As shown in the above diagram, an onError notification from any of the source 
        /// Observables will immediately be passed through to observers and will terminate the 
        /// merged Observable.</para>
        /// 
        /// <para>In many ReactiveX implementations there is a second operator, MergeDelayError, 
        /// that changes this behavior � reserving onError notifications until all of the merged 
        /// Observables complete and only then passing it along to the observers</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/merge.html"/>
        public static IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources) {
            return new MergeObservable<TSource>(sources.ToObservable(scheduler), scheduler == Scheduler.CurrentThread);
        }

        /// <summary>
        /// <para>Combine multiple Observables into one by merging their emissions</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>You can combine the output of multiple Observables so that they act like 
        /// a single Observable, by using the Merge operator.</para>
        /// 
        /// <para>Merge may interleave the items emitted by the merged Observables(a similar 
        /// operator, Concat, does not interleave items, but emits all of each source Observable�s 
        /// items in turn before beginning to emit items from the next source Observable).</para>
        /// 
        /// <para>As shown in the above diagram, an onError notification from any of the source 
        /// Observables will immediately be passed through to observers and will terminate the 
        /// merged Observable.</para>
        /// 
        /// <para>In many ReactiveX implementations there is a second operator, MergeDelayError, 
        /// that changes this behavior � reserving onError notifications until all of the merged 
        /// Observables complete and only then passing it along to the observers</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/merge.html"/>
        public static IObservable<T> Merge<T>(this IObservable<T> first, params IObservable<T>[] seconds) {
            return Merge(CombineSources(first, seconds));
        }

        /// <summary>
        /// <para>Combine multiple Observables into one by merging their emissions</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>You can combine the output of multiple Observables so that they act like 
        /// a single Observable, by using the Merge operator.</para>
        /// 
        /// <para>Merge may interleave the items emitted by the merged Observables(a similar 
        /// operator, Concat, does not interleave items, but emits all of each source Observable�s 
        /// items in turn before beginning to emit items from the next source Observable).</para>
        /// 
        /// <para>As shown in the above diagram, an onError notification from any of the source 
        /// Observables will immediately be passed through to observers and will terminate the 
        /// merged Observable.</para>
        /// 
        /// <para>In many ReactiveX implementations there is a second operator, MergeDelayError, 
        /// that changes this behavior � reserving onError notifications until all of the merged 
        /// Observables complete and only then passing it along to the observers</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/merge.html"/>
        public static IObservable<T> Merge<T>(this IObservable<T> first, IObservable<T> second, IScheduler scheduler) {
            return Merge(scheduler, new[] { first, second });
        }

        /// <summary>
        /// <para>Combine multiple Observables into one by merging their emissions</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>You can combine the output of multiple Observables so that they act like 
        /// a single Observable, by using the Merge operator.</para>
        /// 
        /// <para>Merge may interleave the items emitted by the merged Observables(a similar 
        /// operator, Concat, does not interleave items, but emits all of each source Observable�s 
        /// items in turn before beginning to emit items from the next source Observable).</para>
        /// 
        /// <para>As shown in the above diagram, an onError notification from any of the source 
        /// Observables will immediately be passed through to observers and will terminate the 
        /// merged Observable.</para>
        /// 
        /// <para>In many ReactiveX implementations there is a second operator, MergeDelayError, 
        /// that changes this behavior � reserving onError notifications until all of the merged 
        /// Observables complete and only then passing it along to the observers</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/merge.html"/>
        public static IObservable<T> Merge<T>(this IObservable<IObservable<T>> sources) {
            return new MergeObservable<T>(sources, false);
        }

        /// <summary>
        /// <para>Combine multiple Observables into one by merging their emissions</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>You can combine the output of multiple Observables so that they act like 
        /// a single Observable, by using the Merge operator.</para>
        /// 
        /// <para>Merge may interleave the items emitted by the merged Observables(a similar 
        /// operator, Concat, does not interleave items, but emits all of each source Observable�s 
        /// items in turn before beginning to emit items from the next source Observable).</para>
        /// 
        /// <para>As shown in the above diagram, an onError notification from any of the source 
        /// Observables will immediately be passed through to observers and will terminate the 
        /// merged Observable.</para>
        /// 
        /// <para>In many ReactiveX implementations there is a second operator, MergeDelayError, 
        /// that changes this behavior � reserving onError notifications until all of the merged 
        /// Observables complete and only then passing it along to the observers</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/merge.html"/>
        public static IObservable<T> Merge<T>(this IObservable<IObservable<T>> sources, int maxConcurrent) {
            return new MergeObservable<T>(sources, maxConcurrent, false);
        }

        /// <summary>
        /// <para>Combine the emissions of multiple Observables together via a specified function 
        /// and emit single items for each combination based on the results of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Zip method returns an Observable that applies a function of your choosing to 
        /// the combination of items emitted, in sequence, by two (or more) other Observables, with 
        /// the results of this function becoming the items emitted by the returned Observable. It 
        /// applies this function in strict sequence, so the first item emitted by the new Observable 
        /// will be the result of the function applied to the first item emitted by Observable #1 
        /// and the first item emitted by Observable #2; the second item emitted by the new zip-Observable 
        /// will be the result of the function applied to the second item emitted by Observable #1 
        /// and the second item emitted by Observable #2; and so forth. It will only emit as many 
        /// items as the number of items emitted by the source Observable that emits the fewest items.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/zip.html"/>
        public static IObservable<TResult> Zip<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) {
            return new ZipObservable<TLeft, TRight, TResult>(left, right, selector);
        }

        /// <summary>
        /// <para>Combine the emissions of multiple Observables together via a specified function 
        /// and emit single items for each combination based on the results of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Zip method returns an Observable that applies a function of your choosing to 
        /// the combination of items emitted, in sequence, by two (or more) other Observables, with 
        /// the results of this function becoming the items emitted by the returned Observable. It 
        /// applies this function in strict sequence, so the first item emitted by the new Observable 
        /// will be the result of the function applied to the first item emitted by Observable #1 
        /// and the first item emitted by Observable #2; the second item emitted by the new zip-Observable 
        /// will be the result of the function applied to the second item emitted by Observable #1 
        /// and the second item emitted by Observable #2; and so forth. It will only emit as many 
        /// items as the number of items emitted by the source Observable that emits the fewest items.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/zip.html"/>
        public static IObservable<IList<T>> Zip<T>(this IEnumerable<IObservable<T>> sources) {
            return Zip(sources.ToArray());
        }

        /// <summary>
        /// <para>Combine the emissions of multiple Observables together via a specified function 
        /// and emit single items for each combination based on the results of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Zip method returns an Observable that applies a function of your choosing to 
        /// the combination of items emitted, in sequence, by two (or more) other Observables, with 
        /// the results of this function becoming the items emitted by the returned Observable. It 
        /// applies this function in strict sequence, so the first item emitted by the new Observable 
        /// will be the result of the function applied to the first item emitted by Observable #1 
        /// and the first item emitted by Observable #2; the second item emitted by the new zip-Observable 
        /// will be the result of the function applied to the second item emitted by Observable #1 
        /// and the second item emitted by Observable #2; and so forth. It will only emit as many 
        /// items as the number of items emitted by the source Observable that emits the fewest items.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/zip.html"/>
        public static IObservable<IList<T>> Zip<T>(params IObservable<T>[] sources) {
            return new ZipObservable<T>(sources);
        }

        /// <summary>
        /// <para>Combine the emissions of multiple Observables together via a specified function 
        /// and emit single items for each combination based on the results of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Zip method returns an Observable that applies a function of your choosing to 
        /// the combination of items emitted, in sequence, by two (or more) other Observables, with 
        /// the results of this function becoming the items emitted by the returned Observable. It 
        /// applies this function in strict sequence, so the first item emitted by the new Observable 
        /// will be the result of the function applied to the first item emitted by Observable #1 
        /// and the first item emitted by Observable #2; the second item emitted by the new zip-Observable 
        /// will be the result of the function applied to the second item emitted by Observable #1 
        /// and the second item emitted by Observable #2; and so forth. It will only emit as many 
        /// items as the number of items emitted by the source Observable that emits the fewest items.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/zip.html"/>
        public static IObservable<TR> Zip<T1, T2, T3, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, ZipFunc<T1, T2, T3, TR> resultSelector) {
            return new ZipObservable<T1, T2, T3, TR>(source1, source2, source3, resultSelector);
        }

        /// <summary>
        /// <para>Combine the emissions of multiple Observables together via a specified function 
        /// and emit single items for each combination based on the results of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Zip method returns an Observable that applies a function of your choosing to 
        /// the combination of items emitted, in sequence, by two (or more) other Observables, with 
        /// the results of this function becoming the items emitted by the returned Observable. It 
        /// applies this function in strict sequence, so the first item emitted by the new Observable 
        /// will be the result of the function applied to the first item emitted by Observable #1 
        /// and the first item emitted by Observable #2; the second item emitted by the new zip-Observable 
        /// will be the result of the function applied to the second item emitted by Observable #1 
        /// and the second item emitted by Observable #2; and so forth. It will only emit as many 
        /// items as the number of items emitted by the source Observable that emits the fewest items.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/zip.html"/>
        public static IObservable<TR> Zip<T1, T2, T3, T4, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, ZipFunc<T1, T2, T3, T4, TR> resultSelector) {
            return new ZipObservable<T1, T2, T3, T4, TR>(source1, source2, source3, source4, resultSelector);
        }

        /// <summary>
        /// <para>Combine the emissions of multiple Observables together via a specified function 
        /// and emit single items for each combination based on the results of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Zip method returns an Observable that applies a function of your choosing to 
        /// the combination of items emitted, in sequence, by two (or more) other Observables, with 
        /// the results of this function becoming the items emitted by the returned Observable. It 
        /// applies this function in strict sequence, so the first item emitted by the new Observable 
        /// will be the result of the function applied to the first item emitted by Observable #1 
        /// and the first item emitted by Observable #2; the second item emitted by the new zip-Observable 
        /// will be the result of the function applied to the second item emitted by Observable #1 
        /// and the second item emitted by Observable #2; and so forth. It will only emit as many 
        /// items as the number of items emitted by the source Observable that emits the fewest items.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/zip.html"/>
        public static IObservable<TR> Zip<T1, T2, T3, T4, T5, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, ZipFunc<T1, T2, T3, T4, T5, TR> resultSelector) {
            return new ZipObservable<T1, T2, T3, T4, T5, TR>(source1, source2, source3, source4, source5, resultSelector);
        }

        /// <summary>
        /// <para>Combine the emissions of multiple Observables together via a specified function 
        /// and emit single items for each combination based on the results of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Zip method returns an Observable that applies a function of your choosing to 
        /// the combination of items emitted, in sequence, by two (or more) other Observables, with 
        /// the results of this function becoming the items emitted by the returned Observable. It 
        /// applies this function in strict sequence, so the first item emitted by the new Observable 
        /// will be the result of the function applied to the first item emitted by Observable #1 
        /// and the first item emitted by Observable #2; the second item emitted by the new zip-Observable 
        /// will be the result of the function applied to the second item emitted by Observable #1 
        /// and the second item emitted by Observable #2; and so forth. It will only emit as many 
        /// items as the number of items emitted by the source Observable that emits the fewest items.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/zip.html"/>
        public static IObservable<TR> Zip<T1, T2, T3, T4, T5, T6, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, ZipFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector) {
            return new ZipObservable<T1, T2, T3, T4, T5, T6, TR>(source1, source2, source3, source4, source5, source6, resultSelector);
        }

        /// <summary>
        /// <para>Combine the emissions of multiple Observables together via a specified function 
        /// and emit single items for each combination based on the results of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The Zip method returns an Observable that applies a function of your choosing to 
        /// the combination of items emitted, in sequence, by two (or more) other Observables, with 
        /// the results of this function becoming the items emitted by the returned Observable. It 
        /// applies this function in strict sequence, so the first item emitted by the new Observable 
        /// will be the result of the function applied to the first item emitted by Observable #1 
        /// and the first item emitted by Observable #2; the second item emitted by the new zip-Observable 
        /// will be the result of the function applied to the second item emitted by Observable #1 
        /// and the second item emitted by Observable #2; and so forth. It will only emit as many 
        /// items as the number of items emitted by the source Observable that emits the fewest items.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/zip.html"/>
        public static IObservable<TR> Zip<T1, T2, T3, T4, T5, T6, T7, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, ZipFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector) {
            return new ZipObservable<T1, T2, T3, T4, T5, T6, T7, TR>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
        }

        /// <summary>
        /// <para>When an item is emitted by either of two Observables, combine the latest item 
        /// emitted by each Observable via a specified function and emit items based on the results
        /// of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The CombineLatest operator behaves in a similar way to Zip, but while Zip emits 
        /// items only when each of the zipped source Observables have emitted a previously unzipped 
        /// item, CombineLatest emits an item whenever any of the source Observables emits an item 
        /// (so long as each of the source Observables has emitted at least one item). When any of 
        /// the source Observables emits an item, CombineLatest combines the most recently emitted 
        /// items from each of the other source Observables, using a function you provide, and emits 
        /// the return value from that function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/combinelatest.html"/>
        public static IObservable<TResult> CombineLatest<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) {
            return new CombineLatestObservable<TLeft, TRight, TResult>(left, right, selector);
        }

        /// <summary>
        /// <para>When an item is emitted by either of two Observables, combine the latest item 
        /// emitted by each Observable via a specified function and emit items based on the results
        /// of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The CombineLatest operator behaves in a similar way to Zip, but while Zip emits 
        /// items only when each of the zipped source Observables have emitted a previously unzipped 
        /// item, CombineLatest emits an item whenever any of the source Observables emits an item 
        /// (so long as each of the source Observables has emitted at least one item). When any of 
        /// the source Observables emits an item, CombineLatest combines the most recently emitted 
        /// items from each of the other source Observables, using a function you provide, and emits 
        /// the return value from that function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/combinelatest.html"/>
        public static IObservable<IList<T>> CombineLatest<T>(this IEnumerable<IObservable<T>> sources) {
            return CombineLatest(sources.ToArray());
        }

        /// <summary>
        /// <para>When an item is emitted by either of two Observables, combine the latest item 
        /// emitted by each Observable via a specified function and emit items based on the results
        /// of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The CombineLatest operator behaves in a similar way to Zip, but while Zip emits 
        /// items only when each of the zipped source Observables have emitted a previously unzipped 
        /// item, CombineLatest emits an item whenever any of the source Observables emits an item 
        /// (so long as each of the source Observables has emitted at least one item). When any of 
        /// the source Observables emits an item, CombineLatest combines the most recently emitted 
        /// items from each of the other source Observables, using a function you provide, and emits 
        /// the return value from that function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/combinelatest.html"/>
        public static IObservable<IList<TSource>> CombineLatest<TSource>(params IObservable<TSource>[] sources) {
            return new CombineLatestObservable<TSource>(sources);
        }

        /// <summary>
        /// <para>When an item is emitted by either of two Observables, combine the latest item 
        /// emitted by each Observable via a specified function and emit items based on the results
        /// of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The CombineLatest operator behaves in a similar way to Zip, but while Zip emits 
        /// items only when each of the zipped source Observables have emitted a previously unzipped 
        /// item, CombineLatest emits an item whenever any of the source Observables emits an item 
        /// (so long as each of the source Observables has emitted at least one item). When any of 
        /// the source Observables emits an item, CombineLatest combines the most recently emitted 
        /// items from each of the other source Observables, using a function you provide, and emits 
        /// the return value from that function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/combinelatest.html"/>
        public static IObservable<TR> CombineLatest<T1, T2, T3, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, CombineLatestFunc<T1, T2, T3, TR> resultSelector) {
            return new CombineLatestObservable<T1, T2, T3, TR>(source1, source2, source3, resultSelector);
        }

        /// <summary>
        /// <para>When an item is emitted by either of two Observables, combine the latest item 
        /// emitted by each Observable via a specified function and emit items based on the results
        /// of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The CombineLatest operator behaves in a similar way to Zip, but while Zip emits 
        /// items only when each of the zipped source Observables have emitted a previously unzipped 
        /// item, CombineLatest emits an item whenever any of the source Observables emits an item 
        /// (so long as each of the source Observables has emitted at least one item). When any of 
        /// the source Observables emits an item, CombineLatest combines the most recently emitted 
        /// items from each of the other source Observables, using a function you provide, and emits 
        /// the return value from that function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/combinelatest.html"/>
        public static IObservable<TR> CombineLatest<T1, T2, T3, T4, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, CombineLatestFunc<T1, T2, T3, T4, TR> resultSelector) {
            return new CombineLatestObservable<T1, T2, T3, T4, TR>(source1, source2, source3, source4, resultSelector);
        }

        /// <summary>
        /// <para>When an item is emitted by either of two Observables, combine the latest item 
        /// emitted by each Observable via a specified function and emit items based on the results
        /// of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The CombineLatest operator behaves in a similar way to Zip, but while Zip emits 
        /// items only when each of the zipped source Observables have emitted a previously unzipped 
        /// item, CombineLatest emits an item whenever any of the source Observables emits an item 
        /// (so long as each of the source Observables has emitted at least one item). When any of 
        /// the source Observables emits an item, CombineLatest combines the most recently emitted 
        /// items from each of the other source Observables, using a function you provide, and emits 
        /// the return value from that function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/combinelatest.html"/>
        public static IObservable<TR> CombineLatest<T1, T2, T3, T4, T5, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, CombineLatestFunc<T1, T2, T3, T4, T5, TR> resultSelector) {
            return new CombineLatestObservable<T1, T2, T3, T4, T5, TR>(source1, source2, source3, source4, source5, resultSelector);
        }

        /// <summary>
        /// <para>When an item is emitted by either of two Observables, combine the latest item 
        /// emitted by each Observable via a specified function and emit items based on the results
        /// of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The CombineLatest operator behaves in a similar way to Zip, but while Zip emits 
        /// items only when each of the zipped source Observables have emitted a previously unzipped 
        /// item, CombineLatest emits an item whenever any of the source Observables emits an item 
        /// (so long as each of the source Observables has emitted at least one item). When any of 
        /// the source Observables emits an item, CombineLatest combines the most recently emitted 
        /// items from each of the other source Observables, using a function you provide, and emits 
        /// the return value from that function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/combinelatest.html"/>
        public static IObservable<TR> CombineLatest<T1, T2, T3, T4, T5, T6, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, CombineLatestFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector) {
            return new CombineLatestObservable<T1, T2, T3, T4, T5, T6, TR>(source1, source2, source3, source4, source5, source6, resultSelector);
        }

        /// <summary>
        /// <para>When an item is emitted by either of two Observables, combine the latest item 
        /// emitted by each Observable via a specified function and emit items based on the results
        /// of this function</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>The CombineLatest operator behaves in a similar way to Zip, but while Zip emits 
        /// items only when each of the zipped source Observables have emitted a previously unzipped 
        /// item, CombineLatest emits an item whenever any of the source Observables emits an item 
        /// (so long as each of the source Observables has emitted at least one item). When any of 
        /// the source Observables emits an item, CombineLatest combines the most recently emitted 
        /// items from each of the other source Observables, using a function you provide, and emits 
        /// the return value from that function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/combinelatest.html"/>
        public static IObservable<TR> CombineLatest<T1, T2, T3, T4, T5, T6, T7, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, CombineLatestFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector) {
            return new CombineLatestObservable<T1, T2, T3, T4, T5, T6, T7, TR>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
        }

        /// <summary>
        /// <para>Zips the latest values from multiple sources and calls a combiner function for them.If one of the sources is faster then the others, its unconsumed values will be overwritten by newer values.Unlike combineLatest, source items are participating in the combination at most once; i.e., the operator emits only if all sources have produced an item.</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>See remarks for diagram, since this extension is not well documented</para>
        /// </summary>
        /// <remarks>
        /// Zip
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----b2--c3--------d4--e5----
        /// 
        /// CombineLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1b1--c2--d3--e3--f3f4--f5--g5
        /// 
        /// ZipLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----c2--d3--------f4------g5
        /// </remarks>
        public static IObservable<TResult> ZipLatest<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) {
            return new ZipLatestObservable<TLeft, TRight, TResult>(left, right, selector);
        }

        /// <summary>
        /// <para>Zips the latest values from multiple sources and calls a combiner function for them.If one of the sources is faster then the others, its unconsumed values will be overwritten by newer values.Unlike combineLatest, source items are participating in the combination at most once; i.e., the operator emits only if all sources have produced an item.</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>See remarks for diagram, since this extension is not well documented</para>
        /// </summary>
        /// <remarks>
        /// Zip
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----b2--c3--------d4--e5----
        /// 
        /// CombineLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1b1--c2--d3--e3--f3f4--f5--g5
        /// 
        /// ZipLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----c2--d3--------f4------g5
        /// </remarks>
        public static IObservable<IList<T>> ZipLatest<T>(this IEnumerable<IObservable<T>> sources) {
            return ZipLatest(sources.ToArray());
        }

        /// <summary>
        /// <para>Zips the latest values from multiple sources and calls a combiner function for them.If one of the sources is faster then the others, its unconsumed values will be overwritten by newer values.Unlike combineLatest, source items are participating in the combination at most once; i.e., the operator emits only if all sources have produced an item.</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>See remarks for diagram, since this extension is not well documented</para>
        /// </summary>
        /// <remarks>
        /// Zip
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----b2--c3--------d4--e5----
        /// 
        /// CombineLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1b1--c2--d3--e3--f3f4--f5--g5
        /// 
        /// ZipLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----c2--d3--------f4------g5
        /// </remarks>
        public static IObservable<IList<TSource>> ZipLatest<TSource>(params IObservable<TSource>[] sources) {
            return new ZipLatestObservable<TSource>(sources);
        }

        /// <summary>
        /// <para>Zips the latest values from multiple sources and calls a combiner function for them.If one of the sources is faster then the others, its unconsumed values will be overwritten by newer values.Unlike combineLatest, source items are participating in the combination at most once; i.e., the operator emits only if all sources have produced an item.</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>See remarks for diagram, since this extension is not well documented</para>
        /// </summary>
        /// <remarks>
        /// Zip
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----b2--c3--------d4--e5----
        /// 
        /// CombineLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1b1--c2--d3--e3--f3f4--f5--g5
        /// 
        /// ZipLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----c2--d3--------f4------g5
        /// </remarks>
        public static IObservable<TR> ZipLatest<T1, T2, T3, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, ZipLatestFunc<T1, T2, T3, TR> resultSelector) {
            return new ZipLatestObservable<T1, T2, T3, TR>(source1, source2, source3, resultSelector);
        }

        /// <summary>
        /// <para>Zips the latest values from multiple sources and calls a combiner function for them.If one of the sources is faster then the others, its unconsumed values will be overwritten by newer values.Unlike combineLatest, source items are participating in the combination at most once; i.e., the operator emits only if all sources have produced an item.</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>See remarks for diagram, since this extension is not well documented</para>
        /// </summary>
        /// <remarks>
        /// Zip
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----b2--c3--------d4--e5----
        /// 
        /// CombineLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1b1--c2--d3--e3--f3f4--f5--g5
        /// 
        /// ZipLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----c2--d3--------f4------g5
        /// </remarks>
        public static IObservable<TR> ZipLatest<T1, T2, T3, T4, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, ZipLatestFunc<T1, T2, T3, T4, TR> resultSelector) {
            return new ZipLatestObservable<T1, T2, T3, T4, TR>(source1, source2, source3, source4, resultSelector);
        }

        /// <summary>
        /// <para>Zips the latest values from multiple sources and calls a combiner function for them.If one of the sources is faster then the others, its unconsumed values will be overwritten by newer values.Unlike combineLatest, source items are participating in the combination at most once; i.e., the operator emits only if all sources have produced an item.</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>See remarks for diagram, since this extension is not well documented</para>
        /// </summary>
        /// <remarks>
        /// Zip
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----b2--c3--------d4--e5----
        /// 
        /// CombineLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1b1--c2--d3--e3--f3f4--f5--g5
        /// 
        /// ZipLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----c2--d3--------f4------g5
        /// </remarks>
        public static IObservable<TR> ZipLatest<T1, T2, T3, T4, T5, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, ZipLatestFunc<T1, T2, T3, T4, T5, TR> resultSelector) {
            return new ZipLatestObservable<T1, T2, T3, T4, T5, TR>(source1, source2, source3, source4, source5, resultSelector);
        }

        /// <summary>
        /// <para>Zips the latest values from multiple sources and calls a combiner function for them.If one of the sources is faster then the others, its unconsumed values will be overwritten by newer values.Unlike combineLatest, source items are participating in the combination at most once; i.e., the operator emits only if all sources have produced an item.</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>See remarks for diagram, since this extension is not well documented</para>
        /// </summary>
        /// <remarks>
        /// Zip
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----b2--c3--------d4--e5----
        /// 
        /// CombineLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1b1--c2--d3--e3--f3f4--f5--g5
        /// 
        /// ZipLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----c2--d3--------f4------g5
        /// </remarks>
        public static IObservable<TR> ZipLatest<T1, T2, T3, T4, T5, T6, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, ZipLatestFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector) {
            return new ZipLatestObservable<T1, T2, T3, T4, T5, T6, TR>(source1, source2, source3, source4, source5, source6, resultSelector);
        }

        /// <summary>
        /// <para>Zips the latest values from multiple sources and calls a combiner function for them.If one of the sources is faster then the others, its unconsumed values will be overwritten by newer values.Unlike combineLatest, source items are participating in the combination at most once; i.e., the operator emits only if all sources have produced an item.</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>See remarks for diagram, since this extension is not well documented</para>
        /// </summary>
        /// <remarks>
        /// Zip
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----b2--c3--------d4--e5----
        /// 
        /// CombineLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1b1--c2--d3--e3--f3f4--f5--g5
        /// 
        /// ZipLatest
        /// aa--bb--cc--dd--ee--ff--------gg
        /// --11----22--33--------44--55----
        /// ================================
        /// --a1----c2--d3--------f4------g5
        /// </remarks>
        public static IObservable<TR> ZipLatest<T1, T2, T3, T4, T5, T6, T7, TR>(this IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, ZipLatestFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector) {
            return new ZipLatestObservable<T1, T2, T3, T4, T5, T6, T7, TR>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
        }

        /// <summary>
        /// <para>Convert an Observable that emits Observables into a single Observable 
        /// that emits the items emitted by the most-recently-emitted of those Observables</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>Switch subscribes to an Observable that emits Observables. Each time it 
        /// observes one of these emitted Observables, the Observable returned by Switch 
        /// unsubscribes from the previously-emitted Observable begins emitting items from 
        /// the latest Observable. Note that it will unsubscribe from the previously-emitted 
        /// Observable when a new Observable is emitted from the source Observable, not when 
        /// the new Observable emits an item. This means that items emitted by the previous 
        /// Observable between the time the subsequent Observable is emitted and the time 
        /// that subsequent Observable itself begins emitting items will be dropped (as with 
        /// the yellow circle in the diagram above).</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/switch.html"/>
        public static IObservable<T> Switch<T>(this IObservable<IObservable<T>> sources) {
            return new SwitchObservable<T>(sources);
        }

        /// <summary>
        /// <para>Similar to CombineLatest, but only emits items when the single source 
        /// Observable emits an item (not when any of the Observables that are passed to 
        /// the operator do, as CombineLatest does).</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/combinelatest.html"/>
        public static IObservable<TResult> WithLatestFrom<TLeft, TRight, TResult>(this IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) {
            return new WithLatestFromObservable<TLeft, TRight, TResult>(left, right, selector);
        }

        /// <summary>
        /// <para>UniRx specific: Specialized for single async operations like Task.WhenAll, Zip.Take(1).</para>
        /// <para>If sequence is empty, return T[0] array.</para>
        /// </summary>
        public static IObservable<T[]> WhenAll<T>(params IObservable<T>[] sources) {
            if (sources.Length == 0)
                return Observable.Return(new T[0]);

            return new WhenAllObservable<T>(sources);
        }

        /// <summary>
        /// <para>UniRx specific: Specialized for single async operations like Task.WhenAll, Zip.Take(1).</para>
        /// </summary>
        public static IObservable<Unit> WhenAll(params IObservable<Unit>[] sources) {
            if (sources.Length == 0)
                return Observable.ReturnUnit();

            return new WhenAllObservable(sources);
        }

        /// <summary>
        /// <para>UniRx specific: Specialized for single async operations like Task.WhenAll, Zip.Take(1).</para>
        /// <para>If sequence is empty, return T[0] array.</para>
        /// </summary>
        public static IObservable<T[]> WhenAll<T>(this IEnumerable<IObservable<T>> sources) {
            var array = sources as IObservable<T>[];
            if (array != null)
                return WhenAll(array);

            return new WhenAllObservable<T>(sources);
        }

        /// <summary>
        /// <para>UniRx specific: Specialized for single async operations like Task.WhenAll, Zip.Take(1).</para>
        /// </summary>
        public static IObservable<Unit> WhenAll(this IEnumerable<IObservable<Unit>> sources) {
            var array = sources as IObservable<Unit>[];
            if (array != null)
                return WhenAll(array);

            return new WhenAllObservable(sources);
        }

        /// <summary>
        /// <para>Emit a specified sequence of items before beginning to emit the items from the source Observable</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>If you want an Observable to emit a specific sequence of items before it begins emitting the 
        /// items normally expected from it, apply the StartWith operator to it.</para>
        /// 
        /// <para>(If, on the other hand, you want to append a sequence of items to the end of those normally
        /// emitted by an Observable, you want the Concat operator.)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/startwith.html"/>
        public static IObservable<T> StartWith<T>(this IObservable<T> source, T value) {
            return new StartWithObservable<T>(source, value);
        }

        /// <summary>
        /// <para>Emit a specified sequence of items before beginning to emit the items from the source Observable</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>If you want an Observable to emit a specific sequence of items before it begins emitting the 
        /// items normally expected from it, apply the StartWith operator to it.</para>
        /// 
        /// <para>(If, on the other hand, you want to append a sequence of items to the end of those normally
        /// emitted by an Observable, you want the Concat operator.)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/startwith.html"/>
        public static IObservable<T> StartWith<T>(this IObservable<T> source, Func<T> valueFactory) {
            return new StartWithObservable<T>(source, valueFactory);
        }

        /// <summary>
        /// <para>Emit a specified sequence of items before beginning to emit the items from the source Observable</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>If you want an Observable to emit a specific sequence of items before it begins emitting the 
        /// items normally expected from it, apply the StartWith operator to it.</para>
        /// 
        /// <para>(If, on the other hand, you want to append a sequence of items to the end of those normally
        /// emitted by an Observable, you want the Concat operator.)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/startwith.html"/>
        public static IObservable<T> StartWith<T>(this IObservable<T> source, params T[] values) {
            return StartWith(source, Scheduler.DefaultSchedulers.ConstantTimeOperations, values);
        }

        /// <summary>
        /// <para>Emit a specified sequence of items before beginning to emit the items from the source Observable</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>If you want an Observable to emit a specific sequence of items before it begins emitting the 
        /// items normally expected from it, apply the StartWith operator to it.</para>
        /// 
        /// <para>(If, on the other hand, you want to append a sequence of items to the end of those normally
        /// emitted by an Observable, you want the Concat operator.)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/startwith.html"/>
        public static IObservable<T> StartWith<T>(this IObservable<T> source, IEnumerable<T> values) {
            return StartWith(source, Scheduler.DefaultSchedulers.ConstantTimeOperations, values);
        }

        /// <summary>
        /// <para>Emit a specified sequence of items before beginning to emit the items from the source Observable</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>If you want an Observable to emit a specific sequence of items before it begins emitting the 
        /// items normally expected from it, apply the StartWith operator to it.</para>
        /// 
        /// <para>(If, on the other hand, you want to append a sequence of items to the end of those normally
        /// emitted by an Observable, you want the Concat operator.)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/startwith.html"/>
        public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, T value) {
            return Observable.Return(value, scheduler).Concat(source);
        }

        /// <summary>
        /// <para>Emit a specified sequence of items before beginning to emit the items from the source Observable</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>If you want an Observable to emit a specific sequence of items before it begins emitting the 
        /// items normally expected from it, apply the StartWith operator to it.</para>
        /// 
        /// <para>(If, on the other hand, you want to append a sequence of items to the end of those normally
        /// emitted by an Observable, you want the Concat operator.)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/startwith.html"/>
        public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, IEnumerable<T> values) {
            var array = values as T[];
            if (array == null) {
                array = values.ToArray();
            }

            return StartWith(source, scheduler, array);
        }

        /// <summary>
        /// <para>Emit a specified sequence of items before beginning to emit the items from the source Observable</para>
        /// 
        /// <para>Set of similar extensions: StartWith CombineLatest Concat Merge Switch WithLatestFrom Zip ZipLatest</para>
        /// 
        /// <para>If you want an Observable to emit a specific sequence of items before it begins emitting the 
        /// items normally expected from it, apply the StartWith operator to it.</para>
        /// 
        /// <para>(If, on the other hand, you want to append a sequence of items to the end of those normally
        /// emitted by an Observable, you want the Concat operator.)</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/startwith.html"/>
        public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, params T[] values) {
            return values.ToObservable(scheduler).Concat(source);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using UniRx.Operators;

namespace UniRx {
    public static partial class Observable {

        /// <summary>
        /// <para>Apply a function to each item emitted by an Observable, sequentially, and emit
        /// each successive value</para>
        /// 
        /// <para>Set of similar extensions: Aggregate Scan Select</para>
        /// 
        /// <para>The Scan operator applies a function to the first item emitted by the source 
        /// Observable and then emits the result of that function as its own first emission. It 
        /// also feeds the result of the function back into the function along with the second item 
        /// emitted by the source Observable in order to generate its second emission. It continues 
        /// to feed back its own subsequent emissions along with the subsequent emissions from the 
        /// source Observable in order to create the rest of its sequence.</para>
        /// 
        /// <para>This sort of operator is sometimes called an �accumulator� in other contexts.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/scan.html"/>
        public static IObservable<TSource> Scan<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator) {
            return new ScanObservable<TSource>(source, accumulator);
        }

        /// <summary>
        /// <para>Apply a function to each item emitted by an Observable, sequentially, and emit
        /// each successive value</para>
        /// 
        /// <para>Set of similar extensions: Aggregate Scan Select</para>
        /// 
        /// <para>The Scan operator applies a function to the first item emitted by the source 
        /// Observable and then emits the result of that function as its own first emission. It 
        /// also feeds the result of the function back into the function along with the second item 
        /// emitted by the source Observable in order to generate its second emission. It continues 
        /// to feed back its own subsequent emissions along with the subsequent emissions from the 
        /// source Observable in order to create the rest of its sequence.</para>
        /// 
        /// <para>This sort of operator is sometimes called an �accumulator� in other contexts.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/scan.html"/>
        public static IObservable<TAccumulate> Scan<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator) {
            return new ScanObservable<TSource, TAccumulate>(source, seed, accumulator);
        }

        /// <summary>
        /// <para>Apply a function to each item emitted by an Observable, sequentially, and emit the final value (Sometimes called Reduce)</para>
        /// 
        /// <para>Set of similar extensions: Aggregate Scan Select</para>
        /// 
        /// <para>The Reduce operator applies a function to the first item emitted by the source 
        /// Observable and then feeds the result of the function back into the function along with 
        /// the second item emitted by the source Observable, continuing this process until the 
        /// source Observable emits its final item and completes, whereupon the Observable returned 
        /// from Reduce emits the final value returned from the function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/reduce.html"/>
        public static IObservable<TSource> Aggregate<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator) {
            return new AggregateObservable<TSource>(source, accumulator);
        }

        /// <summary>
        /// <para>Apply a function to each item emitted by an Observable, sequentially, and emit the final value (Sometimes called Reduce)</para>
        /// 
        /// <para>Set of similar extensions: Aggregate Scan Select</para>
        /// 
        /// <para>The Reduce operator applies a function to the first item emitted by the source 
        /// Observable and then feeds the result of the function back into the function along with 
        /// the second item emitted by the source Observable, continuing this process until the 
        /// source Observable emits its final item and completes, whereupon the Observable returned 
        /// from Reduce emits the final value returned from the function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/reduce.html"/>
        public static IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator) {
            return new AggregateObservable<TSource, TAccumulate>(source, seed, accumulator);
        }

        /// <summary>
        /// <para>Apply a function to each item emitted by an Observable, sequentially, and emit the final value (Sometimes called Reduce)</para>
        /// 
        /// <para>Set of similar extensions: Aggregate Scan Select</para>
        /// 
        /// <para>The Reduce operator applies a function to the first item emitted by the source 
        /// Observable and then feeds the result of the function back into the function along with 
        /// the second item emitted by the source Observable, continuing this process until the 
        /// source Observable emits its final item and completes, whereupon the Observable returned 
        /// from Reduce emits the final value returned from the function.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/reduce.html"/>
        public static IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector) {
            return new AggregateObservable<TSource, TAccumulate, TResult>(source, seed, accumulator, resultSelector);
        }

        /// <summary>
        /// <para>Counts the number of items emitted by the observable. Derived from Aggregate.</para>
        /// 
        /// <para>Set of similar extensions: Count Sum Min Max</para>
        /// 
        /// <see cref="http://www.introtorx.com/Content/v1.0.10621.0/07_Aggregation.html#Aggregate"/>
        public static IObservable<int> Count<T>(this IObservable<T> source) {
            return source.Aggregate(0, (acc, _) => acc + 1);
        }

        /// <summary>
        /// <para>Sum the items emitted by the observable. Derived from Aggregate.</para>
        /// 
        /// <para>Set of similar extensions: Count Sum Min Max</para>
        /// 
        /// <see cref="http://www.introtorx.com/Content/v1.0.10621.0/07_Aggregation.html#Aggregate"/>
        public static IObservable<int> Sum(this IObservable<int> source) {
            return source.Aggregate(0, (acc, currentValue) => acc + currentValue);
        }

        /// <summary>
        /// <para>Calculates the minimum of the items emitted by the observable. Derived from Aggregate.</para>
        /// 
        /// <para>Set of similar extensions: Count Sum Min Max</para>
        /// 
        /// <see cref="http://www.introtorx.com/Content/v1.0.10621.0/07_Aggregation.html#Aggregate"/>
        public static IObservable<T> Min<T>(this IObservable<T> source) {
            Func<T, T, int> compare = Comparer<T>.Default.Compare;
            return source.Aggregate(
                (min, current) => compare(min, current) > 0 ? current : min
                );
        }

        /// <summary>
        /// <para>Calculates the maximum of the items emitted by the observable. Derived from Aggregate.</para>
        /// 
        /// <para>Set of similar extensions: Count Sum Min Max</para>
        /// 
        /// <see cref="http://www.introtorx.com/Content/v1.0.10621.0/07_Aggregation.html#Aggregate"/>
        public static IObservable<T> Max<T>(this IObservable<T> source) {
            var comparer = Comparer<T>.Default;
            return source.Aggregate((x, y) => {
                if (comparer.Compare(x, y) < 0) {
                    return y;
                }
                return x;
            });
        }

        /// <summary>
        /// <para>Emits true and completes on completion if all items emitted satisfy the predicate, or false and 
        /// completes immediately if the predicate fails on any item</para>
        /// 
        /// <para>All implementation using Where, Select and FirstOrDefault</para>
        /// </summary>
        public static IObservable<bool> All<T>(this IObservable<T> source, Func<T, int, bool> predicate) {
            return source
                  .Where((t, i) => !predicate(t, i)) // Only keep the items that don't satisfy the predicate
                  .Select(t => true) // Transform these failing items into trues
                  .FirstOrDefault() // Take the first true that arrives, emit it, and complete. Or if complete arrives before any items, return a default item (false since we're on an IObservable<bool> now)
                  .Select(t => !t); // Invert the value to get the correct result
        }

        /// <summary>
        /// <para>Emits false and completes on completion if no items emitted satisfy the predicate, or true and 
        /// completes immediately if the predicate matches on any item</para>
        /// 
        /// <para>All implementation using Where, Select and FirstOrDefault</para>
        /// </summary>
        public static IObservable<bool> Any<T>(this IObservable<T> source, Func<T, int, bool> predicate) {
            return source
                  .Where(predicate) // Only keep the items that satisfy the predicate
                  .Select(t => true)
                  .FirstOrDefault(); // Take the first true that arrives, emit it, and complete. Or if complete arrives before any items, return a default item (false since we're on an IObservable<bool> now)
        }

        /// <summary>
        /// <para>Emits true and completes on completion if all items emitted satisfy predicate, or false and 
        /// completes imediately if the predicate fails on any item</para>
        /// 
        /// <para>All implementation using Where, Select and FirstOrDefault</para>
        /// </summary>
        public static IObservable<bool> Contains<T>(this IObservable<T> source, T item) {
            var comparer = Comparer<T>.Default;
            return source.Any((t, i) => comparer.Compare(t, item) >= 0);
        }

        /// <summary>
        /// <para>Emits only item nth emitted by an Observable</para>
        /// 
        /// <para>Throws an IndexOutOfRangeException if the index is not encountered before compeletion</para>
        /// </summary>
        public static IObservable<T> ElementAt<T>(this IObservable<T> source, int n) {
            bool returnedSomething = false;
            return source
                    .Where((t, i) => i == n)
                    .Do(t => returnedSomething = true)
                    .DoOnCompleted(() => {
                        if (!returnedSomething)
                            throw new IndexOutOfRangeException();
                    });
        }

        /// <summary>
        /// <para>Emits false on completion if no items were emitted, or true and then completes if at least one
        /// item is emitted</para>
        /// 
        /// <para>All implementation using Where, Select and FirstOrDefault</para>
        /// </summary>
        public static IObservable<bool> IsEmpty<T>(this IObservable<T> source) {
            return source
                    .Select(t => true)
                    .FirstOrDefault()
                    .Select(t => !t);
        }

    }
}

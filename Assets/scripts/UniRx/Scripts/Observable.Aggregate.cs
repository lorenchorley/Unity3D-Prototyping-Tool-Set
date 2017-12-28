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
    }
}

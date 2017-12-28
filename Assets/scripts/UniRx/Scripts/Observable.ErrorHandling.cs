using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Operators;

namespace UniRx {
    public static partial class Observable {

        /// <summary>
        /// The finallyDo operator registers an Action which will be called just after the resulting Observable terminates, 
        /// whether normally or with an error. (Similar to DoOnTerminate, which is called just before the observable terminates)
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/do.html"/>
        public static IObservable<T> Finally<T>(this IObservable<T> source, Action finallyAction) {
            return new FinallyObservable<T>(source, finallyAction);
        }

        /// <summary>
        /// <para>Recover from an onError notification by continuing the sequence without error</para>
        /// 
        /// <para>The Catch operator intercepts an onError notification from the source Observable and, 
        /// instead of passing it through to any observers, replaces it with some other item or sequence 
        /// of items, potentially allowing the resulting Observable to terminate normally or not to terminate 
        /// at all.</para>
        /// 
        /// <para>There are several variants of the Catch operator, and a variety of names used by different 
        /// ReactiveX implementations to describe this operation, as you can see in the sections below.</para>
        /// 
        /// <para>In some ReactiveX implementations, there is an operator called something like �OnErrorResumeNext� 
        /// that behaves like a Catch variant: specifically reacting to an onError notification from the source 
        /// Observable. In others, there is an operator with that name that behaves more like a Concat variant: 
        /// performing the concatenation operation regardless of whether the source Observable terminates normally 
        /// or with an error. This is unfortunate and confusing, but something we have to live with.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/catch.html"/>
        public static IObservable<T> Catch<T, TException>(this IObservable<T> source, Func<TException, IObservable<T>> errorHandler)
            where TException : Exception {
            return new CatchObservable<T, TException>(source, errorHandler);
        }

        /// <summary>
        /// <para>Recover from an onError notification by continuing the sequence without error</para>
        /// 
        /// <para>The Catch operator intercepts an onError notification from the source Observable and, 
        /// instead of passing it through to any observers, replaces it with some other item or sequence 
        /// of items, potentially allowing the resulting Observable to terminate normally or not to terminate 
        /// at all.</para>
        /// 
        /// <para>There are several variants of the Catch operator, and a variety of names used by different 
        /// ReactiveX implementations to describe this operation, as you can see in the sections below.</para>
        /// 
        /// <para>In some ReactiveX implementations, there is an operator called something like �OnErrorResumeNext� 
        /// that behaves like a Catch variant: specifically reacting to an onError notification from the source 
        /// Observable. In others, there is an operator with that name that behaves more like a Concat variant: 
        /// performing the concatenation operation regardless of whether the source Observable terminates normally 
        /// or with an error. This is unfortunate and confusing, but something we have to live with.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/catch.html"/>
        public static IObservable<TSource> Catch<TSource>(this IEnumerable<IObservable<TSource>> sources) {
            return new CatchObservable<TSource>(sources);
        }

        /// <summary>Catch exception and return Observable.Empty.</summary>
        public static IObservable<TSource> CatchIgnore<TSource>(this IObservable<TSource> source) {
            return source.Catch<TSource, Exception>(Stubs.CatchIgnore<TSource>);
        }

        /// <summary>Catch exception and return Observable.Empty.</summary>
        public static IObservable<TSource> CatchIgnore<TSource, TException>(this IObservable<TSource> source, Action<TException> errorAction)
            where TException : Exception {
            var result = source.Catch((TException ex) => {
                errorAction(ex);
                return Observable.Empty<TSource>();
            });
            return result;
        }

        /// <summary>
        /// <para>If a source Observable emits an error, resubscribe to it in the hopes that it will complete without error</para>
        /// 
        /// <para>The Retry operator responds to an onError notification from the source Observable by not passing
        /// that call through to its observers, but instead by resubscribing to the source Observable and giving 
        /// it another opportunity to complete its sequence without error. Retry always passes onNext notifications 
        /// through to its observers, even from sequences that terminate with an error, so this can cause duplicate 
        /// emissions (as shown in the diagram above).</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/retry.html"/>
        public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source) {
            return RepeatInfinite(source).Catch();
        }

        /// <summary>
        /// <para>If a source Observable emits an error, resubscribe to it in the hopes that it will complete without error</para>
        /// 
        /// <para>The Retry operator responds to an onError notification from the source Observable by not passing
        /// that call through to its observers, but instead by resubscribing to the source Observable and giving 
        /// it another opportunity to complete its sequence without error. Retry always passes onNext notifications 
        /// through to its observers, even from sequences that terminate with an error, so this can cause duplicate 
        /// emissions (as shown in the diagram above).</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/retry.html"/>
        public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source, int retryCount) {
            return System.Linq.Enumerable.Repeat(source, retryCount).Catch();
        }

        /// <summary>
        /// <para>Repeats the source observable sequence until it successfully terminates.</para>
        /// <para>This is same as Retry().</para>
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource>(
            this IObservable<TSource> source) {
            var result = source.Retry();
            return result;
        }

        /// <summary>
        /// When exception is caught, do onError action and repeat observable sequence.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            this IObservable<TSource> source, Action<TException> onError)
            where TException : Exception {
            return source.OnErrorRetry(onError, TimeSpan.Zero);
        }

        /// <summary>
        /// When exception is caught, do onError action and repeat observable sequence after delay time.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            this IObservable<TSource> source, Action<TException> onError, TimeSpan delay)
            where TException : Exception {
            return source.OnErrorRetry(onError, int.MaxValue, delay);
        }

        /// <summary>
        /// When exception is caught, do onError action and repeat observable sequence during within retryCount.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            this IObservable<TSource> source, Action<TException> onError, int retryCount)
            where TException : Exception {
            return source.OnErrorRetry(onError, retryCount, TimeSpan.Zero);
        }

        /// <summary>
        /// When exception is caught, do onError action and repeat observable sequence after delay time during within retryCount.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            this IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay)
            where TException : Exception {
            return source.OnErrorRetry(onError, retryCount, delay, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// When exception is caught, do onError action and repeat observable sequence after delay time(work on delayScheduler) during within retryCount.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            this IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay, IScheduler delayScheduler)
            where TException : Exception {
            var result = Observable.Defer(() => {
                var dueTime = (delay.Ticks < 0) ? TimeSpan.Zero : delay;
                var count = 0;

                IObservable<TSource> self = null;
                self = source.Catch((TException ex) => {
                    onError(ex);

                    return (++count < retryCount)
                        ? (dueTime == TimeSpan.Zero)
                            ? self.SubscribeOn(Scheduler.CurrentThread)
                            : self.DelaySubscription(dueTime, delayScheduler).SubscribeOn(Scheduler.CurrentThread)
                        : Observable.Throw<TSource>(ex);
                });
                return self;
            });

            return result;
        }
    }
}
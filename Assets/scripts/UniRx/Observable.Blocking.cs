using System;

namespace UniRx {
    public static partial class Observable {

        /// <summary>
        /// <para>Waits (and blocks) on the current thread until System.Threading.WaitHandle receives a signal</para>
        /// </summary>
        public static T Wait<T>(this IObservable<T> source) {
            return new UniRx.Operators.Wait<T>(source, InfiniteTimeSpan).Run();
        }

        /// <summary>
        /// <para>Waits (and blocks) on the current thread until System.Threading.WaitHandle receives a signal or the timeout occurs</para>
        /// </summary>
        public static T Wait<T>(this IObservable<T> source, TimeSpan timeout) {
            return new UniRx.Operators.Wait<T>(source, timeout).Run();
        }
    }
}

using System;
using UniRx.Operators;

namespace UniRx {
    public static partial class Observable {

        /// <summary>
        /// <para>The multicast operator which operates on an ordinary Observable, multicasts 
        /// that Observable by means of a particular Subject that you specify, applies a 
        /// transformative function to each emission, and then emits those transformed values 
        /// as its own ordinary Observable sequence. Each subscription to this new Observable 
        /// will trigger a new subscription to the underlying multicast Observable. (Similar to Publish)</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/publish.html"/>
        public static IConnectableObservable<T> Multicast<T>(this IObservable<T> source, ISubject<T> subject) {
            return new ConnectableObservable<T>(source, subject);
        }

        /// <summary>
        /// <para>Convert an ordinary Observable into a connectable Observable</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// <para>A connectable Observable resembles an ordinary Observable, except 
        /// that it does not begin emitting items when it is subscribed to, but only 
        /// when the Connect operator is applied to it. In this way you can prompt an 
        /// Observable to begin emitting items at a time of your choosing.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/publish.html"/>
        public static IConnectableObservable<T> Publish<T>(this IObservable<T> source) {
            return source.Multicast(new Subject<T>());
        }

        /// <summary>
        /// <para>Convert an ordinary Observable into a connectable Observable</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// <para>A connectable Observable resembles an ordinary Observable, except 
        /// that it does not begin emitting items when it is subscribed to, but only 
        /// when the Connect operator is applied to it. In this way you can prompt an 
        /// Observable to begin emitting items at a time of your choosing.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/publish.html"/>
        public static IConnectableObservable<T> Publish<T>(this IObservable<T> source, T initialValue) {
            return source.Multicast(new BehaviorSubject<T>(initialValue));
        }

        /// <summary>
        /// <para>The publishLast operator is similar to publish, and takes a 
        /// similarly-behaving function as its parameter.It differs from publish in 
        /// that instead of applying that function to, and emitting an item for every 
        /// item emitted by the source Observable subsequent to the connection, it 
        /// only applies that function to and emits an item for the last item that 
        /// was emitted by the source Observable, when that source Observable terminates 
        /// normally.</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/publish.html"/>
        public static IConnectableObservable<T> PublishLast<T>(this IObservable<T> source) {
            return source.Multicast(new AsyncSubject<T>());
        }

        /// <summary>
        /// <para>Ensure that all observers see the same sequence of emitted items, 
        /// even if they subscribe after the Observable has begun emitting items</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// <para>A connectable Observable resembles an ordinary Observable, except that 
        /// it does not begin emitting items when it is subscribed to, but only when the 
        /// Connect operator is applied to it. In this way you can prompt an Observable 
        /// to begin emitting items at a time of your choosing.</para>
        /// 
        /// <para>If you apply the Replay operator to an Observable before you convert it 
        /// into a connectable Observable, the resulting connectable Observable will always 
        /// emit the same complete sequence to any future observers, even those observers that 
        /// subscribe after the connectable Observable has begun to emit items to other 
        /// subscribed observers.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/replay.html"/>
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source) {
            return source.Multicast(new ReplaySubject<T>());
        }

        /// <summary>
        /// <para>Ensure that all observers see the same sequence of emitted items, 
        /// even if they subscribe after the Observable has begun emitting items</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// <para>A connectable Observable resembles an ordinary Observable, except that 
        /// it does not begin emitting items when it is subscribed to, but only when the 
        /// Connect operator is applied to it. In this way you can prompt an Observable 
        /// to begin emitting items at a time of your choosing.</para>
        /// 
        /// <para>If you apply the Replay operator to an Observable before you convert it 
        /// into a connectable Observable, the resulting connectable Observable will always 
        /// emit the same complete sequence to any future observers, even those observers that 
        /// subscribe after the connectable Observable has begun to emit items to other 
        /// subscribed observers.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/replay.html"/>
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, IScheduler scheduler) {
            return source.Multicast(new ReplaySubject<T>(scheduler));
        }

        /// <summary>
        /// <para>Ensure that all observers see the same sequence of emitted items, 
        /// even if they subscribe after the Observable has begun emitting items</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// <para>A connectable Observable resembles an ordinary Observable, except that 
        /// it does not begin emitting items when it is subscribed to, but only when the 
        /// Connect operator is applied to it. In this way you can prompt an Observable 
        /// to begin emitting items at a time of your choosing.</para>
        /// 
        /// <para>If you apply the Replay operator to an Observable before you convert it 
        /// into a connectable Observable, the resulting connectable Observable will always 
        /// emit the same complete sequence to any future observers, even those observers that 
        /// subscribe after the connectable Observable has begun to emit items to other 
        /// subscribed observers.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/replay.html"/>
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize) {
            return source.Multicast(new ReplaySubject<T>(bufferSize));
        }

        /// <summary>
        /// <para>Ensure that all observers see the same sequence of emitted items, 
        /// even if they subscribe after the Observable has begun emitting items</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// <para>A connectable Observable resembles an ordinary Observable, except that 
        /// it does not begin emitting items when it is subscribed to, but only when the 
        /// Connect operator is applied to it. In this way you can prompt an Observable 
        /// to begin emitting items at a time of your choosing.</para>
        /// 
        /// <para>If you apply the Replay operator to an Observable before you convert it 
        /// into a connectable Observable, the resulting connectable Observable will always 
        /// emit the same complete sequence to any future observers, even those observers that 
        /// subscribe after the connectable Observable has begun to emit items to other 
        /// subscribed observers.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/replay.html"/>
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize, IScheduler scheduler) {
            return source.Multicast(new ReplaySubject<T>(bufferSize, scheduler));
        }

        /// <summary>
        /// <para>Ensure that all observers see the same sequence of emitted items, 
        /// even if they subscribe after the Observable has begun emitting items</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// <para>A connectable Observable resembles an ordinary Observable, except that 
        /// it does not begin emitting items when it is subscribed to, but only when the 
        /// Connect operator is applied to it. In this way you can prompt an Observable 
        /// to begin emitting items at a time of your choosing.</para>
        /// 
        /// <para>If you apply the Replay operator to an Observable before you convert it 
        /// into a connectable Observable, the resulting connectable Observable will always 
        /// emit the same complete sequence to any future observers, even those observers that 
        /// subscribe after the connectable Observable has begun to emit items to other 
        /// subscribed observers.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/replay.html"/>
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, TimeSpan window) {
            return source.Multicast(new ReplaySubject<T>(window));
        }

        /// <summary>
        /// <para>Ensure that all observers see the same sequence of emitted items, 
        /// even if they subscribe after the Observable has begun emitting items</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// <para>A connectable Observable resembles an ordinary Observable, except that 
        /// it does not begin emitting items when it is subscribed to, but only when the 
        /// Connect operator is applied to it. In this way you can prompt an Observable 
        /// to begin emitting items at a time of your choosing.</para>
        /// 
        /// <para>If you apply the Replay operator to an Observable before you convert it 
        /// into a connectable Observable, the resulting connectable Observable will always 
        /// emit the same complete sequence to any future observers, even those observers that 
        /// subscribe after the connectable Observable has begun to emit items to other 
        /// subscribed observers.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/replay.html"/>
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, TimeSpan window, IScheduler scheduler) {
            return source.Multicast(new ReplaySubject<T>(window, scheduler));
        }

        /// <summary>
        /// <para>Ensure that all observers see the same sequence of emitted items, 
        /// even if they subscribe after the Observable has begun emitting items</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// <para>A connectable Observable resembles an ordinary Observable, except that 
        /// it does not begin emitting items when it is subscribed to, but only when the 
        /// Connect operator is applied to it. In this way you can prompt an Observable 
        /// to begin emitting items at a time of your choosing.</para>
        /// 
        /// <para>If you apply the Replay operator to an Observable before you convert it 
        /// into a connectable Observable, the resulting connectable Observable will always 
        /// emit the same complete sequence to any future observers, even those observers that 
        /// subscribe after the connectable Observable has begun to emit items to other 
        /// subscribed observers.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/replay.html"/>
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize, TimeSpan window, IScheduler scheduler) {
            return source.Multicast(new ReplaySubject<T>(bufferSize, window, scheduler));
        }

        /// <summary>
        /// <para>Make a Connectable Observable behave like an ordinary Observable</para>
        /// 
        /// <para>Set of similar extensions: Multicast Publish PublishLast Replay RefCount Share</para>
        /// 
        /// <para>A connectable Observable resembles an ordinary Observable, except that 
        /// it does not begin emitting items when it is subscribed to, but only when the 
        /// Connect operator is applied to it. In this way you can prompt an Observable 
        /// to begin emitting items at a time of your choosing.</para>
        /// 
        /// <para>The RefCount operator automates the process of connecting to and disconnecting 
        /// from a connectable Observable.It operates on a connectable Observable and 
        /// returns an ordinary Observable. When the first observer subscribes to this 
        /// Observable, RefCount connects to the underlying connectable Observable.RefCount 
        /// then keeps track of how many other observers subscribe to it and does not disconnect 
        /// from the underlying connectable Observable until the last observer has done so.</para>
        /// </summary>
        /// <see cref="http://reactivex.io/documentation/operators/refcount.html"/>
        public static IObservable<T> RefCount<T>(this IConnectableObservable<T> source) {
            return new RefCountObservable<T>(source);
        }

        /// <summary>
        /// Equivalent to Publish().RefCount()
        /// </summary>
        public static IObservable<T> Share<T>(this IObservable<T> source) {
            return source.Publish().RefCount();
        }
    }
}
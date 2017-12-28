using System;
using System.Collections.Generic;
using UniRx.Operators;
using UnityEngine;

namespace UniRx {

    public static partial class Observable {

        /// <summary>
        /// <para>Debug function that print to the Unity console all values it receives</para>
        /// </summary>
        public static IObservable<T> Dump<T>(this IObservable<T> source, string Name) {
            return source.Do(o => Debug.Log(Name + " --> " + o.ToString()));
        }

    }

}
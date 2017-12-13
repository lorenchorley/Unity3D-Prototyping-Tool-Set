#if !(UNITY_METRO || UNITY_WP8)

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniRx.Examples
{

    public static class UnityEngineExtensions {

        public static IObservable<float> ToObservable(this UnityEngine.AsyncOperation asyncOperation) {
            if (asyncOperation == null)
                throw new ArgumentNullException("asyncOperation");

            return Observable.FromCoroutine<float>((observer, cancellationToken) => RunAsyncOperation(asyncOperation, observer, cancellationToken));
        }

        static IEnumerator RunAsyncOperation(UnityEngine.AsyncOperation asyncOperation, IObserver<float> observer, CancellationToken cancellationToken) {
            while (!asyncOperation.isDone && !cancellationToken.IsCancellationRequested) {
                observer.OnNext(asyncOperation.progress);
                yield return null;
            }
            if (!cancellationToken.IsCancellationRequested) {
                observer.OnNext(asyncOperation.progress); // push 100%
                observer.OnCompleted();
            }
        }

    }
    
    public class Sample14_LoadLevel : MonoBehaviour
    {
         
        public string LevelName;
         
        void Start()
        {
            SceneManager.LoadSceneAsync(LevelName)
                .ToObservable()
                .Do(x => Debug.Log("Loading: " + x.ToString("0.0000") + " at " + DateTime.Now.Second + "." + DateTime.Now.Millisecond)) // output progress
                .Last() // last sequence is load completed
                .Subscribe();
        }

    }
}

#endif
using UniRx;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace photon.essynchronisation {

    public class PingSimulator : MonoBehaviour {

        public float Average;
        public float Variation;

        public void Delay(Action callback) {
            if (enabled)
                StartCoroutine(Wait(callback));
            else
                callback.Invoke();
        }

        private IEnumerator Wait(Action callback) {
            float msToWait = Average + UnityEngine.Random.Range(-Variation, Variation);
            yield return new WaitForSecondsRealtime(msToWait / 1000);
            callback.Invoke();
        }

    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ValueListTest : MonoBehaviour {

    public int Quantity = 100000;

    struct CustomValue {
        public int I;
        public bool B;
        public float F;

        public override string ToString() {
            return B ? I.ToString() : F.ToString();
        }
    }

    private void Start() {
        //EnumerableTest();
        SpeedTest();
    }

    private void EnumerableTest() {

        ValueList<CustomValue> a = new ValueList<CustomValue>();
        a.Add(new CustomValue() { B = true, F = 2.1f, I = 3 });
        a.Add(new CustomValue() { B = false, F = 2.2f, I = 2 });
        a.Add(new CustomValue() { B = true, F = 2.3f, I = 1 });

        IRefEnumerator<CustomValue> e = a.GetEnumerator();
        CustomValue v = default(CustomValue);
        e.Reset();
        while (e.MoveNext(ref v)) {
            Debug.Log(v.ToString());
        }
        e.Dispose();
        e = null;

    }

    private long TimeFunction(Action a) {
        DateTime start = DateTime.Now;
        a.Invoke();
        return DateTime.Now.Subtract(start).Ticks / TimeSpan.TicksPerMillisecond;
    }

    private void SpeedTest() {
        Debug.Log("Value List:");

        ValueList<int> a = new ValueList<int>();
        Debug.Log("Add " + Quantity + " elements: " + TimeFunction(() => {
            for (int i = 0; i < Quantity; i++) {
                a.Add(i);
            }
        }) + "ms");

        Debug.Log("Enumerate and sum " + Quantity + " elements: " + TimeFunction(() => {
            IRefEnumerator<int> e = a.GetEnumerator();
            int v = 0, sum = 0;
            e.Reset();
            while (e.MoveNext(ref v)) {
                sum += v;
            }
            e.Dispose();
            Debug.Log("Sum: " + sum);
        }) + "ms");

        Debug.Log("Array:");

        int[] b = new int[Quantity];
        Debug.Log("Add " + Quantity + " elements: " + TimeFunction(() => {
            for (int i = 0; i < Quantity; i++) {
                b[i] = i;
            }
        }) + "ms");

        Debug.Log("Enumerate and sum " + Quantity + " elements: " + TimeFunction(() => {
            IEnumerator e = b.GetEnumerator();
            int sum = 0;
            e.Reset();
            while (e.MoveNext()) {
                sum += (int) e.Current;
            }
            Debug.Log("Sum: " + sum);
        }) + "ms");

        Debug.Log("Array without enumerator:");

        b = new int[Quantity];
        Debug.Log("Add " + Quantity + " elements: " + TimeFunction(() => {
            for (int i = 0; i < Quantity; i++) {
                b[i] = i;
            }
        }) + "ms");

        Debug.Log("For and sum " + Quantity + " elements: " + TimeFunction(() => {
            int sum = 0;
            for (int i = 0; i < Quantity; i++) {
                sum += b[i];
            }
            Debug.Log("Sum: " + sum);
        }) + "ms");

        Debug.Log("List:");

        List<int> c = new List<int>();
        Debug.Log("Add " + Quantity + " elements: " + TimeFunction(() => {
            for (int i = 0; i < Quantity; i++) {
                c.Add(i);
            }
        }) + "ms");

        Debug.Log("Enumerate and sum " + Quantity + " elements: " + TimeFunction(() => {
            IEnumerator<int> e = c.GetEnumerator();
            int sum = 0;
            e.Reset();
            while (e.MoveNext()) {
                sum += e.Current;
            }
            e.Dispose();
            Debug.Log("Sum: " + sum);
        }) + "ms");

    }

}


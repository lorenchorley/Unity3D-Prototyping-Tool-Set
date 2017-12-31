using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace eventsourcing {

    public class CustomStackTest : MonoBehaviour {

        private void Start() {

            CustomStack<int> a = new CustomStack<int>(1);
            a.Push(1);
            a.Push(2);
            a.Push(3);
            a.Push(4);
            Assert.AreEqual(a.Peek(), 4);

            foreach (int i in a) {
                Debug.Log("i: " + i);
            }

            Assert.AreEqual(a.Pop(), 4);
            Assert.AreEqual(a.Pop(), 3);
            Assert.AreEqual(a.Pop(), 2);
            Assert.AreEqual(a.Pop(), 1);
            try {
                a.Pop();
            } catch {
                Debug.Log("Popping empty stack returned error as expected");
            }

        }

    }
    
}
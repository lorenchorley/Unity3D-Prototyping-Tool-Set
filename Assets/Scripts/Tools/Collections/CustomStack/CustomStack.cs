using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// Like Stack, but enumerator reads from bottom to top
public class CustomStack<T> : ICollection, IEnumerable<T> {

    private object syncObj = new object();

    private T[] s;
    private int currentStackIndex;
    private int blockSize;

    public int Count {
        get {
            return currentStackIndex + 1;
        }
    }

    public object SyncRoot {
        get {
            return syncObj;
        }
    }

    public bool IsSynchronized {
        get {
            return false;
        }
    }

    public CustomStack(int blockSize = 100) {
        if (blockSize <= 0)
            throw new ArgumentOutOfRangeException("blockSize");

        this.blockSize = blockSize;
        s = new T[blockSize];
        currentStackIndex = -1;
    }

    public void Push(T x) {
        if (currentStackIndex + 1 >= s.Length) {
            Array.Resize(ref s, s.Length + blockSize);
        }

        s[++currentStackIndex] = x;
    }

    public T Pop() {
        if (currentStackIndex < 0)
            throw new InvalidOperationException("The stack is empty");

        T value = s[currentStackIndex];
        s[currentStackIndex--] = default(T);
        return value;
    }

    public T Peek() {
        if (currentStackIndex < 0)
            throw new InvalidOperationException("The stack is empty");

        return s[currentStackIndex];
    }

    public IEnumerator<T> GetEnumerator() {
        return new Enumerator<T>(s, currentStackIndex);
    }

    public void CopyTo(Array array, int index) {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return new Enumerator<T>(s, currentStackIndex);
    }

    public T[] ToArray() {
        T[] clone = new T[Count];
        for (int i = 0; i < Count; i++) {
            clone[i] = s[i];
        }
        return clone;
    }

    class Enumerator<S> : IEnumerator<S> {

        private int topOfStack;
        private int currentStackIndex;
        private S[] s;

        public Enumerator(S[] s, int topOfStack) {
            this.topOfStack = topOfStack;
            this.s = s;
            Reset();
        }

        public S Current => s[currentStackIndex];

        object IEnumerator.Current => s[currentStackIndex];

        public void Dispose() {
            s = null;
        }

        public bool MoveNext() {
            return ++currentStackIndex <= topOfStack;
        }

        public void Reset() {
            currentStackIndex = -1;
        }
    }

}


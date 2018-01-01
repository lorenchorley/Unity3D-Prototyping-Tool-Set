using System;

public interface IRefEnumerator<S> : IDisposable {
    bool MoveNext(ref S current);
    void Reset();
}

 
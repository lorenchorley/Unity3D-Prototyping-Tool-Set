using UnityEngine;
using System.Collections;

namespace eventsource {

    public interface IESQuery {
    }

    public class IESQuery<T> : IESQuery {
        public T result;
    }

}
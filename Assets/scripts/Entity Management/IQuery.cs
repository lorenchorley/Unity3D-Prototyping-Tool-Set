using UnityEngine;
using System.Collections;

namespace eventsourcing {

    public interface IQuery {
    }

    public class IQuery<T> : IQuery {
        public T result;
    }

}
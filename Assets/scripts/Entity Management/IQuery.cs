using UnityEngine;
using System.Collections;

namespace entitymanagement {

    public interface IQuery {
    }

    public class IQuery<T> : IQuery {
        public T result;
    }

}
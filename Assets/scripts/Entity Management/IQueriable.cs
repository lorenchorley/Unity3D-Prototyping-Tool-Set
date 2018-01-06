using UnityEngine;
using System.Collections;

namespace entitymanagement {

    public interface IQueriable<Q> where Q : IQuery {
        void Query(Q q);
    }

}
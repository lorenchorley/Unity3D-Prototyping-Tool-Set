using UnityEngine;
using System.Collections;
using System;

namespace eventsource {

    public interface IESModel {
        EventSource ES { get; }
        void InitES();
        void LoadESFrom(byte[] dataSource);
        byte[] SerialiseES();
        void SynchroniseESToFile(string path);
    }

}
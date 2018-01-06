
using strange.extensions.signal.impl;
using UnityEngine;
/// <summary>
/// A base menu class that implements parameterless Show and Hide methods
/// </summary>
namespace menusystem {

    public abstract class SimpleMenu<T> : MenuView<T> where T : SimpleMenu<T> {

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

    }

}
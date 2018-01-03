using strange.extensions.mediation.impl;
using strange.framework.api;
using strange.framework.impl;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace menusystem {

    public class MenuBinder : Binder {

        public GameObject Context;

        public override IBinding Bind<T>() {
            if (!(typeof(T).IsSubclassOf(typeof(MenuView))))
                throw new Exception("Cannot bind non-MenuView type: " + typeof(T).Name);

            return Bind(typeof(T));
        }

        public override IBinding Bind(object key) {
            if (key is Type && (key as Type).IsSubclassOf(typeof(MenuView)))
                return base.Bind(key);
            else
                throw new Exception("Can only bind MenuView types");
        }

        public object GetInstance<T>() {
            return GetInstance(typeof(T));
        }

        public virtual object GetInstance(Type key) {
            object instance;
            IBinding binding = GetBinding(key, "Instance");

            if (binding == null) {
                binding = GetBinding(key, "Prefab");

                if (binding == null)
                    throw new Exception("MenuBinder has no binding for:\n\tkey: " + key);

                if (binding.value == null)
                    throw new Exception("MenuBinder found null binding for :\n\tkey: " + key);

                if (!(binding.value is GameObject))
                    throw new Exception("MenuBinder found non-GameObject binding for :\n\tkey: " + key);

                GameObject go = GameObject.Instantiate(binding.value as GameObject, Vector3.zero, Quaternion.identity);
                go.name = (key as Type).Name;
                go.transform.SetParent(Context.transform);
                binding = Bind(key).To(go.AddComponent(key as Type)).ToName("Instance");

            }

            if (binding.value == null)
                throw new Exception("MenuBinder found null binding for :\n\tkey: " + key);

            instance = binding.value;
            Assert.IsTrue(instance.GetType().IsSubclassOf(typeof(MenuView)));

            return instance;
        }

    }

}
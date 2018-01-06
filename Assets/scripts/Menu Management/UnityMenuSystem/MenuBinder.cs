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

        public virtual MenuView GetInstance(Type key) {
            IBinding binding = GetBinding(key, "Instance");

            if (binding == null) {
                binding = GetBinding(key, "Prefab");

                if (binding == null)
                    throw new Exception("MenuBinder has no binding for:\n\tkey: " + key);

                if (binding.value == null)
                    throw new Exception("MenuBinder found null binding for:\n\tkey: " + key);

                object[] aa = binding.value as object[];

                if (aa == null || aa.Length == 0 || !(aa[0] is GameObject))
                    throw new Exception("MenuBinder found invalid binding for:\n\tkey: " + key);

                GameObject go = GameObject.Instantiate(aa[0] as GameObject, Vector3.zero, Quaternion.identity);
                go.name = (key as Type).Name;
                go.transform.SetParent(Context.transform);

                Component component = go.GetComponent(key as Type) ?? go.AddComponent(key as Type);
                binding = Bind(key).To(component).ToName("Instance");

            }

            object[] bb = binding.value as object[];
            if (bb == null || bb.Length == 0 || !(bb[0] is MenuView))
                throw new Exception("MenuBinder found invalid binding for:\n\tkey: " + key);

            return bb[0] as MenuView;
        }

    }

}
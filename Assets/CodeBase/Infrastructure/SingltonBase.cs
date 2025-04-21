using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [DisallowMultipleComponent]
    public abstract class SingltonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        public void Init()
        {
            if(Instance != null)
            {
                Debug.LogWarning("MonoSinglton: object of type already existm instance will be destroyed = " + typeof(T).Name);
                Destroy(this);
                return;
            }
            Instance = this as T;
        }
    }

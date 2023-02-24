using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Core
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] objs = FindObjectsOfType<T>();
                    if (objs.Length > 0)
                    {
                        T _instance = objs[0];
                        instance = _instance;
                    }

                    else
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        instance = go.AddComponent<T>();
                        DontDestroyOnLoad(go);
                    }
                }

                return instance;
            }
        }
    }
}

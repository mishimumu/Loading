using UnityEngine;
using System.Collections;
using System;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {

                T t = UnityEngine.Object.FindObjectOfType(typeof(T)) as T;
                if (t != null)
                {
                    instance = t;
                }
                else
                {
                    instance = (T)((object)new GameObject
                    {
                        name = "MonoBehaviorSingleton_" + typeof(T).Name
                    }.AddComponent<T>());
                }
                UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }


}

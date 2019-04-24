using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Singleton<T> where T : new()
{
    private static T s_singleton = default(T);
    private static object s_objectLock = new object();

    public static T Instance
    {
        get
        {
            if (s_singleton == null)
            {
                object obj;
                Monitor.Enter(obj = s_objectLock); //加锁防止多线程创建单例
                try
                {
                    if (s_singleton == null)
                    {
                        s_singleton = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
                            //创建单例的实例
                    }
                }
                finally
                {
                    Monitor.Exit(obj);
                }
            }
            return s_singleton;
        }
    }

    protected Singleton()
    {
    }
    public static T Init()
    {
        return Instance;
    }
}

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    protected static T sInstance = null;
    protected static bool IsCreate = false;
    public static bool s_debugDestroy = false;

    public static T Instance
    {
        get
        {
            if (s_debugDestroy)
            {
                return null;
            }
            CreateInstance();
            return sInstance;
        }
    }

    protected virtual void Awake()
    {
        if (sInstance == null)
        {
            sInstance = this as T;
            IsCreate = true;

            Init();
        }
    }

    protected virtual void Init()
    {
    }

    protected virtual void OnDestroy()
    {
        sInstance = null;
        IsCreate = false;
    }

    public static void CreateInstance()
    {
        if (IsCreate == true)
            return;

        IsCreate = true;
        T[] managers = GameObject.FindObjectsOfType(typeof (T)) as T[];
        if (managers.Length != 0)
        {
            if (managers.Length == 1)
            {
                sInstance = managers[0];
                sInstance.gameObject.name = typeof (T).Name;
                DontDestroyOnLoad(sInstance.gameObject);
                return;
            }
            else
            {
                foreach (T manager in managers)
                {
                    Destroy(manager.gameObject);
                }
            }
        }

        GameObject gO = new GameObject(typeof (T).Name, typeof (T));
        sInstance = gO.GetComponent<T>();
        DontDestroyOnLoad(sInstance.gameObject);
    }

    public static void ReleaseInstance()
    {
        if (sInstance != null)
        {
            Destroy(sInstance.gameObject);
            sInstance = null;
            IsCreate = false;
        }
    }
}
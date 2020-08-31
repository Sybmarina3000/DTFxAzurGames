using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public delegate void callbackFunc();
public delegate void callbackFunc<P1>(P1 param);
public delegate void callbackFunc<P1, P2>(P1 param, P2 param2);
public class callback
{
    callbackFunc func = null;
    protected callback nextCallback = null; 

    public callback(callbackFunc aFunc, callback aCallback = null) 
    {
        func = aFunc;
        nextCallback = aCallback;
    }

    public virtual void use()
    {
        if (func != null)
        {
            func();
        }
        if (nextCallback != null)
        {
            nextCallback.use();
        }
    }
}

public class callback<P1> : callback
{
    callbackFunc<P1> func = null;
    protected P1 p1;

    public callback(callbackFunc<P1> aFunc, P1 aP1, callback aCallback = null) : base(null, aCallback)
    {
        func = aFunc;
        p1 = aP1;
    }
    public override void use()
    {
        if (func != null)
        {
            func(p1);
        }
        base.use();
    }
}

public class callback<P1, P2> : callback<P1>
{
    callbackFunc<P1, P2> func = null;
    P2 p2;

    public callback(callbackFunc<P1, P2> aFunc, P1 aP1, P2 aP2, callback aCallback = null) : base(null, aP1, aCallback)
    {
        func = aFunc;
        p2 = aP2;
    }

    public override void use()
    {
        if (func != null)
        {
            func(p1, p2);
        }
        base.use();
    }
}


public class utilFunction
{
    const int sizePull = 16;
    static System.Random rnd = new System.Random();
    static GameObject lastObject = null;

    static int GetAllObjsFromCollider(Collider2D[] objs, Collider2D collider)
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int count = collider.OverlapCollider(filter, objs);
        return count;
    }

    public static int getRandomInt(int min, int max)
    {
        return rnd.Next(min, max);
    }

    public static List<T> getTargetInCollider<T>(Collider2D collider, List<string> ignorePool)
    {
        List<T> result = new List<T>();
        Collider2D[] objs = new Collider2D[sizePull];
        int count = GetAllObjsFromCollider(objs, collider);
        for (int n = 0; n < count; n++)
        {
            bool ignore = false;
            for (int i = 0; i < ignorePool.Count; i++)
            {
                if (objs[n].gameObject.tag == ignorePool[i])
                {
                    ignore = true;
                    break;
                }
            }
            if (ignore) 
            {
                continue;
            }
            T obj = objs[n].GetComponent<T>();
            if (obj != null)
            {
                result.Add(obj);
            }
        }
        return result;
    }

    static public bool isRange<type>(Collider2D collider, string tag)
    {
        var objs = new Collider2D[sizePull];
        int count = GetAllObjsFromCollider(objs, collider);
        for (int n = 0; n < count; n++)
        {
            var obj = objs[n].GetComponent<type>();
            if (obj != null)
            {
                var cast = obj as MonoBehaviour;
                if (cast.tag == tag)
                {
                    lastObject = cast.gameObject;
                    return true;
                }
            }
        }
        return false;
    }

    static public bool isRange<type>(Collider2D collider, GameObject target)
    {
        var objs = new Collider2D[sizePull];
        int count = GetAllObjsFromCollider(objs, collider);
        for (int n = 0; n < count; n++)
        {
            var obj = objs[n].GetComponent<type>();
            if (obj != null)
            {
                var cast = obj as MonoBehaviour;
                if (cast == target)
                {
                    lastObject = cast.gameObject;
                    return true;
                }
            }
        }
        return false;
    }

    static string loadFile(string nameConfig)
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            return "";
        }
        var dir = Path.Combine(Application.streamingAssetsPath, nameConfig + ".json");
        if (!File.Exists(dir))
        {
            return "";
        }
        return File.ReadAllText(dir);
    }

    static public T[] loadArrayData<T>(string config)
    {
        var data = loadFile(config);
        if (data.Length > 0)
        {
            return JsonLoader.FromJson<T>(data);
        }
        return default;
    }

    static public T loadData<T>(string config)
    {
        var data = loadFile(config);
        if (data.Length > 0)
        {
            return JsonUtility.FromJson<T>(data);
        }
        return default;
    }

    static protected void saveFile(string data, string nameFile)
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            return; 
        }
        var dir = Path.Combine(Application.streamingAssetsPath, nameFile + ".json");
        File.WriteAllText(dir, data);
    }


    static public void saveArrayData<T>(T[] data, string config)
    {
        var dataElemnt = JsonLoader.ToJson(data);
        saveFile(dataElemnt, config);
    }
}

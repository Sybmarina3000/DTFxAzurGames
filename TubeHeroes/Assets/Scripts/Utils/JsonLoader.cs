using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JsonLoader : MonoBehaviour
{
    [Serializable]
    public class array<T>
    {
        public T[] Items;
    }

    public static T[] FromJson<T>(string json)
    {
        array<T> wrapper = JsonUtility.FromJson<array<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        array<T> wrapper = new array<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        array<T> wrapper = new array<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

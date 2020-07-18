using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderFactory
{
    [Serializable]
    public struct colliderConfigs
    {
        public string name;
        public string type;
        public bool trigger;
        public List<float> parameters;
    }

    
    SortedDictionary<int, Collider2D> keyToCollider;//пулл созданных коллайдеров


    static public Collider2D getCollideFromParameters(string type, List<float> parameters, bool trigger, GameObject target)
    {
        Collider2D result = null;
        if (type == "box" && parameters.Count == 4)
        {
            var boxCollide = target.AddComponent<BoxCollider2D>();
            boxCollide.size = new Vector2(parameters[2], parameters[3]);
            result = boxCollide;
        }
        else if (type == "circle" && parameters.Count == 3)
        {
            var circleCollide = target.AddComponent<CircleCollider2D>();
            circleCollide.radius = parameters[2];
            result = circleCollide;
        }
        else if (type == "capsule" && parameters.Count == 4)
        {
            var capsuleCollide = target.AddComponent<CapsuleCollider2D>();
            capsuleCollide.size = new Vector2(parameters[2], parameters[3]);
            result = capsuleCollide;
        }

        if (parameters.Count > 2 && result != null)
        {
            result.offset = new Vector2(parameters[0], parameters[1]);
        }
        result.isTrigger = trigger;
        return result;
    }

    public void initColliders(string dirColliderConfig, SortedDictionary<string, int> nameToKey, SortedDictionary<int, GameObject> keyToGameObject)
    {
        keyToCollider = new SortedDictionary<int, Collider2D>();
        var settingsCollider = utilFunction.loadArrayData<colliderConfigs>(dirColliderConfig);
        for (int n = 0; n < settingsCollider.Length; n++)
        {
            var element = settingsCollider[n];
            var key = nameToKey[element.name];
            var gameObject = keyToGameObject[key];
            keyToCollider[key] = getCollideFromParameters(element.type, element.parameters, element.trigger, gameObject);
        }
        nameToKey = null;
    }

    public Collider2D getColliderFromId(int id)
    {
        return keyToCollider[id];
    }

}

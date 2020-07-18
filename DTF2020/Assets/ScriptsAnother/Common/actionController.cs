using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionController : MonoBehaviour
{
   
    static Dictionary<GameObject, List<actionHead>> keyToAction = new Dictionary<GameObject, List<actionHead>>();
    static Dictionary<GameObject, List<GameObject>> keyToModels = new Dictionary<GameObject, List<GameObject>>();

    void Update()
    {
        if (keyToAction.Count == 0 )
        {
            return;
        }
        foreach (var poolAction in keyToAction)
        {
            if (poolAction.Value.Count > 0) 
            {
                var action = poolAction.Value[0];
                action.update();
                if (action.isEnd())
                {
                    poolAction.Value.Remove(action);
                    action.callCallback();
                }
            }
        }
    }

    static public void init(GameObject aHead, List<GameObject> aModels)
    {
        keyToAction[aHead] = new List<actionHead>();
        keyToModels[aHead] = aModels;
    }

    static public void remove(GameObject aHead)
    {
        keyToAction.Remove(aHead);
        keyToModels.Remove(aHead);
    }

    static public void removeAll()
    {
        keyToAction.Clear();
        keyToModels.Clear();
    }

    static public void clearActions(GameObject aHead)
    {
        foreach (var poolAction in keyToAction)
        {
            if (poolAction.Key == aHead)
            {
                poolAction.Value.Clear();
            }
        }
    }

    static bool isValidTarget(GameObject aHead)
    {
        if (keyToModels[aHead] == null || keyToAction[aHead] == null)
        {
            return false;
        }
        return true;
    }

    static public List<actionHead> getActions(GameObject aTarget)
    {
        List<actionHead> result = null;
        if (keyToAction[aTarget] != null)
        {
            result = keyToAction[aTarget];
        }
        return result;
    }

    static public void addMoveToFromPosAndTime(GameObject aHead, Vector2 startPos, Vector2 endPos, typeMove type, float time, callback aCallback = null)
    {
        if (!isValidTarget(aHead)) { return; }
        var actionProgress = new actionTime(time);
        var action = new actionMove(startPos, endPos, type);
        keyToAction[aHead].Add(new actionHead(aHead, actionProgress, action, aCallback));
    }

    static public void addMoveToFromPosAndSpeed(GameObject aHead, Vector2 startPos, Vector2 endPos, typeMove type, float maxSpeed, float minSpeed, typeSpeed typeS, callback aCallback = null)
    {
        if (!isValidTarget(aHead)) { return; }
        var actionProgress = new actionSpeed(maxSpeed, minSpeed, Vector2.Distance(startPos, endPos), typeS);
        var action = new actionMove(startPos, endPos, type);
        keyToAction[aHead].Add(new actionHead(aHead, actionProgress, action, aCallback));
    }

    static public void addMoveToFromObjectAndSpeed(GameObject aHead, Vector2 startPos, GameObject aTarget, typeMove type, float maxSpeed, float minSpeed, typeSpeed typeS, callback aCallback = null)
    {
        if (!isValidTarget(aHead)) { return; }
        var actionProgress = new actionSpeed(maxSpeed, minSpeed, Vector2.Distance(startPos, aTarget.transform.position), typeS);
        var action = new actionMoveDynamic(startPos, aTarget, type);
        keyToAction[aHead].Add(new actionHead(aHead, actionProgress, action, aCallback));
    }

    static public void addChangeScaleAndTime(GameObject aHead, Vector2 startScale, Vector2 endScale, float time, callback aCallback = null)
    {
        if (!isValidTarget(aHead)) { return; }
        var actionProgress = new actionTime(time);
        var action = new actionChangeScale(startScale, endScale);
        keyToAction[aHead].Add(new actionHead(aHead, actionProgress, action, aCallback));
    }

    static public void addMoveToFromObjectAndTime(GameObject aHead, Vector2 startPos, GameObject aTarget, typeMove type, float time, callback aCallback = null)
    {
        if (!isValidTarget(aHead)) { return; }
        var actionProgress = new actionTime(time);
        var action = new actionMoveDynamic(startPos, aTarget, type);
        keyToAction[aHead].Add(new actionHead(aHead, actionProgress, action, aCallback));
    }

    static public void addDelay(GameObject aHead, float time, callback aCallback = null)
    {
        if (!isValidTarget(aHead)) { return; }
        var actionProgress = new actionTime(time);
        var action = new actionDelay();
        keyToAction[aHead].Add(new actionHead(aHead, actionProgress, action, aCallback));
    }

    static public void addChangeAlpha(GameObject aHead, float startAlpha, float endAlpha, float time, callback aCallback = null)
    {
        if (!isValidTarget(aHead)) { return; }
        if (keyToModels[aHead].Count > 0)
        {
            var actionProgress = new actionTime(time);
            var action = new actionChangeAlpha(startAlpha, endAlpha);
            keyToAction[aHead].Add(new actionHead(keyToModels[aHead], actionProgress, action, aCallback));
        }
    }
}

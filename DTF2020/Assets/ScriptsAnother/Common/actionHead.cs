using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionHead
{
    List<GameObject> subjects = null;
    actionValue value = default;
    actionType type = default;
    callback valueCallback = null;
    actionHead() { }
    public actionHead(GameObject aSubject, actionValue aValue, actionType aType, callback aValueCallback = null) { subjects = new List<GameObject>(); subjects.Add(aSubject); value = aValue; type = aType; valueCallback = aValueCallback; }

    public actionHead(List<GameObject> aSubjects, actionValue aValue, actionType aType, callback aValueCallback = null) { subjects = aSubjects; value = aValue; type = aType; valueCallback = aValueCallback; }

    bool checkObject()
    {
        if (type == null)
        {
            return false;
        }
        if (value == null)
        {
            return false;
        }
        if (subjects == null)
        {
            return false;
        }
        foreach (var subject in subjects)
        {
            if (subject == null)
            {
                return false;
            }
        }
        return true;
    }
    public virtual void update()
    {
        if (!checkObject())
        {
            return;
        }
        foreach (var subject in subjects)
        {
            type.update(subject, value);
        }
    }
    public virtual void end()
    {
        if (!checkObject())
        {
            return;
        }
        type.end(value);
    }
    public virtual bool isEnd()
    {
        if (!checkObject())
        {
            return true;
        }
        bool result = true;
        foreach (var subject in subjects)
        {
            result = type.isEnd(subject, value) && result;
        }
        return result;
    }

    public void callCallback()
    {
        if (valueCallback != null)
        {
            valueCallback.use();
        }
    }
}


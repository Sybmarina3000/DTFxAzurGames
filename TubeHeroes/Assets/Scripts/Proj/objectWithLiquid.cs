using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class slostsDictionary : SerializableDictionaryBase<GameObject, objectWithLiquid> { }

public class objectWithLiquid : MonoBehaviour
{
    public slostsDictionary slots = new slostsDictionary();
    public uint countActiveSlots = 0;

    private void Start()
    {
        init();
    }

    protected void init()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Collider2D>())
            {
                slots.Add(child.gameObject, null);
            }
        }
    }

    public virtual bool connectObject(objectWithLiquid input, GameObject slot) 
    { 
        if (slots.ContainsKey(slot)) 
        { 
            if (slots[slot] == null) 
            { 
                slots[slot] = input;
                countActiveSlots++;
                return true;  
            } 
        } 
        return false; 
    }

    public virtual bool disconnectObject(objectWithLiquid input)
    {
        if (input == null)
        {
            return false;
        }
        foreach (var element in slots)
        {
            if (element.Value == input)
            {
                slots[element.Key] = null;
                countActiveSlots--;
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        
    }

    public virtual float proceedLiquid(liquid objectLiquid, float count)
    {
        return 0.0f;
    }

    public virtual bool isOutSlot(GameObject slot)
    {
        return false;
    }

    public bool isValidSlot(GameObject slot)
    {
        return !slots.ContainsKey(slot);
    }

    public virtual List<GameObject> getSlostForMovement()
    {
        return null;
    }

    public slostsDictionary getAllConnects()
    {
        return slots;
    }
}

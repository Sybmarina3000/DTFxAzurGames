using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tube : objectWithLiquid
{
    public List<GameObject> outSlots = new List<GameObject>();

    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override float proceedLiquid(liquid objectLiquid, float count)
    {
        if (count <= 0.0f)
        {
            return 0.0f;
        }
        int countOutSlots = outSlots.Count + 1;

        foreach (var slot in outSlots)
        {
            var volumeForCurrentSlot = count / countOutSlots;
            if (slots.ContainsKey(slot))
            {
                var target = slots[slot];
                var remainder = target.proceedLiquid(objectLiquid, volumeForCurrentSlot);
                volumeForCurrentSlot -= remainder;
                count -= volumeForCurrentSlot;
                countOutSlots--;
            }
            else
            {
                count -= volumeForCurrentSlot;
                countOutSlots--;
            }
        }
        return count;
    }

    public override bool isOutSlot(GameObject slot)
    {
        return outSlots.Contains(slot);
    }
}

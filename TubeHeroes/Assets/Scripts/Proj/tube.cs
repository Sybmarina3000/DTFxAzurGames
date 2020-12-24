using Obi;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tube : objectWithLiquid
{
    public List<GameObject> outSlots = new List<GameObject>();
    public GameObject end = null;
    public GameObject liquidObject = null;
    public float countLiquid = 0.0f;
    bool needOut = false;
    public liquid currentLiquid = new liquid();
    float dtLiquid = 0;
    bool needReloadLiquid = false;
    public bool lockOut = false;

    public bool debugWater = false;
    public bool debugPoison = false;
    liquid debugLiquid = new liquid();

    void Start()
    {
        init();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (lockOut)
        {
            return;
        }
        if (countLiquid > 0)
        {
            dtLiquid += Time.deltaTime;
        }
        if (countLiquid < float.Epsilon)
        {
            countLiquid = 0.0f;
        }
        if (dtLiquid > 0.5f && countLiquid > 0.01f)
        {
            dtLiquid = 0;

            bool goodConnect = false;
            int countOutSlots = outSlots.Count + 1;
            foreach (var slot in outSlots)
            {
                var volumeForCurrentSlot = countLiquid / countOutSlots;
                if (slots.ContainsKey(slot))
                {
                    var target = slots[slot];
                    if (target)
                    {
                        var remainder = target.proceedLiquid(currentLiquid, volumeForCurrentSlot);
                        volumeForCurrentSlot = volumeForCurrentSlot - remainder;
                        countLiquid = countLiquid - volumeForCurrentSlot;
                        countOutSlots--;
                        goodConnect = true;
                    }
                }
            }
            if (goodConnect)
            {
                needOut = false;
                return;
            }
            else
            {
                needOut = true;
            }
            
        }
        if (countLiquid > 0.01f && needOut)
        {
            countLiquid -= worldSettings.speedLiquid * Time.deltaTime;
            if (!currentLiquid.isStart())
            {
                currentLiquid.outStart(liquidObject);
            }
            else if (currentLiquid.getSpeedOut() == 0)
            {
                currentLiquid.setSpeedOut(worldSettings.speedLiquidEmmiters);
            }
            if (needReloadLiquid && currentLiquid.isStart())
            {
                currentLiquid.outStart(liquidObject);
                needReloadLiquid = false;
            }
        }
        else
        {
            if (currentLiquid.isStart())
            {
                currentLiquid.setSpeedOut(0);
            }
        }


        if (slots[end] == null && (debugWater || debugPoison))
        {
            cheatsAction();
        }
    }

    void cheatsAction()
    {
        if (debugWater && debugLiquid.currentType != liquid.typeLiquid.water)
        {
            debugLiquid.outStop();
            debugLiquid.currentType = liquid.typeLiquid.water;
            debugLiquid.outStart(liquidObject);
        }
        else if (debugPoison && debugLiquid.currentType != liquid.typeLiquid.poison)
        {
            debugLiquid.outStop();
            debugLiquid.currentType = liquid.typeLiquid.poison;
            debugLiquid.outStart(liquidObject);
        }
        else if (debugLiquid.currentType != liquid.typeLiquid.empty && !debugWater && !debugPoison)
        {
            debugLiquid.outStop();
            debugLiquid.currentType = liquid.typeLiquid.empty;
        }
        else
        {
            if (!debugLiquid.isStart())
            {
                return;
            }
            if (debugLiquid.getCountActiveParticle() > currentLiquid.getCountActiveParticle())
            {
                debugLiquid.setSpeedOut(0);
            }
        }
    }

    public override float proceedLiquid(liquid objectLiquid, float count)
    {
        if (count <= 0.0f)
        {
            return 0.0f;
        }

        if (count > worldSettings.speedLiquid)
        {
            countLiquid += worldSettings.speedLiquid;
            count -= worldSettings.speedLiquid;
        }
        else
        {
            countLiquid += count;
            count = 0;
        }
        currentLiquid.mix(objectLiquid);
        return count;
    }

    public override bool isOutSlot(GameObject slot)
    {
        return outSlots.Contains(slot);
    }

    public override bool isEmptySlot(GameObject slot)
    {
        bool result = false;
        if (slots.ContainsKey(slot))
        {
            result = slots[slot] == null;
        }
        return result;
    }
}

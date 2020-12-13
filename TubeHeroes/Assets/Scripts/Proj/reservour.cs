using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reservour : objectWithLiquid
{
    public bool lockDisconnect = false;
    public liquid currentLiquid;
    public float volumeLiquid = 1.0f;
    const float maxVolumeLiquid = 1.0f;
    public bool input = false;

    private void Start()
    {
        if (volumeLiquid > maxVolumeLiquid)
        {
            volumeLiquid = maxVolumeLiquid;
        }
        if (volumeLiquid < 0.0f)
        {
            volumeLiquid = 0.0f;
        }
        init();
    }

    public override bool connectObject(objectWithLiquid input, GameObject slot)
    {
        return base.connectObject(input, slot);
    }

    public override bool disconnectObject(objectWithLiquid input)
    {
        if (lockDisconnect) 
        {
            return false;
        }
        return base.disconnectObject(input);
    }

    private void Update()
    {
        if (volumeLiquid <= 0.0f)
        {
            return;
        }
        if (input)
        {
            return;
        }
        var volume = Time.deltaTime * worldSettings.speedLiquid;
        if (volume > volumeLiquid)
        {
            volume = volumeLiquid;
        }

        foreach (var slot in slots)
        {
            if (slot.Value != null)
            {
                var volumeForCurrentSlot = volume / Convert.ToSingle(countActiveSlots);
                var remainder = slot.Value.proceedLiquid(currentLiquid, volumeForCurrentSlot);
                volumeForCurrentSlot -= remainder;
                volumeLiquid -= volumeForCurrentSlot;
            }
        }
    }

    public override float proceedLiquid(liquid objectLiquid, float count)
    {
        currentLiquid.mix(objectLiquid);
        if (volumeLiquid + count > maxVolumeLiquid)
        {
            volumeLiquid = maxVolumeLiquid;
            count = maxVolumeLiquid - volumeLiquid + count;
            return count;
        }
        volumeLiquid += count;
        return 0.0f;
    }
}

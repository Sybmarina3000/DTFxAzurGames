using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class liquid
{
    public enum typeLiquid { poison, water, empty };

    public typeLiquid currentType = typeLiquid.empty;

    public void mix(liquid input)
    {
        switch (currentType)
        {
            case typeLiquid.water:
                {
                    if (input.currentType == typeLiquid.poison)
                    {
                        currentType = typeLiquid.poison;
                    }
                    break;
                }
            case typeLiquid.poison:
                {
                    if (input.currentType == typeLiquid.water)
                    {
                        currentType = typeLiquid.poison;
                    }
                    break;
                }
            case typeLiquid.empty:
                {
                    currentType = input.currentType;
                    break;
                }
        }
    }
}

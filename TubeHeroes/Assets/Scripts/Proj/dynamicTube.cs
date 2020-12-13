using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicTube : tube
{
    public float d = 0;
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override List<GameObject> getSlostForMovement()
    {
        return outSlots;
    }
}

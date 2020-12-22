using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicTube : tube
{
    public float x0 = 0;
    public float y0 = 0;
    public float x1 = 0;
    public float y1 = 0;
    public Vector3 startPosEnd = Vector3.zero;
    public Quaternion startRotationEnd = new Quaternion(0,0,0,0);
    public GameObject rope = null;
    void Start()
    {
        init();
        startPosEnd = end.transform.position;
        startRotationEnd = end.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override List<GameObject> getSlostForMovement()
    {
        return outSlots;
    }
}

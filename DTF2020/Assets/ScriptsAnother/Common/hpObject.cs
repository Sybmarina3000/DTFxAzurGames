using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpObject : MonoBehaviour
{

    GameObject parent = null;
    void Start()
    {

    }

    public void initParent(GameObject aParent)
    {
        parent = aParent;
    }

    public GameObject getParent()
    {
        return parent;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

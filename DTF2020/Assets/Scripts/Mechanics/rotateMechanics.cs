using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateMechanics : MonoBehaviour
{
    public float rotateSpeed = 1.0f;
    public bool revertRotate = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var rotation = transform.rotation.eulerAngles;
        var chep = Time.deltaTime * rotateSpeed;
        if (revertRotate)
        {
            chep *= -1;
        }
        transform.eulerAngles = (new Vector3(rotation.x, rotation.y, rotation.z + chep));
    }
}

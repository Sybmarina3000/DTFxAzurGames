using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testForce : MonoBehaviour
{
    bool input = false;
    Rigidbody2D unityRigidbody = null;

    // Start is called before the first frame update
    void Start()
    {
        unityRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            input = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            input = false;
            unityRigidbody.velocity = Vector3.zero;
        }

        if (input)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = transform.position.z;
            transform.position = pos;
        }


    }
}

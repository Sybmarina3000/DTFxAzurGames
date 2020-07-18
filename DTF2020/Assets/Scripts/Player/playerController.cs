﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, actionObject
{
    public UnityEngine.UI.Text debugText;
    public bool slowMode = false;
    float force = 300.0f;
    Rigidbody2D moveController = null;
    // Start is called before the first frame update
    void Start()
    {
        initActionObject();
        colliderFactory.getCollideFromParameters("box", new List<float> { 0, 0,1,1 }, true, gameObject);
        moveController = GetComponent<Rigidbody2D>();
    }

    ~playerController()
    {
        destroyActionObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            slowMode = !slowMode;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            jump(new Vector2(0, 1));
        }
        if (slowMode)
        {
            float slowSpeed = 0.2f;
            if (Time.timeScale > slowSpeed)
            {
                Time.timeScale -= Time.deltaTime * 4;
            }
            else
            {
                Time.timeScale = slowSpeed;
            }
            debugText.text = Time.timeScale.ToString();
        }
        else
        {
            float normalSpeed = 1.0f;
            if (Time.timeScale < normalSpeed)
            {
                Time.timeScale += Time.deltaTime * 5;
            }
            else
            {
                Time.timeScale = normalSpeed;
            }
            debugText.text = Time.timeScale.ToString();
        }
    }

    public void jump(Vector2 direction)
    {
        moveController.gravityScale = 2;
        moveController.velocity = new Vector2(moveController.velocity.x, 0);
        moveController.AddForce(new Vector2 (direction.x * force, direction.y * force));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        actionController.clearActions(gameObject);
        moveController.velocity = new Vector2(0, 0);
        moveController.gravityScale = 0;
    }

    public void initActionObject()
    {
        actionController.init(gameObject, new List<GameObject> { gameObject });
    }

    public void destroyActionObject()
    {
        actionController.remove(gameObject);
    }
}

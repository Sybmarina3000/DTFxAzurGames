using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeToDeath : MonoBehaviour
{
    public float time = 1.0f;
    public Slider slider = null;
    float dt = 0.0f;
    public bool startGame = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!startGame)
        {
            return;
        }
        if (dt >= time)
        {
            return;
        }
        dt += Time.deltaTime;
        slider.value = (time - dt) / time;
        
    }
}

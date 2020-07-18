using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class bounceButton : Button, actionObject
{
    Vector2 baseScale = new Vector2();
    float upScale = 1.2f;
    float downScale = 0.8f;
    bool block = false;
    protected override void Start()
    {
        base.Start();
        initActionObject();
        var size = transform.localScale;
        baseScale.x = size.x;
        baseScale.y = size.y;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (block)
        {
            return;
        }
        setBlock(true);
        var unlockButton = new callback<bool>(new callbackFunc<bool>(setBlock), false);
        var callback = new callback<PointerEventData>(new callbackFunc<PointerEventData>(base.OnPointerClick), eventData, unlockButton);
        var downScaleVec = new Vector2(baseScale.x * downScale, baseScale.y * downScale);
        var upScaleVec = new Vector2(baseScale.x * upScale, baseScale.y * upScale);
        actionController.addChangeScaleAndTime(gameObject, baseScale, downScaleVec, 0.05f);
        actionController.addChangeScaleAndTime(gameObject, downScaleVec, upScaleVec, 0.1f);
        actionController.addChangeScaleAndTime(gameObject, upScaleVec, baseScale, 0.05f, callback);
    }

    void setBlock(bool value)
    {
        block = value;
    }

    public void initActionObject()
    {
        actionController.init(gameObject, new List<GameObject>());
    }

    public void destroyActionObject()
    {
        actionController.remove(gameObject);
    }
}

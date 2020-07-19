using System.Collections.Generic;
using UnityEngine;

public class laserController : MonoBehaviour, actionObject
{
    // Start is called before the first frame update
    public string dirAnimConfig;
    public string dirAnimMixConfig;
    animController controller;
    actionHead action = default;
    void Start()
    {
        controller = new animController();
        controller.init(gameObject, dirAnimConfig, dirAnimMixConfig);
        controller.setIdle();
        initActionObject();
    }

    //~laserController() 
    //{
    //    destroyActionObject();
    //}

    // Update is called once per frame
    void Update()
    {
        controller.customUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (controller.isIdle())
        {
            controller.setDamage();
            if (action != default)
            {
                action.end();
            }
            action = actionController.addDelay(gameObject, 1, new callback(new callbackFunc(controller.setIdle)));
        }
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

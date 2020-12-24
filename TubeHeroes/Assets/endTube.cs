using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endTube : MonoBehaviour, actionObject
{
    public GameObject spineGas = null;
    public GameObject spinePrinces = null;
    public GameObject world = null;
    public Slider slider = null;
    public tube tubeObject;
    public reservour reservour = null;
    bool endGame = false;
    bool delayLose = false;
    public void initActionObject()
    {
        actionController.init(gameObject, new List<GameObject>());
    }

    public void destroyActionObject()
    {
        actionController.remove(gameObject);
    }

    void Start()
    {
        initActionObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (endGame)
        {
            return;
        }
        if (tubeObject.currentLiquid.currentType != liquid.typeLiquid.empty)
        {
            endGame = true;
        }
        //if (reservour.volumeLiquid <= 0.0f && !delayLose)
        //{
        //    callback callback = new callback(checkDelayGameOver);
        //    actionController.addDelay(gameObject, 2.0f, callback);
        //    delayLose = true;
        //}
        if (tubeObject.currentLiquid.currentType == liquid.typeLiquid.water)
        {
            var component = world.GetComponent<gameEvents>();
            component.lockController();
            callback callback = new callback(component.win);
            actionController.addDelay(gameObject, 2.0f, callback);
        }
        if (tubeObject.currentLiquid.currentType == liquid.typeLiquid.poison || slider.value <= 0.0f)
        {
            poisonGameOver();
        }
    }

    void checkDelayGameOver()
    {
        if (!endGame)
        {
            poisonGameOver();
        }
    }

    void poisonGameOver()
    {
        spineGas.SetActive(true);
        spineGas.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "gas_star", false);
        spineGas.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "gas_loop", true, 0);
        endGame = true;
        spinePrinces.SetActive(false);
        var component = world.GetComponent<gameEvents>();
        component.lockController();
        callback callback = new callback(component.gameOver);
        actionController.addDelay(gameObject, 2.0f, callback);
    }
}
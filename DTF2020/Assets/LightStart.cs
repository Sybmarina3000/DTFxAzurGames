using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStart : MonoBehaviour, actionObject
{
    public Spine.Unity.SkeletonAnimation componentSkeletonAnimation = null;
    // Start is called before the first frame update
    void Start()
    {
        DefaultNamespace.RealizeBox.instance.score.StartDecreaseScore();
        componentSkeletonAnimation.AnimationState.SetAnimation(0, "light", false);
        initActionObject();
        DefaultNamespace.RealizeBox.instance.score.StopDecreaseScore();
        actionController.addDelay(gameObject, 1.5f, new callback(new callbackFunc(DefaultNamespace.RealizeBox.instance.player.startPlay)));
    }

    // Update is called once per frame
    void Update()
    {
        
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

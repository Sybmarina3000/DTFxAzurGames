using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveMechanics : MonoBehaviour, actionObject
{
    // Start is called before the first frame update
    Vector2 startPos;
    public Vector2 endPos;
    public float time;
    void Start()
    {
        startPos = transform.position;
        endPos = endPos + startPos;
        initActionObject();
        goEndPos();
    }

    //~moveMechanics()
    //{
    //    destroyActionObject();
    //}

    void goStartPos()
    {
        actionController.addMoveToFromPosAndTime(gameObject, endPos, startPos, typeMove.LINARY, time, new callback(new callbackFunc(goEndPos)));
    }

    void goEndPos()
    {
        actionController.addMoveToFromPosAndTime(gameObject, startPos, endPos, typeMove.LINARY, time, new callback(new callbackFunc(goStartPos)));
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

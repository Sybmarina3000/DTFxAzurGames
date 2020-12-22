using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    objectWithLiquid lockTube = null;
    static public GameObject movementSlot = null;
    SpriteRenderer targetSprite = null;
    public bool cheatMode = false;
    static public Vector3 mousePosition = Vector3.zero;
    static public Vector3 startMousePosition = Vector3.zero;
    static public Vector3 startObjectPosition = Vector3.zero;
    static public bool interactionMouse = false;

    public bool debug = false;
    public GameObject debugSaveSlot = null;
    public GameObject debugMoveSlot = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            if (debugSaveSlot && debugMoveSlot)
            {
                debugMoveSlot.transform.position = debugSaveSlot.transform.position;
                debugMoveSlot.transform.localRotation = debugSaveSlot.transform.localRotation;
            }
        }
        if (lockTube && movementSlot)
        {
            var objectTybe = lockTube as dynamicTube;
            float x0 = -10;
            float x1 = 10;
            float y0 = -10;
            float y1 = 10;
            Vector3 startPos = Vector3.zero;
            Quaternion startRotation = new Quaternion(0, 0, 0, 0);
            if (objectTybe)
            {
                x0 = objectTybe.x0;
                x1 = objectTybe.x1;
                y0 = objectTybe.y0;
                y1 = objectTybe.y1;
                startPos = objectTybe.startPosEnd;
                startRotation = objectTybe.startRotationEnd;
            }
            var result = utilFunction.getTargetInCollider<Collider2D>(movementSlot.GetComponent<Collider2D>(), null);
            objectWithLiquid objectConnect = null;
            bool goodConnect = false;
            GameObject slotConnect = null;
            foreach (var elementResult in result)
            {
                if (result.Count > 0)
                {
                    slotConnect = elementResult.gameObject;
                    if (slotConnect == movementSlot)
                    {
                        slotConnect = null;
                    }
                    else
                    {
                        objectConnect = slotConnect.GetComponentInParent<objectWithLiquid>();
                    }
                }
                if (slotConnect == null && targetSprite)
                {
                    targetSprite.color = new Color(255, 255, 255);
                    targetSprite = null;
                }
                if (objectConnect)
                {
                    goodConnect = !objectConnect.isOutSlot(slotConnect) && objectConnect.isValidSlot(movementSlot) && !objectConnect.isValidSlot(slotConnect) && objectConnect.isEmptySlot(slotConnect);
                    if (goodConnect)
                    {
                        if (slotConnect)
                        {
                            targetSprite = slotConnect.GetComponentInChildren<SpriteRenderer>();
                        }
                        break;
                    }
                    else
                    {
                        slotConnect = null;
                        objectConnect = null;
                    }
                }
                else
                {
                    slotConnect = null;
                    objectConnect = null;
                }
            }

            if (targetSprite)
            {
                targetSprite.color = new Color(0, 255, 255);
            }
            if (isInteractionEnd())
            {
                if (goodConnect)
                {
                    objectConnect.connectObject(lockTube, slotConnect);
                    lockTube.connectObject(objectConnect, movementSlot);
                    movementSlot.transform.position = slotConnect.transform.position;
                    movementSlot.transform.localRotation = slotConnect.transform.localRotation;
                }
                else if(movementSlot)
                {
                    movementSlot.transform.position = startPos;
                    movementSlot.transform.rotation = startRotation;
                }
                if (targetSprite)
                {
                    targetSprite.color = new Color(255, 255, 255);
                }
                if (debug)
                {
                    debugSaveSlot = slotConnect;
                    debugMoveSlot = movementSlot;
                }
                targetSprite = null;
                lockTube = null;
                movementSlot = null;
                mousePosition = Vector3.zero;
                startObjectPosition = Vector3.zero;
                startMousePosition = Vector3.zero;
                interactionMouse = false;
                return;
            }
            checkMousePoseAndMoveTube(x0, y0, x1, y1);
            return;
        }
        if (isInteractionStart())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if (hit.collider == null)
            {
                return;
            }
            objectWithLiquid[] result = null;
            if (cheatMode)
            {
                result = FindObjectsOfType<objectWithLiquid>();
            }
            else
            {
                result = FindObjectsOfType<dynamicTube>();
            }
            bool endSearch = false;
            foreach (var liquidObject in result)
            {
                if (endSearch)
                {
                    break;
                }
                var slotForConnect = liquidObject.getAllConnects();
                List<GameObject> slotsForMove = liquidObject.getSlostForMovement();
                if (slotForConnect == null)
                {
                    continue;
                }
                if (cheatMode)
                {
                    foreach (var slot in slotForConnect)
                    {
                        if (slot.Key == hit.collider.gameObject)
                        {
                            lockTube = liquidObject;
                            movementSlot = hit.collider.gameObject;
                            disconnectOldTube(slotForConnect);
                            endSearch = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (slotsForMove == null)
                    {
                        continue;
                    }
                    foreach (var slot in slotsForMove)
                    {
                        if (slot == hit.collider.gameObject)
                        {
                            lockTube = liquidObject;
                            movementSlot = hit.collider.gameObject;
                            disconnectOldTube(slotForConnect);
                            endSearch = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    void disconnectOldTube(slostsDictionary slots)
    {
        if (slots.ContainsKey(movementSlot))
        {
            var connectObject = slots[movementSlot];
            if (connectObject && lockTube)
            {
                lockTube.disconnectObject(connectObject);
                connectObject.disconnectObject(lockTube);
            }
        }
        
    }


    void checkMousePoseAndMoveTube(float x0, float y0, float x1, float y1)
    {
        if (!interactionMouse)
        {
            startMousePosition = Camera.main.ScreenToWorldPoint(getInteractionPosition());
            startObjectPosition = movementSlot.transform.localPosition;
            interactionMouse = true;
        }
        var realMousePos = Camera.main.ScreenToWorldPoint(getInteractionPosition());
        mousePosition = realMousePos - startMousePosition;
        var localPosition = startObjectPosition + mousePosition;
        localPosition.z = 0;
        if (localPosition.x < x0)
        {
            localPosition.x = x0;
        }
        if (localPosition.y < y0)
        {
            localPosition.y = y0;
        }
        if (localPosition.x > x1)
        {
            localPosition.x = x1;
        }
        if (localPosition.y > y1)
        {
            localPosition.y = y1;
        }

        movementSlot.transform.localPosition = localPosition;

        var targetRotation = realMousePos - lockTube.transform.position;

        var rotator = Quaternion.LookRotation(new Vector3(0,0,1), Quaternion.Euler(0, 0, 180) * targetRotation);

        movementSlot.transform.rotation = rotator;
        var obj = lockTube as dynamicTube;
        if (obj)
        {
            var ropeObject = obj.rope;
            var ropeComponent = ropeObject.GetComponent<Obi.ObiRopeExtrudedRenderer>();
            if (ropeComponent)
            { 
                ropeComponent.uvScale.y = 0.4f + (Mathf.Abs(localPosition.x) + Mathf.Abs(localPosition.y)) / 16;
            }
        }
    }

    Vector3 getInteractionPosition()
    {
        Vector3 result = Vector3.zero;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            result = Input.touches[0].position;
        }
        else
        {
            result = Input.mousePosition;
        }
        return result;
    }

    bool isInteractionStart()
    {
        bool result = false;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            result = (Input.touchCount > 0);
        }
        else
        {
            result = Input.GetMouseButtonDown(0);
        }
        return result;
    }

    bool isInteractionEnd()
    {
        bool result = false;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            result = (Input.touchCount == 0);
        }
        else
        {
            result = Input.GetMouseButtonUp(0);
        }
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    objectWithLiquid lockTube = null;
    GameObject movementSlot = null;
    SpriteRenderer targetSprite = null;
    public bool cheatMode = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lockTube && movementSlot)
        {
            var objectTybe = lockTube as dynamicTube;
            float d = 999;
            if (objectTybe)
            {
                d = objectTybe.d;
            }
            var result = utilFunction.getTargetInCollider<Collider2D>(movementSlot.GetComponent<Collider2D>(), null);
            objectWithLiquid objectConnect = null;
            bool goodConnect = false;
            GameObject slotConnect = null;
            if (result.Count > 0)
            {
                slotConnect = result[0].gameObject;
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
                goodConnect = !objectConnect.isOutSlot(slotConnect) && objectConnect.isValidSlot(movementSlot);
                if (goodConnect)
                {
                    if (slotConnect)
                    {
                        targetSprite = slotConnect.GetComponentInChildren<SpriteRenderer>();
                    }
                }
            }
            if (targetSprite)
            {
                targetSprite.color = new Color(0, 255, 255);
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (goodConnect)
                {
                    objectConnect.connectObject(lockTube, slotConnect);
                    lockTube.connectObject(objectConnect, movementSlot);
                }
                if (movementSlot)
                {
                    movementSlot.transform.localPosition = Vector3.zero;
                }

                if (targetSprite)
                {
                    targetSprite.color = new Color(255, 255, 255);
                }
                targetSprite = null;
                lockTube = null;
                movementSlot = null;
                return;
            }
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movementSlot.transform.position = position;
            var localPosition = movementSlot.transform.localPosition;
            localPosition.z = 0;
            if (localPosition.x > d)
            {
                localPosition.x = d;
            }
            if (localPosition.y > d)
            {
                localPosition.y = d;
            }
            if (localPosition.x < -d)
            {
                localPosition.x = -d;
            }
            if (localPosition.y < -d)
            {
                localPosition.y = -d;
            }

            movementSlot.transform.localPosition = localPosition;

            return;
        }
        if (Input.touchCount == 1)
        {

        }
        else if (Input.GetMouseButtonDown(0))
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveController
{
    protected float translation_x = 0.0f;
    protected float translation_y = 0.0f;
    protected Transform transformComponent = null;
    protected Rigidbody2D moveComponent = null;
    protected bool lockMove = false;
    protected bool fly = false;
    protected bool lockFly = false;
    protected float maxMoveSpeed = 0;
    protected float maxUpFlySpeed = 0;
    protected float maxDownFlySpeed = 0;
    public moveController()
    {

    }

    public void init(Transform aTransformComponent, Rigidbody2D aMoveComponent, float aMaxMoveSpeed, float aMaxUpFlySpeed, float aMaxDownFlySpeed)
    {
        transformComponent = aTransformComponent;
        moveComponent = aMoveComponent;
        maxMoveSpeed = aMaxMoveSpeed;
        maxUpFlySpeed = aMaxUpFlySpeed;
        maxDownFlySpeed = aMaxDownFlySpeed;
    }

    public virtual void customUpdate()
    {
        if (moveComponent.velocity.x > maxMoveSpeed)
        {
            moveComponent.velocity = new Vector2(maxMoveSpeed, moveComponent.velocity.y);
        }
        if (moveComponent.velocity.x < -maxMoveSpeed)
        {
            moveComponent.velocity = new Vector2(-maxMoveSpeed, moveComponent.velocity.y);
        }


        if (moveComponent.velocity.y > maxUpFlySpeed)
        {
            moveComponent.velocity = new Vector2(moveComponent.velocity.x, maxUpFlySpeed);
        }
        if (moveComponent.velocity.y < -maxDownFlySpeed)
        {
            moveComponent.velocity = new Vector2(moveComponent.velocity.x, -maxDownFlySpeed);
        }
    }

    public Vector2 getDirection()
    {
        return new Vector2(translation_x, translation_y);
    }

    public void updateMoveWithDirection(float acceleration)
    {
        acceleration = Time.deltaTime * acceleration;

        var speed = translation_x * acceleration;
        if ((speed > 0 && moveComponent.velocity.x < 0) ||
            (speed < 0 && moveComponent.velocity.x > 0))
        {
            moveComponent.velocity = new Vector2(0, moveComponent.velocity.y);
        }
        moveComponent.AddForce(new Vector2(speed, 0));
    }

    public void updateFly(float force)
    {
        moveComponent.velocity = new Vector2(moveComponent.velocity.x, force);
    }

    public void clearSpeedFly()
    {
        moveComponent.velocity = new Vector2(moveComponent.velocity.x, 0);
    }

    public void updateDownFly(float powerDownFly)
    {
        moveComponent.velocity = new Vector2(0, 0);
        moveComponent.AddForce(new Vector2(0, translation_y * powerDownFly));
    }

    public bool isMove()
    {
        if (moveComponent.velocity.x > 0)
        {
            return moveComponent.velocity.x > 0.1f;
        }
        return moveComponent.velocity.x < -0.1f;
    }
    public bool isFly()
    {
        return fly;
    }

    public void setLockMove(bool aLock)
    {
        lockMove = aLock;
    }
    public bool isLockMove()
    {
        return lockMove;
    }

    public void setFly(bool aFly)
    {
        fly = aFly;
    }

    public void setLockFly(bool aLock)
    {
        lockFly = aLock;
    }

    public void setLockFlyDelay(float timeForLock)
    {
        actionController.addDelay(moveComponent.gameObject, timeForLock, new callback<bool>(setLockFly, true));
    }

    public bool isLockFly()
    {
        return lockFly;
    }
}

using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class actionType
{
    public abstract void update(GameObject subject, actionValue aValue);
    public abstract void end(actionValue aValue);
    public abstract bool isEnd(GameObject subject, actionValue aValue);
}

public enum typeMove { LINARY, BEZIER };

public class actionMove : actionType
{
    protected class interactionMove
    {
        Vector2 distance;
        Vector2 bezierPoint = new Vector2();

        public interactionMove()
        {
        }
        public void initLinary(Vector2 aStartPos, Vector2 aEndPos)
        {
            distance.x = Math.Abs(aStartPos.x - aEndPos.x);
            distance.y = Math.Abs(aStartPos.y - aEndPos.y);
        }

        public void initBezier(float x, float y)
        {
            bezierPoint.x = x;
            bezierPoint.y = y;
        }

        float calculateBezierPoint(float p0, float p1, float p2, float t)
        {  //формула - (1 - t)^2 * P0 + 2 * t* (1 - t) * P1 + t^2 * P2 где t - от 0 до 1
            return (1 - t) * (1 - t) * p0 + 2 * t * (1 - t) * p1 + t * t * p2;
        }

        public Vector2 getStepPoint(Vector2 startPos, Vector2 curPos, Vector2 endPos, float progress, typeMove type)//где t это прогресс от 0 до 1
        {
            var result = new Vector2();
            switch (type)
            {
                case typeMove.LINARY:
                    {
                        var newPosX = distance.x * progress;
                        if (startPos.x > endPos.x)
                        {
                            newPosX *= -1;
                        }
                        newPosX += startPos.x;
                        var newPosY = distance.y * progress;
                        if (startPos.y > endPos.y)
                        {
                            newPosY *= -1;
                        }
                        newPosY += startPos.y;

                        result.x = newPosX;
                        result.y = newPosY;
                        break;
                    }
                case typeMove.BEZIER:
                    {

                        //var newX = calculateBezierPoint(startPos.x, bezierPoint.x, endPos.x, t);
                        //var newY = calculateBezierPoint(startPos.y, bezierPoint.y, endPos.y, t);

                        //result.x = newX;
                        //result.y = newY;
                        break;
                    }
            }

            return result;
        }
    }

    protected Vector2 startPos;
    protected Vector2 endPos;
    protected interactionMove calculaterMove = null;
    protected typeMove type = default;
    actionMove() { }

    public actionMove(Vector2 aStartPos, Vector2 aEndPos, typeMove aType)
    {
        startPos = aStartPos;
        endPos = aEndPos;
        type = aType;
        calculaterMove = new interactionMove();
        initCalculaterMove();
    }

    protected void initCalculaterMove()
    {
        switch (type)
        {
            case typeMove.LINARY:
                {
                    calculaterMove.initLinary(startPos, endPos);
                    break;
                }
            case typeMove.BEZIER:
                {//TODO нужно подумать как расчитывать точку безье для этого типа движения
                 //calculaterMove.initBezier(Vector2.Distance(aStartPos, aEndPos) * 0.5f);
                    break;
                }
        }
    }

    public override void update(GameObject subject, actionValue aValue)
    {
        if (isEnd(subject, aValue))
        {
            return;
        }
        aValue.update();
        if (aValue.getProgress() <= 0.0f)
        {
            return;
        }
        var progress = aValue.getProgress();
        var newPos = calculaterMove.getStepPoint(startPos, subject.transform.position, endPos, progress, type);
        setPos(subject, newPos);
    }

    public override void end(actionValue aValue)
    {
        aValue.end();
    }

    void setPos(GameObject subject, Vector2 pos)
    {
        subject.transform.position = new Vector3(pos.x, pos.y, subject.transform.position.z);
    }

    public override bool isEnd(GameObject subject, actionValue aValue)
    {
        if (aValue is actionTime)
        {
            var progress = aValue as actionTime;
            if (progress.isEnd())
            {
                setPos(subject, endPos);
                return true;
            }
            return false;
        }
        if (aValue is actionSpeed)
        {
            var progress = aValue as actionSpeed;
            if (progress.isEnd())
            {
                setPos(subject, endPos);
                return true;
            }
            return false;
        }
        return false;
    }
}


public class actionMoveDynamic : actionMove
{
    GameObject target = null;
    public actionMoveDynamic(Vector2 aStartPos, GameObject aTarget, typeMove aType) : base(aStartPos, aTarget.transform.position, aType)
    {
        target = aTarget;
    }

    public override void update(GameObject subject, actionValue aValue)
    {
        endPos = target.transform.position;
        initCalculaterMove();
        if (aValue is actionSpeed)
        {
            var actionProgress = aValue as actionSpeed;
            actionProgress.setDistance(Vector2.Distance(startPos, target.transform.position));
        }
        base.update(subject, aValue);
    }
}

public class actionChangeScale : actionType
{
    Vector2 startScale;
    Vector2 endScale;
    Vector2 distance;

    actionChangeScale() { }

    public actionChangeScale(Vector2 aStartScale, Vector2 aEndScale)
    {
        startScale = aStartScale;
        endScale = aEndScale;
        var distanceX = Math.Abs(startScale.x - endScale.x);
        var distanceY = Math.Abs(startScale.y - endScale.y);
        distance = new Vector2(distanceX, distanceY);
    }

    public override void end(actionValue aValue)
    {
        aValue.end();
    }

    void setScale(GameObject subject, Vector2 scale)
    {
        subject.transform.localScale = new Vector3(scale.x, scale.y, subject.transform.localScale.z);
    }

    public override bool isEnd(GameObject subject, actionValue aValue)
    {
        if (aValue is actionTime)
        {
            var time = aValue as actionTime;
            if (time.isEnd())
            {
                setScale(subject, endScale);
                return true;
            }
            return false;
        }
        return true;//actionChangeScale action working with only actionTime object 
    }

    public override void update(GameObject subject, actionValue aValue)
    {
        if (isEnd(subject, aValue))
        {
            return;
        }
        aValue.update();
        if (aValue.getProgress() <= 0.0f)
        {
            return;
        }
        var progress = aValue.getProgress();

        var newScaleX = distance.x * progress;
        if (startScale.x > endScale.x)
        {
            newScaleX *= -1;
        }
        newScaleX += startScale.x;
        var newScaleY = distance.y * progress;
        if (startScale.y > endScale.y)
        {
            newScaleY *= -1;
        }
        newScaleY += startScale.y;

        setScale(subject, new Vector2(newScaleX, newScaleY));
    }
}

public class actionDelay : actionType
{
    public override void end(actionValue aValue)
    {
        aValue.end();
    }

    public override bool isEnd(GameObject subject, actionValue aValue)
    {
        if (aValue is actionTime)
        {
            var time = aValue as actionTime;
            if (time.isEnd())
            {
                return true;
            }
            return false;
        }
        return true;//delayAction action working with only actionTime object 
    }

    public override void update(GameObject subject, actionValue aValue)
    {
        aValue.update();
    }
}

public class actionChangeAlpha : actionType
{
    float startAlpha;
    float endAlpha;
    float distance;
    public actionChangeAlpha(float aStartAlpha, float aEndAlpha)
    {
        startAlpha = aStartAlpha;
        endAlpha = aEndAlpha;
        distance = Math.Abs(endAlpha - startAlpha);
    }

    public override void end(actionValue aValue)
    {
        aValue.end();
    }

    public override bool isEnd(GameObject subject, actionValue aValue)
    {
        if (aValue is actionTime)
        {
            var time = aValue as actionTime;
            if (time.isEnd())
            {
                setAlpha(subject, endAlpha);
                return true;
            }
            return false;
        }
        return true;//delayAction action working with only actionTime object 
    }

    void setAlpha(GameObject subject, float alpha)
    {
        //if (subject.GetComponent<Button>() != null)
        //{
        //    var button = subject.GetComponent<Button>();
        //    button.colors.a = alpha;
        //}
        if (subject.GetComponent<MeshRenderer>() != null)
        {
            var render = subject.GetComponent<SkeletonAnimation>();
            var curColor = render.skeleton.GetColor();
            render.skeleton.SetColor(new Color(curColor.r, curColor.g, curColor.b, alpha));
        }
        
    }

    public override void update(GameObject subject, actionValue aValue)
    {
        aValue.update();
        if (aValue.getProgress() <= 0.0f)
        {
            return;
        }
        var progress = aValue.getProgress();
        var newAlpha = distance * progress;
        if (startAlpha > endAlpha)
        {
            newAlpha *= -1;
        }
        newAlpha += startAlpha;
        setAlpha(subject, newAlpha);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum typeSpeed { LINARY, PARABOLA, CUBIC }
abstract public class actionValue
{
    public abstract void end();

    public abstract float getProgress();

    public abstract void update();
}

public class actionTime : actionValue
{
    float fullTime;
    float time;
    actionTime() { }
    public actionTime(float aTime) { time = aTime; fullTime = aTime; }
    public float getTime() { return time; }
    public override float getProgress() { return (fullTime - time) / fullTime; }
    public float getFullTime() { return fullTime; }
    public override void update() { time -= Time.deltaTime; }
    public bool isEnd() { if (time <= 0.0f) { return true; } return false; }
    public override void end() { time = 0.0f; }
}

public class actionSpeed : actionValue
{
    float maxSpeed;//in second
    float minSpeed; //in second
    //the default parameter is maxSpeed - together these parameters(maxSpeed and minSpeed) can only be used in complex motion calculations

    float distance;// full distance need for calculate progress
    float spentTime;// how much spented

    float spentTimeCustom;//can only be used in complex motion calculations
    float progress;//can only be used in complex motion calculations
    typeSpeed type = typeSpeed.LINARY;
    actionSpeed() { }

    public actionSpeed(float aMaxSpeed, float aMinSpeed, float aDistance, typeSpeed aType) { maxSpeed = aMaxSpeed; minSpeed = aMinSpeed; distance = aDistance; spentTime = 0.0f; type = aType; progress = 0.0f; spentTimeCustom = 0.0f; }

    public float getSpeed() 
    { 
        return maxSpeed; 
    }

    public void setDistance(float aDistance)// for dynamic positions need be set new distance 
    {
        distance = aDistance;
    }

    public override void update() 
    {
        spentTime += Time.deltaTime;
        spentTimeCustom += Time.deltaTime;
    }
    public bool isEnd() 
    {
        var result = getProgress();
        if (result >= 1.0f)
        {
            return true;
        }
        return false;
    }
    public override void end() 
    {
        spentTime = distance / maxSpeed;
    }

    public override float getProgress() 
    {
        var y = 0.0f;
        switch (type)
        {
            case typeSpeed.LINARY:
                {
                    var x = maxSpeed * spentTime;
                    y = x / distance;
                    break;
                }
            case typeSpeed.CUBIC:
                {
                    var x = maxSpeed * spentTime;
                    y = Mathf.Pow(x, 2) / distance;
                    break;
                }
            case typeSpeed.PARABOLA:
                {
                    var speedDiff = maxSpeed - minSpeed;
                    var result = progress / distance;
                    if (result < 0.5f)
                    {
                        speedDiff *= result;
                    }
                    else
                    {
                        speedDiff *= 1.0f - result;
                    }
                    progress += spentTimeCustom * (minSpeed + speedDiff);
                    spentTimeCustom = 0;
                    y = progress / distance;
                    break;
                }
        }
      
        if (y > 1.0f)
        {
            y = 1.0f;
        }
        return y;
    }
}
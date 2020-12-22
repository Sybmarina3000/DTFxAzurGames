using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class liquid
{
    public enum typeLiquid { poison, water, empty };

    public typeLiquid currentType = typeLiquid.empty;
    GameObject currentObject = null;
    ObiParticleRenderer currentEmmiter = null;

    public void mix(liquid input)
    {
        switch (currentType)
        {
            case typeLiquid.water:
                {
                    if (input.currentType == typeLiquid.poison)
                    {
                        currentType = typeLiquid.poison;
                    }
                    break;
                }
            case typeLiquid.poison:
                {
                    if (input.currentType == typeLiquid.water)
                    {
                        currentType = typeLiquid.poison;
                    }
                    break;
                }
            case typeLiquid.empty:
                {
                    currentType = input.currentType;
                    break;
                }
        }
    }

    public void outStart(GameObject target)
    {
        currentObject = target;
        switch (currentType)
        {
            case typeLiquid.water:
                {
                    var gObject = target.transform.Find("water").gameObject;
                    if (gObject)
                    { 
                        currentEmmiter = gObject.GetComponent<ObiParticleRenderer>();
                    }
                    break;
                }
            case typeLiquid.poison:
                {
                    var gObject = target.transform.Find("poison").gameObject;
                    if (gObject)
                    {
                        currentEmmiter = gObject.GetComponent<ObiParticleRenderer>();
                    }
                    break;
                }
        }
        if (!currentEmmiter)
        {
            return;
        }

        ObiFluidRenderer component = null;
        foreach (var camera in Camera.allCameras)
        {
            if (camera.gameObject.name == "obiCamera")
            {
                component = camera.gameObject.gameObject.GetComponent<ObiFluidRenderer>();
                break;
            }
        }
        if (!component)
        {
            return;
        }
        var particles = component.particleRenderers;
        bool haveParticle = false;
        foreach (var oldParticles in particles)
        {
            if (currentEmmiter == oldParticles)
            {
                haveParticle = true;
                break;
            }
        }
        if (!haveParticle)
        {
            component.particleRenderers = new ObiParticleRenderer[particles.Length + 1];
            for (int n = 0; n < particles.Length; n++)
            {
                component.particleRenderers[n] = particles[n];
            }
            component.particleRenderers[particles.Length] = currentEmmiter;
            currentEmmiter.gameObject.SetActive(true);
        }
        setSpeedOut(worldSettings.speedLiquidEmmiters);
    }

    public void outStop()
    {
        if (!currentEmmiter)
        {
            return;
        }
        var component = Camera.main.gameObject.GetComponent<ObiFluidRenderer>();
        var particles = component.particleRenderers;
        component.particleRenderers = new ObiParticleRenderer[particles.Length - 1];
        int currentIndex = 0;
        foreach (var particle in particles)
        {
            if (particle == currentEmmiter)
            {
                continue;
            }
            component.particleRenderers[currentIndex] = particle;
            currentIndex++;
        }
        currentEmmiter.gameObject.SetActive(false);
        currentObject = null;
        currentEmmiter = null;
    }

    public bool isStart()
    {
        return currentEmmiter != null;
    }

    public int getCountActiveParticle()
    {
        int result = 0;
        if (currentEmmiter)
        {
            var gObject = currentObject.transform.Find("water").gameObject;
            if (gObject)
            {
                result += gObject.GetComponent<ObiEmitter>().activeParticleCount;
            }


            gObject = currentObject.transform.Find("poison").gameObject;
            if (gObject)
            {
                result += gObject.GetComponent<ObiEmitter>().activeParticleCount;
            }
           
        }
        return result;
    }

    public void setSpeedOut(int speed)
    {
        currentEmmiter.GetComponent<ObiEmitter>().speed = speed;
    }

    public float getSpeedOut()
    {
        return currentEmmiter.GetComponent<ObiEmitter>().speed;
    }
}

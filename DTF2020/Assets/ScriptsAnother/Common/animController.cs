using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine;
using Spine.Unity;
using System;
using System.IO;
using System.Globalization;


// componentSkeletonAnimation.AnimationState.GetCurrent(0).mix;
public class animController
{
    enum state { none = 99, idle = 0, run = 1, dead = 2, damage = 3, custom = 4 };//dodge
    int state_log_custom = (int)state.custom;
    int state_ani_custom = 0;

    [Serializable]
    public class mixData
    {
        public string animFrom;
        public string animTo;
        public float time;
    }

    [Serializable]
    public class animData
    {
        public string animName;
        public float animSpeed;
        public animData(string aName)
        {
            animName = aName; animSpeed = 1.0f;
        }
    }

    protected SkeletonAnimation componentSkeletonAnimation = null;
    protected MeshRenderer componentMeshRenderer = null;

    string currentAnimName = "";

    protected animData[] pullAnim;//Контейнер обычных анимаций
    protected mixData[] mixProperty; //Контейнер информации о миксе анимаций
    public float defaultMixTime = 0.2f;
    state state_log = state.none;//тип который должен быть
    state state_ani = state.none;//тип который сейчас

    float inverModelScale = 0;//сохраненный скейл подели по X для поделий с инвертированным скейлом

    public animController()
    {

    }

    public void init(GameObject model, string dirAnimConfig, string dirAnimMixConfig)
    {
        componentSkeletonAnimation = model.GetComponent<SkeletonAnimation>();
        componentMeshRenderer = model.GetComponent<MeshRenderer>();
        inverModelScale = componentSkeletonAnimation.gameObject.transform.localScale.x;
        loadAnimData(dirAnimConfig, dirAnimMixConfig);
    }

    public bool TrackIsComplete()
    {
        return componentSkeletonAnimation.AnimationState.GetCurrent(0).IsComplete;
    }

    public void setMove()
    {
        state_log = state.run;
    }

    public bool isMove()
    {
        return state_ani == state.run;
    }

    public void setIdle()
    {
        state_log = state.idle;
    }

    public void setDamage()
    {
        state_log = state.damage;
    }

    public bool isIdle()
    {
        return state_ani == state.idle;
    }

    public void setDead()
    {
        state_log = state.dead;
    }

    public void setRandom()
    {
        int startIndex = (int)state.custom;
        int maxIndex = pullAnim.Length;
        while (state_log_custom == state_ani_custom)
        { 
            state_log_custom = UnityEngine.Random.Range(startIndex, maxIndex);
        }
        state_log = state.custom;
    }

    public bool isCustom()
    {
        return state_ani == state.custom;
    }

    protected void setCustom(bool result)
    {
        if (result)
        {
            state_log = state.custom;
            state_ani = state.custom;
        }
        else
        {
            state_log = state.none;
            state_ani = state.none;
        }
    }


    virtual protected animData[] getPullAnim()
    {
        return pullAnim;
    }

    float getMixTime(string nextAnim)
    {
        for (int n = 0; n < mixProperty.Length; n++)
        {
            var curProperty = mixProperty[n];
            if (curProperty.animFrom == currentAnimName && curProperty.animTo == nextAnim)
            {
                return curProperty.time;
            }
        }
        return defaultMixTime;
    }

    protected TrackEntry setAnim(int indexAnim, bool repeat, int track = 0, string prefix = "", bool add = false, float delay = 0.0f)
    {
        TrackEntry result = null;
        if (getPullAnim().Length <= indexAnim)
        {
            return result;
        }
        string nextAnimName = "";

        nextAnimName = getPullAnim()[indexAnim].animName + prefix;
        if (add)
        {
            result = componentSkeletonAnimation.AnimationState.AddAnimation(track, nextAnimName, repeat, delay);
        }
        else
        {
            result = componentSkeletonAnimation.AnimationState.SetAnimation(track, nextAnimName, repeat);
        }
        result.TimeScale = getPullAnim()[indexAnim].animSpeed;

        result.MixDuration = getMixTime(nextAnimName);
        currentAnimName = nextAnimName;
        return result;
    }

    protected void Update() { }

    public void customUpdate()
    {
        if (state_log != state_ani)
        {
            switch (state_log)
            {
                case state.idle:
                    {
                        setAnim((int)state_log, true);
                        state_ani = state_log;
                        break;
                    }
                case state.run:
                    {
                        setAnim((int)state_log, true);
                        state_ani = state_log;
                        break;
                    }
                case state.damage:
                    {
                        setAnim((int)state_log, true);
                        state_ani = state_log;
                        break;
                    }
                case state.dead:
                    {
                        componentSkeletonAnimation.AnimationState.SetAnimation(0, getPullAnim()[(int)state_log].animName, false);
                        state_ani = state_log;
                        break;
                    }
                case state.custom:
                    {        
                        state_ani = state_log;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return;
        }
        if (state_log == state.custom)
        {
            if (state_log_custom != state_ani_custom)
            {
                if (getPullAnim().Length > state_log_custom)
                {
                    componentSkeletonAnimation.AnimationState.SetAnimation(0, getPullAnim()[state_log_custom].animName, true);
                }
            }
            state_ani_custom = state_log_custom;
        }
    }

    protected void loadAnimData(string dirAnimConfig, string dirAnimMixConfig)
    {
        pullAnim = utilFunction.loadArrayData<animData>(dirAnimConfig);
        mixProperty = utilFunction.loadArrayData<mixData>(dirAnimMixConfig);
    }
}

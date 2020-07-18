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
    enum state { none = 99, idle = 0, run = 1, dead = 2, custom = 3 };//dodge


    [Serializable]
    public class mixData {
        public string animFrom;
        public string animTo;
        public float time;
    }

    [Serializable]
    public class animData {
        public string side;
        public string front;
        public string back;
        public float sideSpeed;
        public float frontSpeed;
        public float backSpeed;
        public animData(string aSide, string aFront, string aBack)
        {
            side = aSide; front = aFront; back = aBack; sideSpeed = 1.0f; frontSpeed = 1.0f; backSpeed = 1.0f;
        }
    }

    protected SkeletonAnimation componentSkeletonAnimation = null;
    protected MeshRenderer componentMeshRenderer = null;
    
    //public string idle_nickH = "Side_Idle";
    // public string run_nickH = "Side_Run";
    //public List<List<string>> attack_nickH = new List<List<string>> { new List<string> { "Side_Attack", "Side_Attack2", "Side_Attack3", "Side_Attack4" }  };
    // public string death_nickH = "Side_Death";

    // public string[] idle_nickV = new string[2] { "Back_Idle", "Front_Idle"  };
    // public string[] run_nickV = new string[2] { "Back_Run", "Front_Run" };
    //1 - виды(передний, задний) 2 - тип атаки, 3- список названий анимаций
    //public List<List<List<string>>> attack_nickV = new List<List<List<string>>> {
    //    new List<List<string>> {
    //        new List<string> { "Front_Attack1", "Front_Attack2", "Front_Attack3", "Front_Attack4" } 
    //                           }, 
    //    new List<List<string>> { 
    //        new List<string> { "Back_Attack1", "Back_Attack2", "Back_Attack3", "Back_Attack4" } 
    //                           } 
    //};
    //public string death_nickV = "Front_Death";
    //public float[] damage_delay_combo_time = new float[4] { 0.1f, 0.1f, 0.1f, 0.1f };
    //public float[] damage_delay_combo_damage = new float[4] { 0.2f, 0.2f, 0.2f, 0.2f };
    //public bool punch_complete = false;//следит чтобы наносился всего лишь один удар

    string currentAnimName = "";

    protected animData[] pullAnim;//Контейнер обычных анимаций
    protected mixData[] mixProperty; //Контейнер информации о миксе анимаций
    public float defaultMixTime = 0.2f;
    state state_log = state.none;//тип который должен быть
    state state_ani = state.none;//тип который сейчас

    protected bool flip = false;// поворачивает модельку в виде сбоку
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

    //rotate model from dir
    public void dirToBool(Vector2 dir) {
        flip = dir.x <= 0;
    }

    public void setMove(Vector2 dir) {
        dirToBool(dir);
        state_log = state.run;
    }

    public bool isMove()
    {
        return state_ani == state.run;
    }

    public void setIdle(Vector2 dir) {
        dirToBool(dir);
        state_log = state.idle;
    }

    public void setIdle()
    {
        state_log = state.idle;
    }

    public bool isIdle()
    {
        return state_ani == state.idle;
    }

    public void setDead() {
        state_log = state.dead;
    }

    public void callAttackAnim()
    {
        componentSkeletonAnimation.AnimationState.SetAnimation(1, "Damage", false);
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
        for (int n =0; n < mixProperty.Length; n++) 
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

        nextAnimName = getPullAnim()[indexAnim].side + prefix;
        if (add) {
            result = componentSkeletonAnimation.AnimationState.AddAnimation(track, nextAnimName, repeat, delay); 
        }
        else { 
            result = componentSkeletonAnimation.AnimationState.SetAnimation(track, nextAnimName, repeat); 
        }
        result.TimeScale = getPullAnim()[indexAnim].sideSpeed;
        
        result.MixDuration = getMixTime(nextAnimName);
        currentAnimName = nextAnimName;
        return result;
    }

    protected void Update() { }

    public void customUpdate()
    {
        componentSkeletonAnimation.gameObject.transform.localScale = new Vector3(flip ? inverModelScale * -1 : inverModelScale, componentSkeletonAnimation.gameObject.transform.localScale.y, 1);

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
                case state.dead:
                    {
                        componentSkeletonAnimation.AnimationState.SetAnimation(0, getPullAnim()[(int)state_log].front, false);
                        state_ani = state_log;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    protected void loadAnimData(string dirAnimConfig, string dirAnimMixConfig)
    {
        pullAnim = utilFunction.loadArrayData<animData>(dirAnimConfig);
        mixProperty = utilFunction.loadArrayData<mixData>(dirAnimMixConfig);
    }



    //protected void saveFile(string data)
    //{
    //    if (!Directory.Exists("Settings"))
    //    {
    //        Directory.CreateDirectory("Settings");
    //    }
    //    File.WriteAllText("Settings/" + nameConfig + ".json", data);
    //}


    //virtual protected void saveAnimData()
    //{
    //    string jsonData = JsonLoader.ToJson(pullAnim, true);
    //    saveFile(jsonData);
    //}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats
{
    public Stats() 
    {

    }

    [Serializable]
    public class statBlock
    {
        public int hp = 0;
        public float moveAcceleration = 0;
        public float maxSpeed = 0;
        public int baseDamage = 0;
        public float flyAcceleration = 0;
        public float flyTime = 0;
        public float maxSpeedUpFly = 0;
        public float maxSpeedDownFly = 0;
    }

    public float fullHp = 0;//FULL HP
    float curHp = 0f;//текущее HP
    public float moveAcceleration = 0;
    public float maxSpeed = 10;//скорость
    public float flyAcceleration = 0;
    public float flyTime = 0;
    public float maxSpeedUpFly = 0;
    public float maxSpeedDownFly = 0;
    public float baseDamage = 0;//базовый урон


    public void init(string dirStatConfig)
    {
        statBlock database = new statBlock();
        database = utilFunction.loadData<statBlock>(dirStatConfig);

        curHp = fullHp = database.hp;
        moveAcceleration = database.moveAcceleration;
        maxSpeed = database.maxSpeed;
        flyAcceleration = database.flyAcceleration;
        flyTime = database.flyTime;
        maxSpeedUpFly = database.maxSpeedUpFly;
        maxSpeedDownFly = database.maxSpeedDownFly;
        baseDamage = database.baseDamage;
    }
    public void changeHp(float value)
    {
        curHp -= value;
        //if (componentSlider != null)
        //{ 
        //    componentSlider.value = cur_hp; 
        //}
        //if (componentImage != null)
        //{
        //    componentImage.fillAmount = cur_hp / full_hp;
        //}
    }

    public bool isDead()
    {
        if (curHp < 0)
            return true;
        return false;
    }

    //public float time_dodge = 0.5f;//время в уклонении
    //public float coef_dodge = 5.0f;//сила смещения в уклонении
    // public float[] damage_delay = new float[4];//задержа до урона
    //public GameObject image_hp = null;
    //public GameObject slider_hp = null;
    //Slider componentSlider = null;
    //Image componentImage = null;

    // Start is called before the first frame update
    //void Start()
    //{
    //    cur_hp = full_hp;
    //    if (slider_hp != null)
    //    {
    //        componentSlider = slider_hp.GetComponent<Slider>();
    //        componentSlider.maxValue = full_hp;
    //        componentSlider.value = cur_hp;
    //    }
    //    if (image_hp != null)
    //    {
    //        componentImage = image_hp.GetComponent<Image>();
    //        componentImage.fillAmount = 1;
    //    }
    //}
}

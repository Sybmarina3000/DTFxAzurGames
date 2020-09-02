using System;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, actionObject
{
    public event Action onSpawnInScene;
    
    public UnityEngine.UI.Text debugText;
    bool slowMode = false;
    float force = 1800.0f;
    Rigidbody2D moveController = null;
    public GameObject spineObject;
    public GameObject sirenObject;
    animController animModule;
    animController animSirenModule;
    public string dirConfigAnim;
    public string dirConfigAnimMix;

    public string dirConfigAnimSiren;
    public string dirConfigAnimSirenMix;

    private bool _isFirstCollision = true; // with floor, after spawn;

    public FMODUnity.StudioGlobalParameterTrigger triggerSlouMoSound;
    public FMODUnity.StudioGlobalParameterTrigger triggerGlobalState;

    // Start is called before the first frame update
    void Start()
    {
      // Invoke( "startPlay", 1.0f);
        //trigger.SendMessage("game_state", 2);
    }

    public void OnEnable()
    {
        startPlay();
    }

    public void startPlay()
    {
        gameObject.SetActive(true);
        initActionObject();
        colliderFactory.getCollideFromParameters("box", new List<float> { -0.04939353f, 0.06585872f, 1.332784f, 2.155998f }, false, gameObject);
        moveController = GetComponent<Rigidbody2D>();
       animModule = new animController();
       animSirenModule = new animController();
       animModule.init(spineObject, dirConfigAnim, dirConfigAnimMix);
       animModule.setIdle();
       animSirenModule.init(sirenObject, dirConfigAnimSiren, dirConfigAnimSirenMix);
        Physics2D.autoSimulation = true;
       triggerGlobalState.value = 2;
       triggerGlobalState.TriggerParameters();
       DefaultNamespace.RealizeBox.instance.score.StartDecreaseScore();
    }

    //~playerController()
    //{
    //    destroyActionObject();
    //}

    // Update is called once per frame
    void Update()
    {
      //  animModule.customUpdate();
     //   animSirenModule.customUpdate();
        if (Input.GetKeyDown(KeyCode.F3))
        {
            slowMode = !slowMode;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            jump(new Vector2(0, 1), false);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            DefaultNamespace.RealizeBox.instance.manager.OnWin();
            sirenObject.SetActive(true);
            animSirenModule.setDead();
            Physics2D.autoSimulation = false;
            triggerGlobalState.value = 4;
            triggerGlobalState.TriggerParameters();
        }
        if (slowMode)
        {
            float slowSpeed = DefaultNamespace.RealizeBox.instance.manager.database.coefSlow;
            if (Time.timeScale > slowSpeed)
            {
                Time.timeScale -= Time.fixedDeltaTime * DefaultNamespace.RealizeBox.instance.manager.database.speedSlowModeOn;
                Time.fixedDeltaTime = Time.fixedDeltaTime = Time.timeScale * .02f;
            }
            else
            {
                Time.timeScale = slowSpeed;
            }
            Time.fixedDeltaTime = Time.timeScale * .02f;
            debugText.text = Time.timeScale.ToString();
        }
        else
        {
         setNormalTime();
        }
    }

    void setNormalTime()
    {
        float normalSpeed = DefaultNamespace.RealizeBox.instance.manager.database.coefNormal;
        Time.timeScale = normalSpeed;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        debugText.text = Time.timeScale.ToString();
        triggerSlouMoSound.value = 0;
        triggerSlouMoSound.TriggerParameters();
    }

    //Time.deltaTime * Time.scaleTime
    //Time.deltaTime * 0.2 = x
    //x = 0.2Time.deltatime
    //x/0.2 = Time.deltaTime


    public void jump(Vector2 direction, bool ground)
    {
        setNormalTime();
        moveController.velocity = new Vector2(0, 0);
        if (ground)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/jump_2", transform.position);
            moveController.AddForce(new Vector2(direction.x * DefaultNamespace.RealizeBox.instance.manager.database.powerJumpGround, direction.y * DefaultNamespace.RealizeBox.instance.manager.database.powerJumpGround));
        }
        else
        { 
            moveController.AddForce(new Vector2 (direction.x * DefaultNamespace.RealizeBox.instance.manager.database.powerJumpFly, direction.y * DefaultNamespace.RealizeBox.instance.manager.database.powerJumpFly));
        }
        animModule.setMove();
    }

    public void initActionObject()
    {
        actionController.init(gameObject, new List<GameObject> { gameObject });
    }

    public void destroyActionObject()
    {
        actionController.remove(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            actionController.clearActions(gameObject);
            DefaultNamespace.RealizeBox.instance.touchController.startJump(true);
            slowMode = false;
        }
        if (_isFirstCollision)
        {
            _isFirstCollision = false;
            onSpawnInScene?.Invoke();
        }
        animModule.setIdle();
        // надо добавить задержку, чтобы он стартовал не сразу
        // TODO непонятно надо ли его двигать. вопрос к ГД. пока так не делаем
        /*actionController.addMoveToFromPosAndSpeed(gameObject,
            new Vector2(gameObject.transform.position.x, gameObject.transform.position.y),
            new Vector2(gameObject.transform.position.x + 9999.0f, gameObject.transform.position.y),
            typeMove.LINARY,
            3,
            5,
            typeSpeed.CUBIC);*/
    }

    public void nextStepSlowMode()
    {
        actionController.clearActions(gameObject);
        setNormalTime();

        actionController.addDelay(gameObject, DefaultNamespace.RealizeBox.instance.manager.database.timeBeforeNewSlowMode - 0.25f, new callback(new callbackFunc(animModule.setRandom)));
        actionController.addDelay(gameObject, 0.25f, new callback(new callbackFunc(startSlowMode)));
        Debug.Log("jumpNext");
    }

    public void startSlowMode()
    {
        actionController.clearActions(gameObject);
        slowMode = true;
        DefaultNamespace.RealizeBox.instance.touchController.startJump();
        var secondCallback = new callback<bool>(new callbackFunc<bool>(setSlowMode), false);
        var firstCallback = new callback(new callbackFunc(DefaultNamespace.RealizeBox.instance.touchController.endJump), secondCallback);
        actionController.addDelay(gameObject, DefaultNamespace.RealizeBox.instance.manager.database.timeSlowMode, firstCallback);
        triggerSlouMoSound.value = 1;
        triggerSlouMoSound.TriggerParameters();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "point")
        {
            if (DefaultNamespace.RealizeBox.instance.level.getEndPoint().GetComponent<Collider2D>() == collision)
            {
                DefaultNamespace.RealizeBox.instance.manager.OnWin();
                sirenObject.SetActive(true);
                animSirenModule.setDead();
                Physics2D.autoSimulation = false;
                triggerGlobalState.value = 4;
                triggerGlobalState.TriggerParameters();
            }
        }
    }

    public void setSlowMode(bool slow)
    {
        slowMode = slow;
    }

    public void endAnim()
    {
        actionController.clearActions(gameObject);
        animModule.setDead();
        sirenObject.SetActive(true);
        animSirenModule.setDead();
        triggerGlobalState.value = 3;
        triggerGlobalState.TriggerParameters();
    }

    public bool isSlowMode()
    {
        return slowMode;
    }
}

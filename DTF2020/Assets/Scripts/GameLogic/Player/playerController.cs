using System;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, actionObject
{
    public event Action onSpawnInScene;
    
    public UnityEngine.UI.Text debugText;
    bool slowMode = false;
    float force = 600.0f;
    Rigidbody2D moveController = null;
    public GameObject spineObject;
    animController animModule;
    public string dirConfigAnim;
    public string dirConfigAnimMix;

    private bool _isFirstCollision = true; // with floor, after spawn;
    
    // Start is called before the first frame update
    void Start()
    {
        initActionObject();
        colliderFactory.getCollideFromParameters("box", new List<float> { 0, 0,4f,4f }, false, gameObject);
        moveController = GetComponent<Rigidbody2D>();
        animModule = new animController();
        animModule.init(spineObject, dirConfigAnim, dirConfigAnimMix);
        animModule.setIdle();
    }

    ~playerController()
    {
        destroyActionObject();
    }

    // Update is called once per frame
    void Update()
    {
        animModule.customUpdate();
        if (Input.GetKeyDown(KeyCode.F3))
        {
            slowMode = !slowMode;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            jump(new Vector2(0, 1));
        }
        if (slowMode)
        {
            float slowSpeed = 0.25f;
            if (Time.timeScale > slowSpeed)
            {
                Time.timeScale -= Time.deltaTime * 4;
            }
            else
            {
                Time.timeScale = slowSpeed;
            }
            debugText.text = Time.timeScale.ToString();
        }
        else
        {
            float normalSpeed = 1.0f;
            if (Time.timeScale < normalSpeed)
            {
                Time.timeScale += Time.deltaTime * 5;
            }
            else
            {
                Time.timeScale = normalSpeed;
            }
            debugText.text = Time.timeScale.ToString();
        }
    }

    public void jump(Vector2 direction)
    {
        moveController.velocity = new Vector2(0, 0);
        moveController.AddForce(new Vector2 (direction.x * force, direction.y * force));
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        actionController.clearActions(gameObject);
    }

    public void setSlowMode(bool slow)
    {
        slowMode = slow;
    }

    public bool isSlowMode()
    {
        return slowMode;
    }
}

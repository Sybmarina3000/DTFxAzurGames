using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameEvents : MonoBehaviour
{
    public Slider slider;
    public GameObject gameUI;
    public GameObject loseUI;
    public GameObject winUI;
    public playerController controller = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameOver()
    {
        lockController();
        loseUI.SetActive(true);
        gameUI.SetActive(false);
    }

    public void win()
    {
        lockController();
        winUI.SetActive(true);
        gameUI.SetActive(false);
    }

    public void lockController()
    {
        controller.enabled = false;
        slider.GetComponent<timeToDeath>().startGame = false;
        slider.gameObject.SetActive(false);
    }
}

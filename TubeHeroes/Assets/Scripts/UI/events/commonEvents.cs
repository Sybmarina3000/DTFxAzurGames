using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class commonEvents : MonoBehaviour
{

    public void loadScene(string sceneName)
    {
        actionController.removeAll();
        SceneManager.LoadScene(sceneName);
    }

    public void endGame()
    {
        Application.Quit();
    }

    public void clickButton()
    {
        
    }
}

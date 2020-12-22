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

    public void reloadScene()
    {
        actionController.removeAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void endGame()
    {
        Application.Quit();
    }

    public void showElement(GameObject element)
    {
        element.SetActive(true);
    }

    public void hideElement(GameObject element)
    {
        element.SetActive(false);
    }

    public void lockButton(GameObject element)
    {
        var button = element.GetComponent<bounceButton>();
        if (button)
        {
            button.enabled = false;
        }
    }

    public void unlockButton(GameObject element)
    {
        var button = element.GetComponent<bounceButton>();
        if (button)
        {
            button.enabled = true;
        }
    }
}

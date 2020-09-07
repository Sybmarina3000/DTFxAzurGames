using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public int currentLvl;

    [SerializeField] private TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("currentLvl") == false)
        {
            currentLvl = 0;
        }
        else
        {
            currentLvl = PlayerPrefs.GetInt("currentLvl");
        }

        text.text = (currentLvl + 1).ToString() + " level";
    }

    // Update is called once per frame
    public void StartPlay()
    {
        AppMetrica.Instance.ReportEvent("level_start", new System.Collections.Generic.Dictionary<string, object> {{ "level", currentLvl }});
        SceneManager.LoadScene("level" + ((currentLvl % 13) + 1)); 
    }
}

using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerLoadPrefs : MonoBehaviourSingleton<PlayerLoadPrefs>
    {
        public static int currentSessionLvl = 0;
        public static int currentStars;
        
        public static int GetCurrentLevel()
        {
            if (PlayerPrefs.HasKey("currentLvl") == false)
            {
                Debug.Log("notSave");
                return 0;
            }
            else
            {
                Debug.Log("have save");
                return PlayerPrefs.GetInt("currentLvl");
            }
        }

        public static int GetCountStars(int lvl)
        {
            if (PlayerPrefs.HasKey("currentLvl") == false)
            {
                return 0;
            }
            else if (PlayerPrefs.HasKey("star" + lvl) == false)
            {
                return 0;
            }
            else
            {
                return PlayerPrefs.GetInt("star" + lvl);
            }
        }
        
        public static void SaveCurrentLevelProgress()
        {
            if (PlayerPrefs.HasKey("currentLvl") == false)
            {
                PlayerPrefs.SetInt("currentLvl", currentSessionLvl+ 1);
                PlayerPrefs.SetInt("star" + currentSessionLvl, currentStars);
                Debug.Log("Save lvl & stars");
            }
            else
            {
                var saveLvl = PlayerPrefs.GetInt("currentLvl");

                if (saveLvl < currentSessionLvl + 1)
                {
                    PlayerPrefs.SetInt("currentLvl", currentSessionLvl + 1);
                    PlayerPrefs.SetInt("star" + currentSessionLvl, currentStars);
                    Debug.Log("Save lvl");
                }
                else
                {
                    if(currentStars > PlayerPrefs.GetInt("star" + currentSessionLvl))
                        PlayerPrefs.SetInt("star" + currentSessionLvl, currentStars);  
                }
            }
        }
    }
}
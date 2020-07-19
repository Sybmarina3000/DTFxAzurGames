using UnityEngine;

namespace DefaultNamespace.UI
{
    public class LevelUiLoader : MonoBehaviour
    {
        [SerializeField] private LvlUi[] LvlButton;
        
        private void Start()
        {
            InitLevelUi();
        }

        private void InitLevelUi()
        {
            var currLvl = PlayerLoadPrefs.GetCurrentLevel();

            for (var i = 0; i < LvlButton.Length; i++)
            {
                int stars = PlayerLoadPrefs.GetCountStars(i);
                LvlButton[i].LoadLvl( new lvl(stars,  i < (currLvl + 1) ? false: true), i);
            }
        }
    }
}
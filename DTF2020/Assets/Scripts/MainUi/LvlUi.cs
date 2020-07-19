using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class LvlUi : MonoBehaviour
    {
        private int _lvlNum;
        
        [SerializeField] private Toggle[] _stars;
        
        [SerializeField] GameObject _lock;

        [SerializeField] private GameObject _number;

        [SerializeField] private Button _button;

        [SerializeField] private lvl testLVL;
        
        [SerializeField] private string nameLvl;

        
        public void LoadLvl(lvl info, int num)
        {
            _lvlNum = num;
            
            if (info.lockState)
            {
                _lock.SetActive(true);
                _number.SetActive(false);

                foreach (var star in _stars)
                {
                    star.isOn = false;
                }
                _button.enabled = false;
            }
            else
            {
                _lock.SetActive(false);
                _number.SetActive(true);

                for(int i=0; i< _stars.Length; i++)
                {
                    if(i<info.countStars)
                        _stars[i].isOn = true;
                    else
                        _stars[i].isOn = false;
                }
                _button.enabled = true;
            }
        }

        public void StartLvl()
        {
            PlayerLoadPrefs.currentSessionLvl = _lvlNum;
            SceneManager.LoadScene(nameLvl);
        }
        
        public void setLvl()
        {
            LoadLvl(testLVL, 0);
        }
    }
}
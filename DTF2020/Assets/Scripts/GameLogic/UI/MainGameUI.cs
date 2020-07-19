using UnityEngine;

namespace DefaultNamespace.UI
{
    public class MainGameUI : MonoBehaviour
    {
        [SerializeField] private GameObject _gameUi;
        [SerializeField] private GameObject _victoryUi;
        [SerializeField] private GameObject _defeatUi;
        
        private void Start()
        {
        }

        public void ShowGameUi()
        {
            _gameUi.SetActive(true);
        }
        
        public void HideGameUi()
        {
            _gameUi.SetActive(false);
        }

        public void ShowVictoryWindow()
        {
            _victoryUi.SetActive(true);
        }
        
        public void ShowDefeatWindow()
        {
            _defeatUi.SetActive(true);
        }
    }
}
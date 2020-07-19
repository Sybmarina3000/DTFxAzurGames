using UnityEngine;

namespace DefaultNamespace.UI
{
    public class MainGameUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _gameUi;
        [SerializeField] private GameObject _victoryUi;
        [SerializeField] private GameObject _defeatUi;
        
        private void Start()
        {
        }

        public void ShowGameUi()
        {
            _gameUi.alpha = 1;
        }
        
        public void HideGameUi()
        {
            _gameUi.alpha = 0;
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
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class MainGameUI : MonoBehaviour
    {
        [SerializeField] private GameObject _gameUi;
        
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
    }
}
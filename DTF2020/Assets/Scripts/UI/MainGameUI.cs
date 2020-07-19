using UnityEngine;

namespace DefaultNamespace.UI
{
    public class MainGameUI : MonoBehaviour
    {
        [SerializeField] private GameObject _gameUi;
        
        private void Start()
        {
            RealizeBox.instance.player.onSpawnInScene += ShowGameUi;
        }

        private void ShowGameUi()
        {
            _gameUi.SetActive(true);
        }
        
        private void HideGameUi()
        {
            _gameUi.SetActive(false);
        }
    }
}
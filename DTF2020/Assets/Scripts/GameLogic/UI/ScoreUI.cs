using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _filler;

        private void Start()
        {
            RealizeBox.instance.score.onUpdateValue += OnUpdateScore;
        }

        private void OnUpdateScore(float amount)
        {
            _text.text = ((int)amount).ToString();
            _filler.fillAmount = amount / 60.0f;
        }
        
    }
}
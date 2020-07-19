using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _filler;

        [Space(10)]
        [SerializeField] private Color _simpleColor;
        [SerializeField] private Color _damageColor;
        
        [SerializeField]
        private float _timeDamageFillDuration;

        private bool _flagAnimationDamage = false;

        private Sequence _colorAnimation;
        
        private void Start()
        {
            RealizeBox.instance.score.onUpdateValue += OnUpdateScore;

            _colorAnimation = DOTween.Sequence().SetAutoKill(false).Pause();
            
            _colorAnimation
                .Append(_filler.DOColor(_damageColor, 0.1f))
                .AppendInterval(1f)
                .Append(_filler.DOColor(_simpleColor, 0.1f));
            
        }

        private void OnUpdateScore(float amount, ScoreDecreaseType type)
        {
            if (type == ScoreDecreaseType.Damage || !_flagAnimationDamage)
            {
                _text.text = ((int) amount).ToString();

                if (type == ScoreDecreaseType.Damage)
                {
                    _flagAnimationDamage = true;
                    
                    _filler.DOKill();
                    
                    _colorAnimation.Restart();
                    
                    _filler.DOFillAmount(amount / 60.0f, _timeDamageFillDuration).OnComplete(() => _flagAnimationDamage = false);
                }
                else
                    _filler.fillAmount = amount / 60.0f;
            }
        }
        
    }
}
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class VictoryPanel : MonoBehaviour
    {
        [SerializeField] private float _timeSpot;
        [SerializeField] private float _timeButton;

        [SerializeField] private float _timeIdleButton;
    
        [SerializeField] private CanvasGroup _shadowPers;
        [SerializeField] private RectTransform _spot;
    
        [SerializeField] private RectTransform _buttonCont;
        [SerializeField] private RectTransform _buttonRep;
        
        [SerializeField] private float _timeSwitchPanel;
        [SerializeField] private CanvasGroup _panelPers;

        [SerializeField] private Toggle[] _stars;
        [SerializeField] private TMP_Text _text;
        private Sequence _animation;
    
        // Start is called before the first frame update
        public void Show()
        {
            _animation = DOTween.Sequence().SetAutoKill(false).Pause();

            _animation.Append(_spot.DOScale(1.25f, _timeSpot).From(0.01f)).Join(_shadowPers.DOFade(1, _timeSpot).From(0))
                .Append(_spot.DOScale(1.0f, _timeSpot/5))
                .Append(_buttonCont.DOScale(1.2f, _timeButton).From(0))
                .Join(_buttonRep.DOScale(1.2f, _timeButton).From(0))
                .Append(_buttonCont.DOScale(1.0f, _timeButton))
                .Join((_buttonRep.DOScale(1.0f, _timeButton)))
                .OnComplete( () => OnCompleteFirstAnimation());

            _animation.Play();

            for (int i = 0; i < _stars.Length; i++)
            {
                if (PlayerLoadPrefs.currentStars > i)
                    _stars[i].isOn = true;
                else
                {
                    _stars[i].isOn = false;
                }
            }

            _text.text = " + " + ((int)RealizeBox.instance.score.currentScore).ToString() + " SEC";
        }
    

        public void OnCompleteFirstAnimation()
        {
            _buttonCont.DOScale(0.85f, _timeIdleButton).From(1).SetLoops(-1, LoopType.Yoyo);
        }
    
        public void StartAnim()
        {
            _spot.DOScale(1, _timeSpot).From(0.1f);
        
        }

        public void OpenLvlMap()
        {
            _panelPers.DOFade(0, _timeSwitchPanel).OnComplete( () => _panelPers.gameObject.SetActive(false));
        }
    }
}
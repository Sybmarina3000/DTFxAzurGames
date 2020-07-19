using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainBlueAnimation : MonoBehaviour
{
    [SerializeField] private float _timeSpot;
    [SerializeField] private float _timeButton;

    [SerializeField] private float _timeIdleButton;
    
    [SerializeField] private SpriteRenderer _shadowPers;
    [SerializeField] private RectTransform _spot;
    
    [SerializeField] private RectTransform _button;

    private Sequence _animation;
    
    // Start is called before the first frame update
    void Start()
    {
        _animation = DOTween.Sequence().SetAutoKill(false).Pause();

        _animation.Append(_spot.DOScale(1.25f, _timeSpot).From(0.01f)).Join(_shadowPers.DOFade(1, _timeSpot).From(0))
            .Append(_spot.DOScale(1.0f, _timeSpot/5))
            .Append(_button.DOScale(1.2f, _timeButton).From(0))
            .Append(_button.DOScale(1.0f, _timeButton))
            .OnComplete( () => OnCompleteFirstAnimation());

        _animation.Play();
//        _button.DOScale(1.0f, _timeButton);
    }
    

    public void OnCompleteFirstAnimation()
    {
        _button.DOScale(0.85f, _timeIdleButton).From(1).SetLoops(-1, LoopType.Yoyo);
    }
    
    public void StartAnim()
    {
        _spot.DOScale(1, _timeSpot).From(0.1f);
        
    }
}

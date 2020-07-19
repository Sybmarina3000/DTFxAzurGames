using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class LoseGameAnimation : MonoBehaviour
    {
        [SerializeField] private string _sceneName ;
        
        [SerializeField] private float _timeFirstMaskScale;
        
        [SerializeField] private float _timeWait;
        
        [SerializeField] private float _timeSecondMaskScale;
        
        [SerializeField] private GameObject _loseObj;

        [SerializeField] private Transform _mask;

        private Transform _player;

        private Sequence _maskAnimation;
        
        private void Start()
        {
            _player = RealizeBox.instance.player.transform;

            _maskAnimation = DOTween.Sequence().SetAutoKill(false).Pause();

            _maskAnimation.Append(_mask.DOScale(0.5f, _timeFirstMaskScale))
                .AppendInterval(_timeWait)
                .Append(_mask.DOScale(0, _timeSecondMaskScale))
                .OnComplete(() => SceneManager.LoadScene(_sceneName) );
        }

        public void StartLose()
        {
            _loseObj.transform.position = _player.position;
            _mask.position = _player.position;

            _maskAnimation.Restart();
        }
    }
}
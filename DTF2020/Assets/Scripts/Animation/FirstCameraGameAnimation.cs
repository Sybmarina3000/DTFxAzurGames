using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class FirstCameraGameAnimation : MonoBehaviour
    {
        [SerializeField] private GameObject _firstPoint;
        [SerializeField] private GameObject _finishPoint;
        [Space(10)]
        [SerializeField] private float _firstPauseTime;
        [Header(" speed move == first->finish + pause ")]
        [SerializeField] private float _finishPauseTime;
        [Header(" speed move == finish->first + pause before player spawn ")]
        [SerializeField] private float _returnTime;

        private playerController _player;
        private void Start()
        {
            _player = RealizeBox.instance.player;
        }

        public void StartAnimation()
        {
            // _firstPoint.SetActive(true);
            StartCoroutine("Animation");
        }
        
        private IEnumerator Animation()
        {
            yield return new WaitForSeconds(_firstPauseTime);
            _finishPoint.SetActive(true);
            
            yield return new WaitForSeconds(2 + _firstPauseTime);
            _finishPoint.SetActive(false);
            
            yield return new WaitForSeconds(_returnTime);
            _player.gameObject.SetActive(true);
            yield break;
            
        }
    }
}
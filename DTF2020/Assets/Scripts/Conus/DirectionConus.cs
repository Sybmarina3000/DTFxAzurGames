using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Image))]
    public class DirectionConus : UnityEngine.MonoBehaviour
    {
        
        Vector3 currentDirection => _currentVector;
        
        [SerializeField] int _radius;
        
        [SerializeField] float _timeOneRound;

        [SerializeField] private RectTransform _arrow;

        [SerializeField] private Vector3 _currentVector;

        private Image _conusImage; 
        
        private void Awake()
        {
            _conusImage = GetComponent<Image>();
            _conusImage.fillAmount = ((float)_radius) / 360;
            StartCoroutine(StartArrowLocation());
        }

        void Spawn(Vector3 from, Vector3 to)
        {
            StartCoroutine(StartArrowLocation());
        }

        IEnumerator StartArrowLocation()
        {
            int direction = 1;
            float currentAngle = 0;
            
            _arrow.localRotation = Quaternion.identity;
            
            while (true)
            {
                var angle = (_radius / _timeOneRound) * Time.deltaTime * direction;
                _arrow.Rotate( Vector3.back, angle);
                
                currentAngle += angle;
                if (currentAngle > _radius || currentAngle < 0 )
                    direction = direction * (-1);
                
                yield return null;
            }
            
            yield return null;
        }
    }
}
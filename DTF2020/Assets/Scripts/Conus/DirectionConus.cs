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

        private RectTransform _conusRectTransform;
        
        [SerializeField] private Transform _from;
        
        [SerializeField] private Transform _to;
        
        
        private void Awake()
        {
            _conusImage = GetComponent<Image>();
            _conusRectTransform = GetComponent<RectTransform>();
            
            _conusImage.fillAmount = ((float)_radius) / 360;
        }

        public void TestSpawn()
        {
            Spawn(_from.position, _to.position);
        } 
        
        void Spawn(Vector3 from, Vector3 to)
        {

            _conusRectTransform.position = Camera.main.WorldToScreenPoint(from);

            var vectorDirection = to - from;
            var angle = Vector3.Angle(Vector3.up, vectorDirection) - (0.5f * _radius);
            
            _conusRectTransform.rotation = Quaternion.identity;
            _conusRectTransform.Rotate(Vector3.back, angle);
            
            StartCoroutine(StartArrowLocation());
        }

        void Apply()
        {
            
        }
        
        IEnumerator StartArrowLocation()
        {
            int direction = 1;
            float currentAngle = 0;
            
            _arrow.localRotation = Quaternion.identity;
            
            while (true)
            {
                float gran = 0;
                if (currentAngle > _radius || currentAngle < 0)
                {
                    direction = direction * (-1);
                    if (currentAngle > _radius)
                        gran = _radius - currentAngle;
                    else
                        gran = currentAngle * (-1);
                }
                
                var angle = gran + (_radius / _timeOneRound) * Time.deltaTime * direction;
                _arrow.Rotate( Vector3.back, angle);
                
                currentAngle += angle;
                
                yield return null;
            }
            
            yield return null;
        }
    }
}
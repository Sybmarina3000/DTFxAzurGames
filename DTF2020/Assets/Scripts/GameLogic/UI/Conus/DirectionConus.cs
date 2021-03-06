﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    public class DirectionConus : UnityEngine.MonoBehaviour
    {
        
        public Vector3 currentDirection => _currentVector;
        
        [SerializeField] int _radius;
        
        [SerializeField] float _timeOneRound;

        [SerializeField] private RectTransform _arrow;

        [SerializeField] private RectTransform _arrowFinishPoint;

        [SerializeField] private Vector3 _currentVector;

        private Image _conusImage;

        private CanvasGroup _canvasGroup;

        private RectTransform _conusRectTransform;

        private Transform _playerRect;

        private bool _flagSpawn = false;
        
        [SerializeField] private Transform _from;
        
        [SerializeField] private Transform _to;
        
        
        private void Awake()
        {
            _conusImage = GetComponent<Image>();
            _conusRectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            
            _conusImage.fillAmount = ((float)_radius) / 360;
        }

        private void Start()
        {
            _playerRect = RealizeBox.instance.player.gameObject.transform;
        }

        public void TestSpawn()
        {
            Spawn(_from.position, _to.position);
        }

        public void Hide()
        {
            _flagSpawn = false;
            StopCoroutine(nameof(StartArrowLocation));
            _canvasGroup.alpha = 0;
        }

        private void Update()
        {
            if (!_flagSpawn)
                return;
            
            _conusRectTransform.position = Camera.main.WorldToScreenPoint(_playerRect.position);
        }

        public void Spawn(Vector3 from, Vector3 to)
        {
            _flagSpawn = true;
            _canvasGroup.alpha = 1;
            _conusRectTransform.position = Camera.main.WorldToScreenPoint(from);

            var vectorDirection = to - from;
            int znak = 1;
            if (to.x < from.x)
                znak = -1;
            
            var angle = (Vector3.Angle(Vector3.up, vectorDirection) - (0.5f * _radius * znak)) * znak;
            
            _conusRectTransform.rotation = Quaternion.identity;
            _conusRectTransform.Rotate(Vector3.back, angle);
            
            StartCoroutine(nameof(StartArrowLocation));
        }

        public Vector3 GetDirection()
        {
            var startPoint = _conusRectTransform.position;
            var finishPoint = _arrowFinishPoint.position;

            var direction = finishPoint - startPoint;
            
            Debug.Log(" direction: " + direction.normalized);
            return direction.normalized;
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
                
                var angle = gran + (_radius / _timeOneRound) * (Time.deltaTime / Time.timeScale * DefaultNamespace.RealizeBox.instance.manager.database.speedConus) * direction;
                _arrow.Rotate( Vector3.back, angle);
                
                currentAngle += angle;
                
                yield return null;
            }
            
            yield return null;
        }
    }
}
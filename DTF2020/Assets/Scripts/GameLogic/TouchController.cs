using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TouchController : MonoBehaviour
    {
        private DirectionConus _directionConus;
        private playerController _player;

        private bool _touchFlag = false;
        bool isTouch = false;


        private void Start()
        {
            _directionConus = RealizeBox.instance.directionConus;
            _player = RealizeBox.instance.player;
            
            enabled = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isTouch = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isTouch = false;
            }
            if (Input.touchCount == 1 || isTouch)
            {
                if (_touchFlag)
                    return;
                
                _touchFlag = true;
                _directionConus.Spawn(_player.gameObject.transform.position, new Vector3(500, 100));
            }
            else
            {
                if (!_touchFlag)
                    return;

                _touchFlag = false;
                _directionConus.Hide();
                _player.jump(_directionConus.GetDirection());
            }
        }
    }
}
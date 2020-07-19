using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class TouchController : MonoBehaviour
    {
        private DirectionConus _directionConus;
        private playerController _player;

        bool isTouch = false;
        bool jump = false;

        private void Start()
        {
            _directionConus = RealizeBox.instance.directionConus;
            _player = RealizeBox.instance.player;
            
            enabled = false;
        }

        public void startJump()
        {
            var point = RealizeBox.instance.level.getCurrentPoint();
            _directionConus.Spawn(_player.gameObject.transform.position, point.transform.position);
            jump = true;
        }

        public void endJump()
        {
            _directionConus.Hide();
            jump = false;
        }

        private void Update()
        {
            if (!jump)
            {
                return;
            }
            if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
            {
                _directionConus.Hide();
                _player.jump(_directionConus.GetDirection());
                RealizeBox.instance.player.setSlowMode(false);
                jump = false;
            }
        }
    }
}
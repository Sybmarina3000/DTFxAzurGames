using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class TouchController : MonoBehaviour
    {
        private DirectionConus _directionConus;
        private playerController _player;

        bool jump = false;
        bool ground = false;
        private void Start()
        {
            _directionConus = RealizeBox.instance.directionConus;
            _player = RealizeBox.instance.player;
            
            enabled = false;
        }

        public void startJump(bool aground = false)
        {
            ground = aground;
            var point = RealizeBox.instance.level.getCurrentPoint();
            if (!ground)
            {
                _directionConus.Spawn(_player.gameObject.transform.position, new Vector2(_player.gameObject.transform.position.x + 1, _player.gameObject.transform.position.y));
            }
            else
            {
                _directionConus.Hide();
            }
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
                if (ground)
                {
                    _player.jump(new Vector2(0, 1), ground);
                    ground = false;
                }
                else
                { 
                    _player.jump(_directionConus.GetDirection(), false);
                }
                RealizeBox.instance.player.setSlowMode(false);
                RealizeBox.instance.player.nextStepSlowMode();
                jump = false;
            }
        }
    }
}
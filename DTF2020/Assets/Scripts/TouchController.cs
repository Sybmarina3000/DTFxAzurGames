using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TouchController : MonoBehaviour
    {
        private DirectionConus _directionConus;
        
        private bool _touchFlag = false;
        
        private void Start()
        {
            _directionConus = RealizeBox.instance.directionConus;
        }

        private void Update()
        {
            if (Input.touchCount == 1)
            {
                if (_touchFlag)
                    return;
                
                _touchFlag = true;
                _directionConus.TestSpawn();
            }
            else
            {
                if (!_touchFlag)
                    return;

                _touchFlag = false;
                _directionConus.Hide();
                _directionConus.GetDirection();
            }
        }
    }
}
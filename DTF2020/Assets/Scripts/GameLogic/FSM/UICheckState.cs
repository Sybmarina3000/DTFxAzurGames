using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UICheckState : MonoBehaviour
    {
        [SerializeField] private Text _text;
        
        private void Start()
        {
            RealizeBox.instance.gameFSM.onStateChange += OnUpdateStete;
        }

        private void OnUpdateStete(GameState newState)
        {
            _text.text = newState.ToString();
        }
    }
}
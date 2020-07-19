using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private GameFSM FSM;
        
        private void Awake()
        {
            FSM = RealizeBox.instance.gameFSM;
            RealizeBox.instance.player.onSpawnInScene += OnPlayerSpawn;
            RealizeBox.instance.score.onUpdateValue += OnScoreChange;
        }

        private void Start()
        {
            Invoke( nameof(LateStart), 1.0f);
        }

        private void LateStart()
        {
            FSM.StartFSM();
        }
        private void OnPlayerSpawn()
        {
            FSM.SetNewState(GameState.PlayGame);
        }

        private void OnScoreChange(float amount, ScoreDecreaseType type)
        {
            if (amount == 0)
            {
                FSM.SetNewState(GameState.Lose);
            }
        }
        
    }
}
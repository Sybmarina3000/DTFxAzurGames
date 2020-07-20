using System;
using UnityEngine;
namespace DefaultNamespace
{
    [Serializable]
    public struct configs
    {
        public float speedConus;
        public float powerJumpFly;
        public float powerJumpGround;
        public float speedSlowModeOn;
        public float coefNormal;
        public float coefSlow;
        public float timeSlowMode;
        public float timeBeforeNewSlowMode;
    }

    public class GameManager : MonoBehaviour
    {
        private GameFSM FSM;
        string configName = "configs";
        public configs database;


        private void Awake()
        {
            FSM = RealizeBox.instance.gameFSM;
            RealizeBox.instance.player.onSpawnInScene += OnPlayerSpawn;
            RealizeBox.instance.score.onUpdateValue += OnScoreChange;
        }

        private void Start()
        {
            Invoke( nameof(LateStart), 1.0f);
            database = utilFunction.loadData<configs>(configName);
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
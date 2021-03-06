﻿using System;
using System.Collections.Generic;
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

        public void OnWin()
        {
            var level = PlayerPrefs.GetInt("currentLvl");

            FSM.SetNewState(GameState.Victory);
            RealizeBox.instance.score.StopDecreaseScore();
            AppMetrica.Instance.ReportEvent("level_finish", new System.Collections.Generic.Dictionary<string, object> { { "level", level.ToString() }, {"result", "win"}, {"time", 60 - RealizeBox.instance.score.currentScore}});

            var tutParams = new Dictionary<string, object>();
            tutParams["level"] = level.ToString();
            tutParams["result"] = "win";
            tutParams["time"] = 60 - RealizeBox.instance.score.currentScore;

           // FB.LogAppEvent("level_finish", parameters: tutParams);
        }
        
    }
}
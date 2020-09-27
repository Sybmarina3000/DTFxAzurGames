using DefaultNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum ScoreDecreaseType 
{
    Time,
    Damage,
}

public class Score : MonoBehaviour
{
    public Action<float, ScoreDecreaseType> onUpdateValue;
    
    public float currentScore => _currentScore; 
    
    private float _currentScore;

    private bool _stop = true;
    
    private void Start()
    {
        _currentScore = 60;
    }

    public void StartDecreaseScore()
    {
        _stop = false;
    }

    public void StopDecreaseScore()
    {
        _stop = true;
    }
    
    //FOR DAMAGE
    public void Decrease(float amount, ScoreDecreaseType type)
    {
        if (_stop)
            return;
        if ((_currentScore - amount) < 0) {
            var level = PlayerPrefs.GetInt("currentLvl");

            AppMetrica.Instance.ReportEvent("level_start", new System.Collections.Generic.Dictionary<string, object> { { "level", level.ToString() }, { "result", "lose" }, { "time", 60 - currentScore } });

            var tutParams = new Dictionary<string, object>();
            tutParams["level"] = level.ToString();
            tutParams["result"] = "lose";
            tutParams["time"] = 60 - currentScore;

            //FB.LogAppEvent("level_start", parameters: tutParams);

            StopDecreaseScore();
        }
            
        _currentScore = (_currentScore - amount) < 0 ? 0 : (_currentScore - amount);
        
        onUpdateValue?.Invoke(_currentScore, type);
    }

    private void Update()
    {
        if(!_stop)
            Decrease(Time.deltaTime, ScoreDecreaseType.Time);
    }

    public void TestDamage(int value)
    {
        Decrease(value, ScoreDecreaseType.Damage);
    }
}

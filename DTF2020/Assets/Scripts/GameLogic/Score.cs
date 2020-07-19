using System;
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

    private void Start()
    {
        _currentScore = 60;
    }

    //FOR DAMAGE
    public void Decrease(float amount, ScoreDecreaseType type)
    {
        _currentScore = (_currentScore - amount) < 0 ? 0 : (_currentScore - amount);
        onUpdateValue?.Invoke(_currentScore, type);
    }

    private void Update()
    {
        Decrease(Time.deltaTime, ScoreDecreaseType.Time);
    }

    public void TestDamage(int value)
    {
        Decrease(value, ScoreDecreaseType.Damage);
    }
}

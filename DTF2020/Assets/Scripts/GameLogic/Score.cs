using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    public Action<float> onUpdateValue;
    
    public float currentScore => _currentScore; 
    
    private float _currentScore;

    private void Start()
    {
        _currentScore = 60;
    }

    //FOR DAMAGE
    public void Decrease(float amount)
    {
        _currentScore = (_currentScore - amount) < 0 ? 0 : (_currentScore - amount);
        onUpdateValue?.Invoke(_currentScore);
    }

    private void Update()
    {
        Decrease(Time.deltaTime);
    }
}

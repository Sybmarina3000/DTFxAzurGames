using DefaultNamespace.UI;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class RealizeBox : MonoBehaviourSingleton<RealizeBox>
    {
        public DirectionConus directionConus => _directionConus;
        public playerController player => _player;
        
        public FirstCameraGameAnimation firstCameraGameAnimation => _firstCameraGameAnimation;
        public LoseGameAnimation loseGameAnimation => _loseGameAnimation;
        
        public GameFSM gameFSM => _gameFsm;
        public MainGameUI mainGameUi => _mainGameUi;
        public TouchController touchController => _touchController;
        public Score score => _score;
        public levelConroller level => _level;

        public GameManager manager => _manager;

        [SerializeField] private playerController _player;
        [SerializeField] private DirectionConus _directionConus;
        
        [SerializeField] private FirstCameraGameAnimation _firstCameraGameAnimation; 
        [SerializeField] private  LoseGameAnimation _loseGameAnimation;
        
        [SerializeField] private GameFSM _gameFsm;
        [SerializeField] private MainGameUI _mainGameUi;
        [SerializeField] private Score _score;

        [SerializeField] private levelConroller _level;
        [SerializeField] private TouchController _touchController;

        [SerializeField] private GameManager _manager;
    }
}
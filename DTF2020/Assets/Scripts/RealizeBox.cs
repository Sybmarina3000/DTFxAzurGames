using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class RealizeBox : MonoBehaviourSingleton<RealizeBox>
    {
        public DirectionConus directionConus => _directionConus;
        public playerController player => _player;
        public FirstCameraGameAnimation firstCameraGameAnimation => _firstCameraGameAnimation;
        public GameFSM gameFSM => _gameFsm;
        public MainGameUI mainGameUi => _mainGameUi;
        public TouchController touchController => _touchController;

        [SerializeField] private playerController _player;
        [SerializeField] private DirectionConus _directionConus;
        [SerializeField] private FirstCameraGameAnimation _firstCameraGameAnimation;
        [SerializeField] private GameFSM _gameFsm;
        [SerializeField] private MainGameUI _mainGameUi;
        
        [SerializeField] private TouchController _touchController;
    }
}
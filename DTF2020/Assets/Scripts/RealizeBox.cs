using UnityEngine;

namespace DefaultNamespace
{
    public class RealizeBox : MonoBehaviourSingleton<RealizeBox>
    {
        public DirectionConus directionConus => _directionConus;
        
        [SerializeField] private DirectionConus _directionConus;

        public playerController player => _player;

        [SerializeField] private playerController _player;
    }
}
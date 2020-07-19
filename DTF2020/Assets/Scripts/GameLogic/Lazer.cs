using UnityEngine;

namespace DefaultNamespace
{
    public class Lazer : MonoBehaviour
    {
        private Score _score;
        
        [SerializeField] private int _damage;
        
        private void Start()
        {
            _score = RealizeBox.instance.score;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                Debug.Log("WOW");
                SetDamage();
            }
        }

        private void SetDamage()
        {
            _score.Decrease(_damage, ScoreDecreaseType.Damage);
        }
    }
}
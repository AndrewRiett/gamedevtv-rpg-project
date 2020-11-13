using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float healthPoints = 100f;
        private bool isDead = false;

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
        }

        public void takeDamage(float damage)
        {
            // for making sure that health is >= 0
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints <= 0f && !IsDead())
            {
                Die();
            }
        }

        public float GetHealthPercentage()
        {
            return 100f * (healthPoints / GetComponent<BaseStats>().GetHealth());
        }

        public bool IsDead()
        {
            return isDead;
        }
        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }
}
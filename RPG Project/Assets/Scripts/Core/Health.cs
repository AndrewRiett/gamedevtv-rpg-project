using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health: MonoBehaviour
    {
        [SerializeField] private float healthPoints = 100f;
        private bool isDead = false;

        public void takeDamage(float damage)
        {
            // for making sure that health is >= 0
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints <= 0f && !IsDead())
            {
                Die();
            }
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

        /*public object CaptureState()
        {
            return healthPoints;
        }
        
        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints == 0)
            {
                Die();
            }
        }*/
    }
}
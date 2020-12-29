using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] public UnityEvent<float> onDamage;
        [SerializeField] public UnityEvent onDie = null;

        private LazyValue<float> healthPoints;
        private BaseStats baseStats;
        private bool isDead = false;

        private void Awake() 
        {
            baseStats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
            
        }

        private void Start()
        {
            healthPoints.ForceInit(); // if health value was not called before start
        }

        private void OnEnable()
        {
            baseStats.onLevelUp += RestoreLevelHealth;
        }

        private void OnDisable()
        {
            baseStats.onLevelUp -= RestoreLevelHealth;
        }

        public void takeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0); // Mathf.Max for making sure that health is >= 0
            onDamage.Invoke(damage);

            if (healthPoints.value == 0f && !IsDead())
            {
                Die();
                onDie.Invoke();
                AwardExperiance(instigator);
            }
        }

        public float GetHealthPercentage()
        {
            return 100f * GetHealthFraction();
        }

        public float GetHealthFraction()
        {
            return healthPoints.value / baseStats.GetStat(StatType.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public float GetMaxHealth()
        {
            return baseStats.GetStat(StatType.Health);
        }

        public float GetCurrentHealth()
        {
            return healthPoints.value;
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperiance(GameObject instigator)
        {
            Experiance experiance = instigator.GetComponent<Experiance>();

            if (experiance != null)
            {
                experiance.GainExperiance(baseStats.GetStat(StatType.ExperianceReward));
            }
        }

        private void RestoreLevelHealth()
        {
            healthPoints.value = baseStats.GetStat(StatType.Health);
        }
        
        public void RestoreHealth(float amount)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + amount, GetMaxHealth());
        }

        private float GetInitialHealth()
        {
            return baseStats.GetStat(StatType.Health);
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value <= 0)
            {
                Die();
            }
        }
    }
}
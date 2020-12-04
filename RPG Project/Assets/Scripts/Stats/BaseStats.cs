using System;
using System.Collections.Generic;
using RPG.Utilities;
using UnityEngine;

namespace RPG.Stats
{

    public class BaseStats : MonoBehaviour
    {
#pragma warning disable 0649 // to avoid the annoying unassigning error
        [SerializeField] CharacterType characterType;
#pragma warning restore 0649
        [SerializeField] Progression progression = null;
        [SerializeField] bool shouldUseModifiers = false; // allows to obtain acquire only for player
        [Range(0, 99)] [SerializeField] int startingLevel = 1;

        [Header("For VFX Instantiation")]
        [SerializeField] GameObject levelUpVFX = null;
        [SerializeField] Transform hipsTransform = null;

        public event Action onLevelUp;
        Experiance experiance;
        private LazyValue<int> currentLevel;

        private void Awake()
        {
            currentLevel = new LazyValue<int>(CalculateLevel);
            experiance = GetComponent<Experiance>();
        }

        private void OnEnable()
        {
            if (experiance != null)
            {
                experiance.onExperianceGained += UpdateLevel;
                onLevelUp += LevelUpEffect;
            }
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        private void OnDisable()
        {
            if (experiance != null)
            {
                experiance.onExperianceGained -= UpdateLevel;
                onLevelUp -= LevelUpEffect;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                onLevelUp(); // calling the event
            }

        }

        public float GetStat(StatType statType)
        {
            // firstly counts stat + modifier = value (e.g. 100)
            float value = GetBaseStat(statType) + GetAdditiveModifier(statType);

            // then percentage (e.g. 10%): 10 / 100 = 0.1 => 1 + 0.1 = 1.1 (or 110%)
            float percentage = (1 + GetPercentageModifier(statType) / 100);

            // value (100) * percentage (1.1) = 110 
            // returns 110
            return value * percentage;
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }
        
        private int CalculateLevel() // NB: it replaces the starting level
        {
            if (experiance == null) return startingLevel;

            float currentExp = experiance.GetExperiancePoints();

            float[] levelValues = progression.GetStatLevels(characterType, StatType.ExperianceToLevelUp);
            int thresholdLevel = levelValues.Length;

            for (int level = 1; level <= thresholdLevel; level++)
            {
                float expToLevelUp = levelValues[level - 1];
                if (currentExp < expToLevelUp)
                {
                    return level;
                }
            }
            // level >= thresholdLevel, currentExp >= levelValues[level]
            return thresholdLevel + 1; // it means that the level should be max (treshhold + 1)
        }

        private float GetAdditiveModifier(StatType statType)
        {
            if (!shouldUseModifiers) return 0f;

            float total = 0f;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                IEnumerable<float> modifiers = provider.GetAdditiveModifiers(statType);
                foreach (float modifier in modifiers)
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(StatType statType)
        {
            if (!shouldUseModifiers) return 0f;

            float total = 0f;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                IEnumerable<float> modifiers = provider.GetAdditivePercentages(statType);
                foreach (float modifier in modifiers)
                {
                    total += modifier;
                }
            }
            return total;
        }

        private void LevelUpEffect()
        {
            if (levelUpVFX != null)
            {
                Instantiate(levelUpVFX, hipsTransform.position, Quaternion.identity);
            }
        }

        private float GetBaseStat(StatType statType)
        {
            return progression.GetStat(characterType, statType, GetLevel());
        }
    }
}
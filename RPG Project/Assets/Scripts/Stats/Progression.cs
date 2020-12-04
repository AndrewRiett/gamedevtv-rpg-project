using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterType[] characterTypes = null;

        Dictionary<CharacterType, Dictionary<StatType, float[]>> lookupTable = null;

        public float GetStat(CharacterType characterType, StatType statType, int level)
        {
            BuildLookup();

            float[] levels = lookupTable[characterType][statType];

            if (levels.Length < level)
            {
                Debug.LogError(this + " levels.Length < level, the method GetStat() returns 0");
                return 0f;
            }

            return levels[level - 1];
        }

         public float[] GetStatLevels(CharacterType characterType, StatType statType)
         {
            BuildLookup();

            return lookupTable[characterType][statType];
         }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterType, Dictionary<StatType, float[]>>();

            foreach (ProgressionCharacterType progressionType in characterTypes)
            {
                var statsLookupTable = new Dictionary<StatType, float[]>();

                foreach (ProgressionStat progressionStat in progressionType.stats)
                {   // populating the nested dictionary with levels[] related to stat types
                    statsLookupTable[progressionStat.statType] = progressionStat.levels;
                }

                lookupTable[progressionType.characterType] = statsLookupTable; // assigning the nested dictionary to the main key (character type value)
            }
        }

        [System.Serializable]
        class ProgressionCharacterType
        {
#pragma warning disable 0649 // to avoid the annoying unity unassigned value error
            public CharacterType characterType;
#pragma warning restore 0649
            public ProgressionStat[] stats = null;
        }
        
        [System.Serializable]
        class ProgressionStat
        {
#pragma warning disable 0649
            public StatType statType;
#pragma warning restore 0649
            public float[] levels = null;
        }
    }
}
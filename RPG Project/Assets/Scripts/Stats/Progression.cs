using UnityEngine;

namespace RPG.Stats
{

    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterType[] characterTypes = null;

        public float GetHealth(CharacterType characterType, int level)
        {
            foreach (ProgressionCharacterType type in characterTypes)
            {
                if (characterType == type.characterType)
                {
                    return type.healh[level - 1];
                }
            }

            Debug.LogError(this + " GetHealth(CharacterType characterType, int level) returns 0");
            return 0f;
        }

        [System.Serializable]
        class ProgressionCharacterType
        {
            #pragma warning disable 0649 // to avoid the annoying unassigning error
            public CharacterType characterType;
            #pragma warning restore 0649
            public float[] healh = null;
        }
    }
}
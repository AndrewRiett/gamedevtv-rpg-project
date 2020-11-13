using UnityEngine;

namespace RPG.Stats
{

    public class BaseStats : MonoBehaviour
    {
        #pragma warning disable 0649 // to avoid the annoying unassigning error
        [SerializeField] CharacterType characterType;
        #pragma warning restore 0649
        [SerializeField] Progression progression = null;
        [Range(0, 99)] [SerializeField] int startingLevel = 1;

        public float GetHealth()
        {
            return progression.GetHealth(characterType, startingLevel);
        }
    }
}
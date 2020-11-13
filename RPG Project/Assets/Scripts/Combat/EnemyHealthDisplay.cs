using RPG.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Health targetHealth;
        Fighter playerFighter;
        string defaultText = "N/A";

        private void Awake()
        {
            playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            
        }

        private void Update()
        {
            if (playerFighter.GetTarget() != null)
            {
                targetHealth = playerFighter.GetTarget();
                GetComponent<Text>().text = targetHealth.GetHealthPercentage().ToString("F0") + "%";
            }
            else
            {
                GetComponent<Text>().text = defaultText;
            }
        }
    }
}
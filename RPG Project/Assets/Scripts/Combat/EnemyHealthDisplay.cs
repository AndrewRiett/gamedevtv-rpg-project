using System;
using RPG.Attributes;
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
            if (playerFighter.GetTarget() == null)
            {
                GetComponent<Text>().text = defaultText;
                return;
            }

            targetHealth = playerFighter.GetTarget();

            // {0:0}/{1:0} - {0 argument with 0 numbers after point} + "/" + {1 argument with the same format}
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", 
                targetHealth.GetCurrentHealth(), targetHealth.GetMaxHealth());
        }
    }
}
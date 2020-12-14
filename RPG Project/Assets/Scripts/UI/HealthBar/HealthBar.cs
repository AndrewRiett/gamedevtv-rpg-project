using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.UI.HealthBar
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health = null;

        [SerializeField] Canvas healthCanvas = null;
        [SerializeField] RectTransform foreground = null;

        private void Start()
        {
            UpdateHealthBar();
        }

        public void UpdateHealthBar()
        {
            foreground.localScale = new Vector3(health.GetHealthFraction(),
                foreground.localScale.y, foreground.localScale.z);

            if (health.GetHealthFraction() < 1 && health.GetHealthFraction() > 0)
            {
                EnableHealthBar();
            }
            else
            {
                DisableHealthBar();
            }

        }

        private void EnableHealthBar()
        {
            healthCanvas.enabled = true;
        }

        private void DisableHealthBar()
        {
            healthCanvas.enabled = false;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text damageValue = null;

        public void SetValue(float damage)
        {
            damageValue.text = damage.ToString("F1");
        }

        // used in Unity Animator to destroy an object on the end of the animation event
        private void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}
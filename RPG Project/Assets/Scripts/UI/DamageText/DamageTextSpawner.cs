using UnityEngine;
using System.Collections;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab = null;

        public void Spawn(float damage)
        {
            DamageText dmText = Instantiate(damageTextPrefab, transform);
            
            dmText.SetValue(damage);
            dmText.transform.parent = transform;
        }
    }
}
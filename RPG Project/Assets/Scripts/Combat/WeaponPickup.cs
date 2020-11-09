using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weaponToEquip = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Collision");
                other.GetComponent<Fighter>().EquipWeapon(weaponToEquip);
                Destroy(gameObject);
            }
        }
    }
}
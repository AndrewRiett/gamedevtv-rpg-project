using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weaponToEquip = null;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weaponToEquip);
                StartCoroutine(RespawnAfterTime(respawnTime));
            }
        }

        private IEnumerator RespawnAfterTime(float time)
        {
            EnablePickup(false);
            yield return new WaitForSeconds(time);
            EnablePickup(true);

        }

        private void EnablePickup(bool state)
        {
            GetComponent<Collider>().enabled = state;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(state);
            }
        }
    }
}
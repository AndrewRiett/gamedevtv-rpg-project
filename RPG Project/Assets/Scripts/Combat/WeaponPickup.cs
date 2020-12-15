using RPG.Control;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weaponToEquip = null;
        [SerializeField] float respawnTime = 5f;

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weaponToEquip);
            StartCoroutine(RespawnAfterTime(respawnTime));
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
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(state);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
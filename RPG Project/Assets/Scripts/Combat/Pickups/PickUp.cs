using UnityEngine;
using System.Collections;
using RPG.Control;
using RPG.Movement;
using UnityEngine.Events;

namespace RPG.Combat.Pickups
{
    public class PickUp : MonoBehaviour, IRaycastable
    {
        [SerializeField] float respawnTime = 5f;
        [SerializeField] private UnityEvent onPickup = null;

        public bool HandleRaycast(PlayerController callingController)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Mover>().MoveTo(transform.position);
                //Pickup(callingController.gameObject);
            }
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
                onPickup.Invoke();
            }
        }

        protected virtual void Pickup(GameObject subject)
        {
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
using UnityEngine;
using System.Collections;
using RPG.Attributes;

namespace RPG.Combat.Pickups
{
    public class HealthPickup : PickUp
    {
        
        [SerializeField] private float healthToRestore = 0f;

        protected override void Pickup(GameObject subject)
        {
            base.Pickup(subject);
            subject.GetComponent<Health>().RestoreHealth(healthToRestore);
            
        }
    }
}
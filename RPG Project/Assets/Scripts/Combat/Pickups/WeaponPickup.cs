using RPG.Control;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat.Pickups
{
    public class WeaponPickup : PickUp
    {
        [SerializeField] WeaponConfig weaponToEquip = null;

        protected override void Pickup(GameObject subject)
        {
            base.Pickup(subject);
            if (weaponToEquip != null)
                subject.GetComponent<Fighter>().EquipWeapon(weaponToEquip);
        }
    }
}
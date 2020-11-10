using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        /// <summary>
        /// Instantiates the weapon on the particular Transform position and overrides the animation.
        /// </summary>
        /// <param name="handTransform"></param>
        /// <param name="animator"></param>
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, ChooseHand(leftHand, rightHand));
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride; // overriding the animator in runtime
            }
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, ChooseHand(rightHand, leftHand).position, Quaternion.identity);
            
            projectileInstance.SetTarget(target, weaponDamage);
        }

        public Transform ChooseHand(Transform leftHand, Transform rightHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public bool HasProjectTile()
        {
            return projectile != null;
        }

        public float GetWeaponRange()
        {
            return this.weaponRange;
        }
        public float GetWeaponDamage()
        {
            return this.weaponDamage;
        }
    }
}

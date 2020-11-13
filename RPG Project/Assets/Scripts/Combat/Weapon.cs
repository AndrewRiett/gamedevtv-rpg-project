using UnityEngine;
using RPG.Resources;


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

        const string weaponName = "Weapon";

        /// <summary>
        /// Instantiates the weapon on the particular Transform position and overrides the animation.
        /// </summary>
        /// <param name="handTransform"></param>
        /// <param name="animator"></param>
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (equippedPrefab != null)
            {
                GameObject weapon = Instantiate(equippedPrefab, ChooseHand(leftHand, rightHand));
                weapon.name = weaponName;
            }

            // casting, if result is false, then return null
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride; // overriding the animator in runtime
            }
            else if (overrideController != null) // if the animator was overriden (not null), returns it to the default state
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;  // override is a child of animator, so it posesses the animator runtime default state
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

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);

            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);

        }
    }
}

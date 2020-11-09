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


        /// <summary>
        /// Instantiates the weapon on the particular Transform position and overrides the animation.
        /// </summary>
        /// <param name="handTransform"></param>
        /// <param name="animator"></param>
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, isRightHanded ? rightHand : leftHand);
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride; // overriding the animator in runtime
            }
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

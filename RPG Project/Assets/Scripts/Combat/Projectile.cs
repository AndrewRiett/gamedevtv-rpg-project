using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] private bool isHoming = false; // whether a projectile should follow the target
        [SerializeField] private float speed = 1f;
        [SerializeField] private float maxLifeTime = 5f;
        
        [Space]
        [SerializeField] GameObject[] destroyOnHit = null; // for making some objects of projectile to destroy immediatly
        [SerializeField] float lifeAfterImpact = 0.4f; // time to destroy other objects, like particle effects

        [Space]
        [SerializeField] UnityEvent onHit = null;

        private Health target = null;
        private GameObject instigator;
        private float damage = 0f;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
            Destroy(gameObject, maxLifeTime);
        }

        private void Update()
        {
            if (target == null) return;

            if (isHoming && !target.IsDead()) transform.LookAt(GetAimLocation()); // for making an arrow to always follow a target
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.instigator = instigator;
            this.damage = damage;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            
            onHit.Invoke();
            target.takeDamage(instigator, damage);
            
            GetComponent<Collider>().enabled = false;

            speed = 0f;

            if (hitEffect != null) 
            {
                GameObject effect = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }

            // position is located almost on the ground, so we take 1 on Y axis and multiply it 
            // by the middle point of the collider, 
            // and then add the obtained value to the current position 
            return target.transform.position + (Vector3.up * targetCapsule.height / 2);
        }
    }
}
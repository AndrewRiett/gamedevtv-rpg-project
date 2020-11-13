using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        Mover mover;
        Fighter fighter;

        void Start()
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return; 
            if (InteractWithMovement()) return; 
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target == null) continue;
                if (!fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    fighter.Attack(target.gameObject);
                }
                return true; // used here for hovering mouse over an enemy and changing the cursore icon (cursor affordance)
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hitInfo;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hitInfo);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(hitInfo.point);

                }
                // Debug.DrawLine(GetMouseRay().origin, GetMouseRay().direction, Color.white);
                return true; // used here for hovering mouse over an enemy and changing the cursore icon (cursor affordance)
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
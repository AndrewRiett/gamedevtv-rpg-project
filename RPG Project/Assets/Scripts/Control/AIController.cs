using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Utilities;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [Range(0, 1)] // the fraction could only be withing the range [0, 1)
        [SerializeField] float patrolSpeedFraction = 0.2f; // % of maxSpeed in Mover
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;

        [Space]
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float waypointToleranceDistance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        private GameObject player;
        private Health health;
        private Fighter fighter;
        private Mover mover;
        private LazyValue<Vector3> guardPosition;

        bool wasAggrevated = false;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceLastWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead()) return;

            UpdateTimers();

            // CanAttack is checked here because it is needed to know whether player's health is > 0
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player)) // attack state
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer <= suspicionTime) // suspicion state
            {
                SuspicionBehaviour();
            }
            else // moving state
            {
                PatrolBehaviour();
            }
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceLastWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    wasAggrevated = false;
                    timeSinceLastWaypoint = 0f;
                    CycleWaypoint();

                }

                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceLastWaypoint >= waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, wasAggrevated ? 1f : patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointToleranceDistance;
        }


        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            wasAggrevated = true;
            timeSinceLastSawPlayer = 0f;

            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return (distanceToPlayer <= chaseDistance);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        // called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
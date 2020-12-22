using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] private float maxNavMeshPathLength = 40f;

        Health health;
        Animator animator;
        NavMeshAgent navMeshAgent;

        void Awake()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead(); // to prevent stopping player by dead bodies
            UpdateAnimator();
        }

        // a method is used for creating a distinction between starting the movement action and moving to
        public void StartMoveAction(Vector3 destination, float speedFraction = 1f)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            //NavMesh path
            NavMeshPath path = new NavMeshPath(); // out variable, 
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false; // for making it impossible to calculate partial paths
            if (GetPathLength(path) > maxNavMeshPathLength) return false;

            return true;
        }

        public void MoveTo(Vector3 destination, float speedFraction = 1f)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction); // clamp for protection and keeping the range within 100%
            navMeshAgent.isStopped = false;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0f;
            if (path.corners.Length < 2) return 0f;

            Vector3[] pathCorners = path.corners;

            for (int i = 0; i < pathCorners.Length - 1; i++)
            {
                total += Vector3.Distance(pathCorners[i], pathCorners[i + 1]);
            }

            return total;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        [System.Serializable]
        private struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        } 

        public object CaptureState()
        {
            MoverSaveData data;
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;
   
            // navMesh is called directly to prevent race condition
            navMeshAgent.Warp(data.position.ToVector()); 
            transform.eulerAngles = data.rotation.ToVector();
        }
    }
}

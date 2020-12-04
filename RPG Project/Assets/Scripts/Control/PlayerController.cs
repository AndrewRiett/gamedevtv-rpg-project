using UnityEngine;
using RPG.Movement;
using RPG.Resources;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CursorSet cursorSet = null;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float maxNavMeshPathLength = 40f;

        private Health health;
        private Mover mover;

        void Awake()
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            if (InteractWithUI())
            {
                return;
            }

            if (health.IsDead())
            {
                cursorSet.SetCursor(CursorType.UI);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            cursorSet.SetCursor(CursorType.Default);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        cursorSet.SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay()); // Get all hits
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance; // build an array of distances
            }

            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithMovement()
        {
            Vector3 targetPosition;
            bool hasHit = RaycastNavMesh(out targetPosition);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(targetPosition);
                }
                cursorSet.SetCursor(CursorType.Movement);
                return true; // used here for hovering mouse over an enemy and changing the cursore icon (cursor affordance)
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 targetPosition)
        {
            targetPosition = Vector3.zero;

            // Raycast
            RaycastHit hitInfo;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hitInfo);

            if (!hasHit) return false; // return early if there is no raycastHit

            // find the nearest navMesh hit within the range of maxNavMeshProjectionDistance
            // on a RaycastHit point and stores it to the navMeshHit
            NavMeshHit navMeshHit;
            bool isNavHitFound = NavMesh.SamplePosition(
                hitInfo.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);

            if (!isNavHitFound) return false; // return early if there is no navMeshHit found

            targetPosition = navMeshHit.position; // assign a position if found

            NavMeshPath path = new NavMeshPath(); // out variable, 
            bool hasPath = NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false; // for making it impossible to calculate partial paths
            if (GetPathLength(path) > maxNavMeshPathLength) return false;

            return true; // return true
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0f;
            if (path.corners.Length < 2) return 0f;

            Vector3[] pathCorners = path.corners;

            for (int i = 0; i < pathCorners.Length-1; i++)
            {
                Debug.Log($"{i}: {pathCorners[i]}, {i+1}: {pathCorners[i + 1]}");
                Debug.Log($"total = {total}");

                total += Vector3.Distance(pathCorners[i], pathCorners[i + 1]);
            }

            return total;
        }

        private bool InteractWithUI()
        {
            // Check if the mouse is being hovered over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                cursorSet.SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
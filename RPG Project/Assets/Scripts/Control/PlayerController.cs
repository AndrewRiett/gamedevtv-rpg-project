using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CursorSet cursorSet = null;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float raycastRadius = 1f;

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
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius); // Get all hits
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
                if (!mover.CanMoveTo(targetPosition)) return false;

                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(targetPosition);
                }
                cursorSet.SetCursor(CursorType.Movement);
                return true;
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


            // NavMesh
            // find the nearest navMesh hit within the range of maxNavMeshProjectionDistance
            // on a RaycastHit point and stores it to the navMeshHit
            NavMeshHit navMeshHit;
            bool isNavHitFound = NavMesh.SamplePosition(
                hitInfo.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);

            if (!isNavHitFound) return false; // return early if there is no navMeshHit found

            targetPosition = navMeshHit.position; // assign a position if found

            return true; // return true
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
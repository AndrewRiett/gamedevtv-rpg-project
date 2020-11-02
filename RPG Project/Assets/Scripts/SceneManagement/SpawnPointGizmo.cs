using UnityEngine;

namespace RPG.SceneManagement
{
    public class SpawnPointGizmo : MonoBehaviour
    {

        private void OnDrawGizmosSelected()
        {
            // Draws a line to the forward direction in order to adjust spawning rotation
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
            Gizmos.DrawLine(transform.position, transform.position + (transform.forward));
        }
    }
}
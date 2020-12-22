using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Core
{
    public class RestartGame : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
            }
        }
    }
}



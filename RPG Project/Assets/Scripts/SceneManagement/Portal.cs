using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour 
    {
        [Header("SceneLoading:")]
        [SerializeField] int sceneToLoadIndex = -1; // -1 to throw an error if the portal is not configured
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] DestinationIdentifier destination = 0;

        [Header("Fading:")]
        [SerializeField] private float fadeOutTime = 0.5f;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float fadeWaitTime = 0.5f;

        

        private enum DestinationIdentifier
        {
            A, B, C, D, F
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                DontDestroyOnLoad(gameObject);
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoadIndex < 0)
            {
                Debug.LogError($"gameObject name: {gameObject} - Scene To Load Index is not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject); // in order to transfer the portal in another scene and execute its functionality

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime); // fades the screen before loading
            
            yield return SceneManager.LoadSceneAsync(sceneToLoadIndex); // loads the scene

            // configure player position while fadeOut() 
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            // waits in fadeOut() after the configuration in order to give time for camera and etc. to stabilize
            yield return new WaitForSeconds(fadeWaitTime);

            
            yield return fader.FadeIn(fadeInTime); // fades in after everything is done

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            
            player.GetComponent<PlayerController>().enabled = false;

            // NavMeshAgent.Warp() used in order to prevent conflicts with AI system;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.transform.position);
            player.transform.rotation = otherPortal.spawnPoint.transform.rotation;

            player.GetComponent<PlayerController>().enabled = true;
        }
        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (!this.Equals(portal) && portal.destination.Equals(destination))
                {
                    return portal;
                }
            }

            return null;
        }

    }
}
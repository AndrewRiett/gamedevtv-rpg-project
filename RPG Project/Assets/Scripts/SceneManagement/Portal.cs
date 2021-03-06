﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;
using RPG.Saving;


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

            // in order to transfer the portal in another scene and execute its functionality
            DontDestroyOnLoad(gameObject); 


            Fader fader = FindObjectOfType<Fader>();

            // removes control from the player
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;


            // fades the screen before loading
            yield return fader.FadeOut(fadeOutTime);

            SavingWrapper sWrapper = FindObjectOfType<SavingWrapper>();
            sWrapper.Save(); // saves the current scene state

            // async. loads the scene
            yield return SceneManager.LoadSceneAsync(sceneToLoadIndex);

            // removes control from the player twice, because the new player will be loaded
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;


            // load the current scene state
            sWrapper.Load(); 
            
            // configure player position while fadeOut() 
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            sWrapper.Save(); // saves as a checkpoint after loading the location
            
            yield return new WaitForSeconds(fadeWaitTime); // waits in fadeOut() after the configuration in order to give time for camera and etc. to stabilize
            fader.FadeIn(fadeInTime); // fades in after everything is done

            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            // NavMeshAgent.Warp() used in order to prevent conflicts with AI system;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.transform.position);
            player.transform.rotation = otherPortal.spawnPoint.transform.rotation;
        }
        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
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
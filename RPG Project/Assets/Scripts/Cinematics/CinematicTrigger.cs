﻿using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        [SerializeField] bool alreadyTriggered = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GetComponent<PlayableDirector>().Stop();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyTriggered && other.CompareTag("Player"))
            {
                alreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
        public object CaptureState()
        {
            return alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            alreadyTriggered = (bool)state;
        }
    }
}
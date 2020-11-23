using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experiance : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiancePoints;

        public bool isExperianceRestored {get; private set;}

        public event Action onExperianceGained;

        public void GainExperiance(float experiance)
        {
            experiancePoints += experiance;
            onExperianceGained();
        }

        public float GetExperiancePoints()
        {
            return experiancePoints;
        }

         public object CaptureState()
        {
            return experiancePoints;
        }

        public void RestoreState(object state)
        {
            experiancePoints = (float)state;
        }
    }
}
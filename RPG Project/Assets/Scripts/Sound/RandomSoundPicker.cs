using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Sounds
{
    public class RandomSoundPicker : MonoBehaviour
    {
        [SerializeField] List<AudioSource> sounds = null;

        public void Play()
        {
            ChooseSound(sounds).Play();
        }

        private AudioSource ChooseSound(List<AudioSource> audioSources)
        {
            int randomIndex = Random.Range(0, audioSources.Count-1);

            return audioSources[randomIndex];
        }
    }
}

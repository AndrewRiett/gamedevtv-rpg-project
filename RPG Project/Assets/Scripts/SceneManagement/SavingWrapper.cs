using UnityEngine;
using System.Collections;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float fadeTime = 0.2f;
        private const string defaultSaveFile = "save";


        private IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            
            yield return fader.FadeOut(0f);
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            Debug.Log("Loading");
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

    }
}
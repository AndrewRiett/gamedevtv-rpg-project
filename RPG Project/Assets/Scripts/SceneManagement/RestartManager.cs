using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class RestartManager : MonoBehaviour
    {
        #region SINGLETON

        #endregion
        public static RestartManager Instance { get; private set; }


        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void RestaratFromSave()
        {
            StartCoroutine(RestartRoutine());
        }

        public IEnumerator RestartRoutine()
        {
            yield return new WaitForSeconds(2f);
            yield return FindObjectOfType<Fader>().FadeOut(1f);

            SceneManager.LoadScene(0);
            yield return FindObjectOfType<Fader>().FadeIn(1f);
        }
    }
}
using RPG.Control;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Restarter : MonoBehaviour
    {
        [SerializeField] private float fadeTime = 2f;
        [SerializeField] private bool shouldFadeOut = false;
        [SerializeField] private Fader canvasFader = null;
        [SerializeField] CursorSet cursorSet = null;

        public UnityAction action;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<SavingWrapper>().Delete();
                FindObjectOfType<Fader>().FadeOut(fadeTime);

                LoadMenu();
            }
        }

        private void Start()
        {
            if (shouldFadeOut && canvasFader != null)
            {
                Destroy(GameObject.Find("PersistentObjects(Clone)"));
                canvasFader.FadeOut(fadeTime);
                cursorSet.SetCursor(CursorType.UI);
            }
        }

        public void LoadMenu()
        {
            SceneManager.LoadSceneAsync("Menu");
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}



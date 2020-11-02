using UnityEngine;
using System.Collections;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

        }
        
        /// <summary>
        /// Coroutine that increases alpha of a canvas group up to one within the certain time period 
        /// for creating a fade effect.
        /// </summary>
        /// <param name="time"></param>
        public IEnumerator FadeOut(float time)
        {
            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        /// <summary>
        /// Coroutine that decreases alpha of a canvas group up to one within the certain time period 
        /// for creating a fade effect.
        /// </summary>
        /// <param name="time"></param>
        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
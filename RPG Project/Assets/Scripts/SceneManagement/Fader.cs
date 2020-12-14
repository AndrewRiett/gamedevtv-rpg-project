using UnityEngine;
using System.Collections;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private Coroutine currentCoroutine = null;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        /// <summary>
        /// Coroutine that increases alpha of a canvas group up to one within a time period 
        /// for creating a fade out effect.
        /// </summary>
        /// <param name="time"></param>
        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }

        /// <summary>
        /// Coroutine that decreases alpha of a canvas group up to one within a time period 
        /// for creating a fade in effect.
        /// </summary>
        /// <param name="time"></param>
        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        /// <summary>
        /// Coroutine that changes alpha of a canvas group up to the alphaTarget value 
        /// within a time period for creating a fade effect.
        /// </summary>
        /// <param name="alphaTarget"></param>
        /// <param name="time"></param>
        public Coroutine Fade(float alphaTarget, float time)
        {
            // Cancel current coroutine
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            // Start Fade Coroutine and wait for Fade Coroutine to finish
            currentCoroutine = StartCoroutine(FadeRoutine(alphaTarget, time));
            return currentCoroutine;
        }

        private IEnumerator FadeRoutine(float alphaTarget, float time)
        {
            // true if alpha != alphaTarget, approximately 
            while (!Mathf.Approximately(canvasGroup.alpha, alphaTarget))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alphaTarget, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
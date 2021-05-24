using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Runtime.NeUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFade : MonoBehaviour
    {
        [SerializeField] 
        private float fadeDuration;
        [SerializeField]
        private float appearFadeDelay = 0;
        [SerializeField]
        private float disappearFadeDelay = 0;
        [SerializeField]
        private bool startFromBegin = false;
        [SerializeField, FoldoutGroup("Events")] 
        private UnityEvent onFadeIn;
        [SerializeField, FoldoutGroup("Events")] 
        private UnityEvent onFadeOut;
        [Space]
        [SerializeField, FoldoutGroup("Events")] 
        private UnityEvent onBeginFadeIn;
        [SerializeField, FoldoutGroup("Events")] 
        private UnityEvent onBeginFadeOut;

        
        private CanvasGroup canvasGroup;

        public void FadeToggle()
        {
            Fade(!gameObject.activeSelf);
        }
        public void Fade(bool appear)
        {
            if (!this.gameObject.activeInHierarchy)
            {
                if (appear) this.gameObject.SetActive(true);
                else return;
            }
            if(appear) onBeginFadeIn.Invoke();
            else onBeginFadeOut.Invoke();
            canvasGroup = GetComponent<CanvasGroup>();
            if(fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeRoutine(appear));
        }
        
        private Coroutine fadeRoutine;
        private IEnumerator FadeRoutine(bool appear)
        {
            var delay = appear ? appearFadeDelay : disappearFadeDelay;
            float begin = appear ? 0 : 1;
            float objective = appear ? 1 : 0;
            canvasGroup.interactable = appear;
            float percentage = 0;
            if(!startFromBegin) percentage = canvasGroup.alpha == objective ? 0 : Mathf.InverseLerp(begin, objective, canvasGroup.alpha);
            else canvasGroup.alpha = Mathf.Lerp(begin, objective, percentage);
            if (delay != 0) yield return new WaitForSeconds(delay);

            while (percentage < 1)
            {
                canvasGroup.alpha = Mathf.Lerp(begin, objective, percentage);
                percentage += Time.deltaTime / fadeDuration;
                yield return null;
            }
            if(appear) onFadeIn.Invoke();
            else onFadeOut.Invoke();
            canvasGroup.alpha = objective;
            this.gameObject.SetActive(appear);
        }
    }
}
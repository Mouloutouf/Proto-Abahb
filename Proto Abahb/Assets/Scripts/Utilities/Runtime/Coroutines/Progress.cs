using System;
using System.Collections;
using Game.Utilities.Easing;
using UnityEngine;

namespace Utilities.Coroutines
{
    public static class Coroutines
    {
        public static IEnumerator Progress(float duration, Easing.Functions easing, Action<float> onProgress, Action onComplete)
        {
            float percentage = 0;
            float progress = 0;
            while (percentage < 1)
            {
                onProgress.Invoke(progress);
                percentage = Mathf.Clamp(percentage + Time.deltaTime / duration, 0, 1);
                progress = Easing.Interpolate(percentage, easing);
                yield return null;
            }
            onProgress.Invoke(1);
            onComplete.Invoke();
        }
    }
}
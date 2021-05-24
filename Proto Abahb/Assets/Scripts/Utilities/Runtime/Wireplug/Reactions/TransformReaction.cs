using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utilities.Wireplug.Reactions
{
    public class TransformReaction : ReactionComponent
    {
        [SerializeField, FoldoutGroup("Parameters")]
        private Transform target;
        [SerializeField, FoldoutGroup("Parameters")]
        private MovementMode mode;
        [SerializeField, FoldoutGroup("Parameters")]
        private float translationDuration;
        [SerializeField, FoldoutGroup("Parameters"), ShowIf("@mode.HasFlag(MovementMode.Position)")]
        private MovementInfo position;
        [SerializeField, FoldoutGroup("Parameters"), ShowIf("@mode.HasFlag(MovementMode.Rotation)")]
        private MovementInfo rotation;
        [SerializeField, FoldoutGroup("Parameters"), ShowIf("@mode.HasFlag(MovementMode.Scale)")]
        private MovementInfo scale;

        private float percentage;
        private Coroutine coroutine;
        [Flags]
        private enum MovementMode
        {
            Position = 1,
            Rotation = 2,
            Scale = 4
        }

        private void Awake()
        {
            if (target == null) target = transform;
            position.SetOrigin(target.localPosition);
            rotation.SetOrigin(target.localRotation.eulerAngles);
            scale.SetOrigin(target.localScale);

        }

        public override void OnReact()
        {
            if(coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(Translation(true));
        }

        public override void OnUnreact()
        {
            if(coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(Translation(false));
        }

        private IEnumerator Translation(bool toDestination)
        {
            if (toDestination)
            {
                while (percentage < 1)
                {
                    Translate(percentage);
                    percentage = Mathf.Clamp(percentage + Time.deltaTime / translationDuration, 0, 1);
                    yield return null;
                }
            }
            else
            {
                while (percentage > 0)
                {
                    Translate(percentage);
                    percentage = Mathf.Clamp(percentage - Time.deltaTime / translationDuration, 0, 1);
                    yield return null;
                }
            }
            Translate(toDestination ? 1 : 0);

            void Translate(float percentage)
            {
                if (mode.HasFlag(MovementMode.Position))
                {
                    switch(position.reference){
                        case MovementInfo.TranslationType.Absolute:
                            target.position = position.Evaluate(percentage);
                            break;
                        case MovementInfo.TranslationType.Local:
                            target.localPosition = position.Evaluate(percentage);
                            break;
                        case MovementInfo.TranslationType.Relative:
                            target.localPosition = position.origin + position.Evaluate(percentage);
                            break;
                    }
                }
                if (mode.HasFlag(MovementMode.Rotation))
                {
                    switch(rotation.reference){
                        case MovementInfo.TranslationType.Absolute:
                            target.rotation = Quaternion.Euler(rotation.Evaluate(percentage));
                            break;
                        case MovementInfo.TranslationType.Local:
                            target.localRotation = Quaternion.Euler(rotation.Evaluate(percentage));
                            break;
                        case MovementInfo.TranslationType.Relative:
                            target.localRotation = Quaternion.Euler(rotation.origin + rotation.Evaluate(percentage));
                            break;
                    }
                }
                if (mode.HasFlag(MovementMode.Scale))
                {
                    switch(scale.reference){
                        case MovementInfo.TranslationType.Absolute:
                        case MovementInfo.TranslationType.Local:
                            target.localScale = scale.Evaluate(percentage);
                            break;
                        case MovementInfo.TranslationType.Relative:
                            target.localScale = scale.origin + scale.Evaluate(percentage);
                            break;
                    }
                }
            }
        }
        
        [Serializable]
        private class MovementInfo
        {
            public enum TranslationType
            {
                Local,
                Relative,
                Absolute,
            }

            public TranslationType reference = TranslationType.Local;
            [NonSerialized] public Vector3 origin;

            public Vector3 remap0;
            public Vector3 remap1;
            public bool useSingleCurve = true;
            [ShowIf("useSingleCurve")]
            public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            
            public bool animateX = true;
            [ShowIf("@animateX && !useSingleCurve")]
            public AnimationCurve curveX = AnimationCurve.EaseInOut(0, 0, 1, 1);
            public bool animateY = true;
            [ShowIf("@animateY && !useSingleCurve")]
            public AnimationCurve curveY = AnimationCurve.EaseInOut(0, 0, 1, 1);
            public bool animateZ = true;
            [ShowIf("@animateZ && !useSingleCurve")]
            public AnimationCurve curveZ = AnimationCurve.EaseInOut(0, 0, 1, 1);

            private bool animateAll => animateX && animateY && animateZ;

            public void SetOrigin(Vector3 local)
            {
                origin = local;
            }
            
            public Vector3 Evaluate(float percentage)
            {
                Vector3 vec = remap0;
                
                if (useSingleCurve)
                {
                    if(animateAll) return Vector3.Lerp(remap0, remap1, curve.Evaluate(percentage));
                    else
                    {
                        if (animateX) vec.x = curve.Evaluate(percentage);
                        if (animateY) vec.y = curve.Evaluate(percentage);
                        if (animateZ) vec.z = curve.Evaluate(percentage);
                    }
                }
                else
                {
                    if (animateX) vec.x = curveX.Evaluate(percentage);
                    if (animateY) vec.y = curveY.Evaluate(percentage);
                    if (animateZ) vec.z = curveZ.Evaluate(percentage);
                }

                if (animateX) vec.x = Remap(vec.x, 0f, 1f, remap0.x, remap1.x);
                if (animateY) vec.y = Remap(vec.y, 0f, 1f, remap0.y, remap1.y);
                if (animateZ) vec.z = Remap(vec.z, 0f, 1f, remap0.z, remap1.z);
                
                return vec;
            }
            
            public float Remap(float x, float A, float B, float C, float D)
            {
                float remappedValue = C + (x - A) / (B - A) * (D - C);
                return remappedValue;
            }
        }
        
        
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using MPUIKIT;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class RadialGauge : MonoBehaviour
    {
        #region References

        [SerializeField, FoldoutGroup("References")]
        private Gauge[] gauges = new Gauge[3];
        #endregion
        
        #region Settings
        [SerializeField, FoldoutGroup("Settings")]
        private Color fillColor;
        [SerializeField, FoldoutGroup("Settings")]
        private Gradient decreaseGradient;
        [SerializeField, FoldoutGroup("Settings")]
        private Color backgroundColor;
        [Space] 
        [SerializeField, FoldoutGroup("Settings")]
        private float increaseFillRate = 0.05f;
        [SerializeField, FoldoutGroup("Settings")]
        private float decreaseFillRate = 0.01f;
        [SerializeField, FoldoutGroup("Settings")]
        private float maxDecreaseOffset;
        
        [SerializeField]
        private bool setMaxAtStart;
        [SerializeField, ShowIf("setMaxAtStart")]
        private int maxAtStart = 175;
        #endregion

        #region Events
        public UnityEvent droppedAtZero;

        #endregion

        #region Runtime Values
        [NonSerialized, HideInInspector]
        public int PrevValue;
        [NonSerialized, HideInInspector]
        public int Value;
        [NonSerialized, HideInInspector]
        public int Max;

        #endregion

        private void Start()
        {
            if(setMaxAtStart) SetGauges(maxAtStart);
        }

        public int GetTotalMax()
        {
            int totalMax = 0;
            foreach (var gauge in gauges)
            {
                totalMax += gauge.MaxAmount;
            }
            return totalMax;
        }
        [Button]
        public void SetGauges(int maxAmount)
        {
            maxAmount = Mathf.Clamp(maxAmount, 0, GetTotalMax());
            Max = maxAmount;
            Value = maxAmount;
            
            int currentAmount = 0;
            for (int i = 0; i < gauges.Length; i++)
            {
                ref Gauge gauge = ref gauges[i];
                if (currentAmount < maxAmount)
                {
                    var needed = Mathf.Min(maxAmount - currentAmount, gauge.MaxAmount);
                    SetGauge(ref gauge, currentAmount, needed);
                    currentAmount += needed;
                    
                }
                else
                {
                    DisableGauge(ref gauge);
                }
            }
        }

        private void SetGauge(ref Gauge gauge, int beginAt, int max)
        {
            gauge.CurrentMax = max;
            gauge.CurrentAmount = max;
            gauge.PrevAmount = max;
            gauge.Range = new Vector2Int();
            gauge.Range.x = beginAt;
            gauge.Range.y = beginAt + max;
            gauge.active = true;
            
            var fillAmount = gauge.MaxFillAmount();
            foreach (var image in gauge.AllImages())
            {
                image.fillAmount = fillAmount;
            }

            gauge.Background.color = backgroundColor;
            gauge.Filling.color = decreaseGradient.Evaluate(0);
            gauge.Fill.color = fillColor;
        }

        private void DisableGauge(ref Gauge gauge)
        {
            gauge.CurrentMax = 0;
            gauge.CurrentAmount = 0;
            gauge.PrevAmount = 0;
            gauge.Range = Vector2Int.zero;
            gauge.active = false;

            foreach (var image in gauge.AllImages())
            {
                image.fillAmount = 0;
            }
        }
        
        [Button]
        public void SetAmount(int amount)
        {
            amount = Mathf.Clamp(amount, 0, Max);
            if (Value == amount) return;
            PrevValue = Value;
            Value = amount;

            int prevGaugeIndex = -1;
            int currGaugeIndex = -1;
            for (int i = 0; i < gauges.Length; i++)
            {
                Gauge gauge = gauges[i];
                if (currGaugeIndex < 0 && Value >= gauge.Range.x && Value <= gauge.Range.y)
                {
                    currGaugeIndex = i;
                }
                if (PrevValue >= gauge.Range.x && PrevValue <= gauge.Range.y)
                {
                    prevGaugeIndex = i;
                }
            }

            if (prevGaugeIndex == currGaugeIndex && currGaugeIndex >= 0)
            {
                Gauge gauge = gauges[currGaugeIndex];
                StartGaugeRoutine(currGaugeIndex,Value - gauge.Range.x);
            }
            else
            {
                StartGaugeRoutine(prevGaugeIndex, PrevValue > Value ? 0 : gauges[prevGaugeIndex].Range.y);
                StartGaugeRoutine(currGaugeIndex, Value - gauges[currGaugeIndex].Range.x);
            }

            void StartGaugeRoutine(int index, int value)
            {
                ref Gauge gauge = ref gauges[index];
                gauge.PrevAmount = gauge.CurrentAmount;
                gauge.CurrentAmount = value;
                if (gauge.activeCoroutine != null) StopCoroutine(gauge.activeCoroutine);
                gauge.activeCoroutine = StartCoroutine(FillingRoutine(gauge));
            }
            
            if(Value == 0) droppedAtZero.Invoke();
        }
        [Button]
        public void SetRelative(int amount)
        {
            amount = Mathf.Clamp(Value + amount, 0, Max);
            SetAmount(amount);
        }
        [Button]
        private void ContinuousMod(int value, float rate)
        {
            if (value == 0) return;
            if(continousCoroutine != null) StopCoroutine(continousCoroutine);
            continousCoroutine = StartCoroutine(ContinuousRoutine(value, rate));
        }
        
        [SerializeField] 
        private int relativeAmount = 2;
        [ButtonGroup()]
        private void Increase()
        {
            SetRelative(relativeAmount);
        }
        [ButtonGroup()]
        private void Decrease()
        {
            SetRelative(-relativeAmount);
        }



        private IEnumerator FillingRoutine(Gauge gauge)
        {
            var currentFill = gauge.CurrentFillAmount();
            bool increase = PrevValue < Value;
            if (increase)
            {
                if (increaseFillRate != 0)
                {
                    while (gauge.Fill.fillAmount < currentFill)
                    {
                        gauge.Fill.fillAmount += increaseFillRate;
                        yield return null;
                    }
                }
                gauge.Fill.fillAmount = currentFill;
                gauge.Filling.fillAmount = currentFill;
            }
            else
            {
                gauge.Fill.fillAmount = currentFill;
                if (gauge.Filling.fillAmount - currentFill > maxDecreaseOffset)
                    gauge.Filling.fillAmount = currentFill + maxDecreaseOffset;
                
                while (gauge.Filling.fillAmount > currentFill)
                {
                    gauge.Filling.fillAmount -= decreaseFillRate;
                    yield return null;
                }
                gauge.Filling.fillAmount = currentFill;
            }
            /*
            if (increase)
            {
                gauge.Filling.fillAmount = currentFill;
                if (increaseFillRate != 0)
                {
                    while (gauge.Fill.fillAmount < currentFill)
                    {
                        gauge.Fill.fillAmount = Mathf.Clamp(gauge.Fill.fillAmount + increaseFillRate, 0, currentFill);
                        yield return null;
                    }
                }
                gauge.Fill.fillAmount = currentFill;
            }
            else
            {
                if (gauge.Filling.fillAmount - currentFill > maxDecreaseOffset)
                    gauge.Filling.fillAmount = currentFill + maxDecreaseOffset;
                
                if (decreaseFillRate != 0)
                {
                    while (gauge.Filling.fillAmount > currentFill)
                    {
                        gauge.Filling.fillAmount = Mathf.Clamp(gauge.Filling.fillAmount - decreaseFillRate, currentFill, 1);
                        yield return null;
                    }
                }
                gauge.Filling.fillAmount = currentFill;
            }*/
            yield return null;
        }

        private Coroutine continousCoroutine;
        private IEnumerator ContinuousRoutine(int value, float rate)
        {
            int objective = value > 0 ? Max : 0;
            while (Value != objective)
            {
                SetRelative(1 * (value > 0 ? 1 : -1));
                yield return new WaitForSeconds(rate);
            }
        }

        [System.Serializable]
        private struct Gauge
        {
            public int MaxAmount;
            [HideInPlayMode]
            public MPImage Background;
            [HideInPlayMode]
            public MPImage Filling;
            [HideInPlayMode]
            public MPImage Fill;
            [ShowInInspector, HideInEditorMode, NonSerialized]
            public int CurrentMax;
            [ShowInInspector, HideInEditorMode, NonSerialized]
            public int CurrentAmount;
            [ShowInInspector, HideInEditorMode, NonSerialized]
            public int PrevAmount;
            [ShowInInspector, HideInEditorMode, NonSerialized]
            public Vector2Int Range;
            [ShowInInspector, HideInEditorMode, NonSerialized]
            public bool active;
            [ShowInInspector, HideInEditorMode, NonSerialized]
            public Coroutine activeCoroutine;

            public List<MPImage> AllImages()
            {
                return new List<MPImage>()
                {
                    Background,
                    Filling,
                    Fill
                };
            }

            public float MaxFillAmount()
            {
                return CurrentMax / (float)MaxAmount;
            }
            
            public float CurrentFillAmount()
            {
                return CurrentAmount / (float)MaxAmount;
            }
        }
    }
}

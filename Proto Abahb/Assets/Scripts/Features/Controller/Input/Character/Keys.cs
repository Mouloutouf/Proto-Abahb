using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controller.Input.Character
{
    [Serializable]
    public class DirectionalKeys
    {
        [BoxGroup, ShowInInspector]
        public AxisKey left;
        [BoxGroup, ShowInInspector]
        public AxisKey right;
        [BoxGroup, ShowInInspector]
        public AxisKey up;
        [BoxGroup, ShowInInspector]
        public AxisKey down;
        
        private List<AxisKey> keys = new List<AxisKey>();
        public DirectionalKeys()
        {
            left = new AxisKey("left", -1);
            right = new AxisKey("right", 1);
            up = new AxisKey("up", 1);
            down = new AxisKey("down", -1);
            keys.Add(left);
            keys.Add(right);
            keys.Add(up);
            keys.Add(down);
        }

        public void Evaluate(float vertical, float horizontal)
        {
            left.Evaluate(horizontal);
            right.Evaluate(horizontal);
            up.Evaluate(vertical);
            down.Evaluate(vertical);
        }
    }
    [Serializable]
    public abstract class Key
    {
        [BoxGroup, HorizontalGroup("Top")]
        public string name;
        [BoxGroup, HorizontalGroup("Top")]
        public KeyState state;
        [BoxGroup, HorizontalGroup("Middle")]
        public float Value
        {
            get => value;
            set
            {
                prevValue = this.value;
                this.value = value;
            }
        }
        
        [BoxGroup, HorizontalGroup("Middle")]
        public float PrevValue
        {
            get => prevValue;
        }
        private float value;
        private float prevValue;

        public float heldTime;
        public Key(string _name)
        {
            name = _name;
        }

        public abstract void Evaluate(float value);
    }

    public class AxisKey : Key
    {
        [BoxGroup]
        public float sign;
        
        public AxisKey(string _name, float _sign) : base(_name)
        {
            sign = _sign;
        }

        public override void Evaluate(float value)
        {
            Value = value;
            if (Value != PrevValue)
            {
                if (Value == 0) state = KeyState.Released;
                else if (Mathf.Sign(Value) == sign) state = KeyState.Pressed;
                else if (state == KeyState.Pressed || state == KeyState.On) state = KeyState.Released;
            }
            else
            {
                if (Value == 0) state = KeyState.Idle;
                else if (Mathf.Sign(Value) == sign) state = KeyState.On;
                else state = KeyState.Idle;
            }

            switch (state)
            {
                case KeyState.Idle:
                    break;
                case KeyState.Pressed:
                    break;
                case KeyState.On:
                    heldTime += Time.deltaTime;
                    break;
                case KeyState.Released:
                    heldTime = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public enum KeyState
    {
        Idle,
        Pressed,
        On,
        Released
    }
}
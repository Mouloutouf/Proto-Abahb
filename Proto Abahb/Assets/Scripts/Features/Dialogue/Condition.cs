using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Features.Dialogue
{
    public class Condition
    {
        [Serializable]
        public struct Check
        {
            public Type Condition;
            public CheckType CheckType;
            [ShowIf("@(int)CheckType >= 2")]
            public int Number;
        }

        [Serializable]
        public struct Result
        {
            public Type Condition;
            public CheckType ModType;
            [ShowIf("@(int)CheckType >= 2")]
            public int Number;
        }


        public enum Type
        {
            Unassigned,
            Unknown,
        }

        public enum CheckType
        {    
            False = 0,
            True = 1,
            Above = 2,
            Under = 4,
            Equal = 8,
        }

        public enum ModType
        {
            False = 0,
            True = 1,
            Set = 2,
            Increase = 4,
            Decrease = 8,
        }
    }
}
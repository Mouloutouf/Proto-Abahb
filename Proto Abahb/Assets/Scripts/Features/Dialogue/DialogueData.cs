using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Features.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue", order = 0)]
    public class DialogueData : SerializedScriptableObject
    {
        public List<Dialogue.Frame> dialogueFrames = new List<Dialogue.Frame>();
        public Dialogue.Frame this[int key] => dialogueFrames[key];
    }


    public class Dialogue
    {
        [Serializable]
        public struct Frame
        {
            [HorizontalGroup("Character"), LabelWidth(75)]
            public CharacterName Character;
            [HorizontalGroup("Character"), LabelWidth(75)]
            public Sprite Sprite;
            [TextArea(2,6)]
            public string Text;
            [Space]
            public bool HasChoice;
            [ShowIf("HasChoice")]
            public List<Choice> Choices;
        }

        [Serializable]
        public struct Choice
        {
            public string Text;
            public bool ShowIf;
            [ShowIf("ShowIf")]
            public List<Condition.Check> Conditions;
            public List<Condition.Result> Results;
            public List<Dialogue.Branch> Branches;
        }
    
        [Serializable]
        public struct Branch
        {
            public List<Condition.Check> Conditions;
            public DialogueData Dialogue;
        }
    }
}
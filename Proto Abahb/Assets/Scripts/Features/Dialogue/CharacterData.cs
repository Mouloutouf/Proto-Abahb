using UnityEngine;

namespace Features.Dialogue
{
    [CreateAssetMenu(fileName = "Character", menuName = "Dialogue/Character", order = 0)]
    public class CharacterData : ScriptableObject
    {
        public string Name;
        public string DisplayName;
    }
}
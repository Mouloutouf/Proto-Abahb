using UnityEngine;
using Utilities.Databases;

namespace Features.Dialogue
{
    [CreateAssetMenu(fileName = "Character Database", menuName = "Dialogue/Character Database", order = 0)]
    public class CharacterDB : Database<CharacterName, CharacterData>
    {
        
    }
}
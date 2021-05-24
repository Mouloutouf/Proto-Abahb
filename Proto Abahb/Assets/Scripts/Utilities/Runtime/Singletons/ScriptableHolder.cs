using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Singletons
{
    public class ScriptableHolder : MonoBehaviour
    {
        [SerializeField]
        private List<ScriptableObject> singletonAssets = new List<ScriptableObject>();
    }
}
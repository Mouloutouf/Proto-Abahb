using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Utilities.Wireplug.Reactions
{
    public class OnSceneReaction : ReactionComponent
    {
        [SerializeField] 
        private List<Reaction> reactions = new List<Reaction>();
        [SerializeField] 
        private UnityEvent defaultReaction;
        
        public override void OnReact()
        {
            var scene = SceneManager.GetActiveScene();

            var reaction = reactions.Where(r => r.Match(scene));
            if (reaction.Any())
            {
                reaction.First().reaction.Invoke();
            }
            else defaultReaction.Invoke();
        }

        public override void OnUnreact() { }

        [System.Serializable]
        private struct Reaction
        {
            public SceneType Mode;
            [ShowIf("Mode", Value = SceneType.SceneName)] 
            public string SceneName;
            [ShowIf("Mode", Value = SceneType.BuildIndex)]
            public int BuildIndex;

            public UnityEvent reaction;
            public enum SceneType
            {
                BuildIndex,
                SceneName
            }

            public bool Match(Scene scene)
            {
                switch (Mode)
                {
                    case SceneType.BuildIndex:
                        return scene.buildIndex == BuildIndex;
                        break;
                    case SceneType.SceneName:
                        return scene.name == SceneName;
                        break;
                    default:
                        return false;
                }
            }
        }
    }
}
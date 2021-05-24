using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Wireplug.Reactions
{
    public class EventReaction : ReactionComponent
    {
        [SerializeField, FoldoutGroup("Reactions")]
        private List<UnityEvent> onReact = new List<UnityEvent>();
        [SerializeField, FoldoutGroup("Reactions"), ShowIf("@needAllTriggers || needsAtLeast")]
        private List<UnityEvent> onUnreact = new List<UnityEvent>();

        public override void OnReact()
        {
            foreach (var reaction in onReact)
            {
                reaction.Invoke();
            }
        }
        public override void OnUnreact()
        {
            foreach (var reaction in onUnreact)
            {
                reaction.Invoke();
            }
        }
    }
}

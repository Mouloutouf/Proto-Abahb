using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Random;
using Utilities.Wireplug;
using Random = Utilities.Random.Random;

public class SoundReaction : ReactionComponent
{
    [SerializeField, FoldoutGroup("Parameters")]
    private AudioSource source;
    [SerializeField, FoldoutGroup("Parameters"), ShowIf("@!useRandomClips")]
    private AudioClip clip;
    [SerializeField, FoldoutGroup("Parameters"), ShowIf("@useRandomClips && !useWeightedRandom")]
    private List<AudioClip> clips = new List<AudioClip>();
    [SerializeField, FoldoutGroup("Parameters"), ShowIf("@useRandomClips && useWeightedRandom")]
    private List<WeightedClip> weightedClips = new List<WeightedClip>();
    [Space]
    [SerializeField, FoldoutGroup("Parameters")]
    private bool useRandomClips;
    [SerializeField, FoldoutGroup("Parameters"), ShowIf("useRandomClips")]
    private bool useWeightedRandom;

    private int totalWeight;
    private ShuffleBagCollection<float> clipBag;
    private Random rand = new Random();
    private void Start()
    {
        if (source == null && !TryGetComponent(out source))
        {
            source = this.gameObject.AddComponent<AudioSource>();
        }

        if (useRandomClips && useWeightedRandom)
        {
            Dictionary<float, int> values = new Dictionary<float, int>();
            for (int i = 0; i < weightedClips.Count; i++)
            {
                values.Add(i, weightedClips[i].weight);
            }
            clipBag = rand.ShuffleBag(values);
        }
        
    }

    public override void OnReact()
    {
        if (!useRandomClips)
        {
            source.PlayOneShot(clip);
        }
        else if (!useWeightedRandom)
        {
            source.PlayOneShot(clips.RandomItem());
        }
        else
        {
            source.PlayOneShot(weightedClips[Mathf.CeilToInt(clipBag.Next())].clip);
        }
    }

    public override void OnUnreact()
    {
        throw new System.NotImplementedException();
    }
    
    
    [Serializable]
    private struct WeightedClip
    {
        public AudioClip clip;
        public int weight;
    }
}

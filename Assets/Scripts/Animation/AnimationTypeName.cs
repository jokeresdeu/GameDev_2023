using System;
using Spine.Unity;
using UnityEngine;

namespace Animation
{
    [Serializable]
    public class AnimationTypeName
    {
        [field: SerializeField] public AnimationType Type { get; private set; }
        [field: SerializeField, SpineAnimation] public string Name { get; private set; }
    }
}
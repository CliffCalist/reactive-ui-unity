using System.Collections.Generic;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public sealed class UIGridGroup<TElement>
        where TElement : Component
    {
        public Transform Root;
        public readonly List<TElement> Elements = new();
        public readonly List<Transform> Placeholders = new();



        public UIGridGroup()
        { }

        public UIGridGroup(Transform root)
        {
            Root = root;
        }
    }
}
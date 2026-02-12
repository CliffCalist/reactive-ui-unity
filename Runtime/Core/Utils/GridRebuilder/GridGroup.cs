using System.Collections.Generic;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Core
{
    public sealed class GridGroup<TElement>
        where TElement : Component
    {
        public Transform Root;
        public readonly List<TElement> Elements = new();
        public readonly List<Transform> Placeholders = new();



        public GridGroup()
        { }

        public GridGroup(Transform root)
        {
            Root = root;
        }
    }
}
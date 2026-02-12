using System;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Core
{
    public sealed class GridRebuilderConfig<TData, TElement>
    {
        // GROUP
        public int ElementsPerGroup;
        public Transform GroupsContent;
        public Transform GroupPrefab;
        public Func<Transform> CreateGroup;
        public Action<Transform> DestroyGroup;

        // ELEMENT
        public TElement ElementPrefab;
        public Func<TElement> CreateElement;
        public Action<TElement> DestroyElement;
        public Action<TElement, int, TData> BindElement;

        // PLACEHOLDER
        public Transform PlaceholderPrefab;
        public Func<Transform> CreatePlaceholder;
        public Action<Transform> DestroyPlaceholder;
    }
}
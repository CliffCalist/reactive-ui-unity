using System;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public sealed class UIListRebuildConfig<TData, TUIElement>
    where TUIElement : Component
    {
        /// <summary>Prefab used when Create is not provided.</summary>
        public TUIElement Prefab;

        /// <summary>Parent used when Create is not provided.</summary>
        public Transform Parent;

        /// <summary>Custom creation logic (optional). If null → prefab is instantiated.</summary>
        public Func<TUIElement> Create;

        /// <summary>Custom destroy logic (optional). If null → Destroy(gameObject).</summary>
        public Action<TUIElement> Destroy;

        /// <summary>Binding logic (optional).</summary>
        public Action<TUIElement, int, TData> Bind;
    }
}
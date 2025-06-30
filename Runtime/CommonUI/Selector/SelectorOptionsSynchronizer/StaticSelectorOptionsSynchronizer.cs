using System;
using System.Collections.Generic;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    [Serializable]
    public class StaticSelectorOptionsSynchronizer<T> : ISelectorOptionsSynchronizer<T>
        where T : SelectorOption
    {
        [SerializeField] private Transform _content;



        public void SyncTo(List<T> options)
        {
            options.Clear();
            for (int i = 0; i < _content.childCount; i++)
            {
                if (_content.GetChild(i).TryGetComponent(out T option))
                    options.Add(option);
            }
        }
    }
}
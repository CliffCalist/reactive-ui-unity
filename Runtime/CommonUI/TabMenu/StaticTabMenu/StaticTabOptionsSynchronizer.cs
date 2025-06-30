using System;
using System.Collections.Generic;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    [Serializable]
    public class StaticTabOptionsSynchronizer<T> : ISelectorOptionsSynchronizer<T>
        where T : SelectorOption
    {
        [SerializeField] private List<ViewTabOptionPair<T>> _map;


        public IReadOnlyList<UIView> Tabs => _map.ConvertAll(pair => pair.View);



        public void SyncTo(List<T> options)
        {
            options.Clear();
            foreach (var pair in _map)
            {
                if (pair.Option != null)
                    options.Add(pair.Option);
            }
        }
    }
}

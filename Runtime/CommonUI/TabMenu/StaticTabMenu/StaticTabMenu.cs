using System.Collections.Generic;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public abstract class StaticTabMenu<T> : TabMenu<T>
        where T : SelectorOption
    {
        [SerializeField] private StaticTabOptionsSynchronizer<T> _synchronizer;



        protected override sealed IReadOnlyList<UIView> _tabs => _synchronizer?.Tabs;
        protected override ISelectorOptionsSynchronizer<T> _optionsSynchronizer => _synchronizer;
    }

    public class StaticTabMenu : StaticTabMenu<SelectorOption>
    { }
}
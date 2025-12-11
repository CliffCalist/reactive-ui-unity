using System;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    [Serializable]
    public class TabBarEntry
    {
        [SerializeField] private SelectorOption<UIView> _option;
        [SerializeField] private UIView _tab;



        public SelectorOption<UIView> Option => _option;
        public UIView Tab => _tab;
    }
}
using System;
using UnityEngine;
using UnityEngine.Serialization;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrow.ReactiveUI.Components
{
    [Serializable]
    public class TabBarEntry
    {
        [SerializeField] private SelectorOption<UIView> _option;

        [FormerlySerializedAs("_tab")]
        [SerializeField] private UIView _view;



        public SelectorOption<UIView> Option => _option;
        public UIView View => _view;
    }
}
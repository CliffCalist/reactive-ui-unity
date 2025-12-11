using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public abstract class TabMenu<T> : Selector<T>
        where T : SelectorOption
    {
        [SerializeField] private bool _closeTabsManuallyOnHide;
        [SerializeField, Min(0)] private int _initTabIndex = 0;


        protected abstract IReadOnlyList<UIView> _tabs { get; }

        private IDisposable _disposables;



        protected override void CreateSubscriptions()
        {
            base.CreateSubscriptions();
            SelectOption(_initTabIndex);

            var disposablesBuilder = new DisposableBuilder();

            for (int i = 0; i < _tabs.Count; i++)
            {
                var tab = _tabs[i];
                if (tab == null)
                    continue;

                tab.IsSelfShowed
                    .Subscribe(isShowed => OnTabShowStateChanged(isShowed, tab))
                    .AddTo(ref disposablesBuilder);
            }

            _disposables = disposablesBuilder.Build();
        }

        protected override void DisposeSubscriptions()
        {
            _disposables?.Dispose();
            _disposables = null;
        }



        private void OnTabShowStateChanged(bool isShowed, UIView tab)
        {
            var tabIndex = _tabs.ToList().IndexOf(tab);
            if (isShowed && _selectedIndex.Value != tabIndex)
            {
                SelectOption(tabIndex);
                return;
            }

            var isAllTabsHidden = _tabs.All(t => !t.IsSelfShowed.CurrentValue);
            if (isAllTabsHidden)
                SelectOption(_initTabIndex);
        }



        protected override void OnOptionSelected(int index)
        {
            UpdateTabsVisibility();
        }

        private void UpdateTabsVisibility()
        {
            var selectedTab = _tabs[_selectedIndex.Value];
            if (selectedTab != null && !selectedTab.IsSelfShowed.CurrentValue)
                selectedTab.Show();

            for (int i = 0; i < _tabs.Count; i++)
            {
                if (i == _selectedIndex.Value)
                    continue;

                var tab = _tabs[i];
                if (tab == null)
                    continue;

                if (tab.IsSelfShowed.CurrentValue)
                    tab.Hide();
            }
        }



        protected override void OnHided()
        {
            if (_closeTabsManuallyOnHide)
            {
                foreach (var tab in _tabs)
                {
                    if (tab.IsSelfShowed.CurrentValue)
                        tab.Hide();
                }
            }
        }



#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_tabs != null && _initTabIndex > 0 && _initTabIndex >= _tabs.Count)
            {
                _initTabIndex = _tabs.Count - 1;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}
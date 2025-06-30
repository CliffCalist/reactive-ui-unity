using System;
using System.Collections.Generic;
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



        protected override void BindFromCache()
        {
            base.BindFromCache();
            SelectOption(_initTabIndex);

            var disposablesBuilder = new DisposableBuilder();

            for (int i = 0; i < _tabs.Count; i++)
            {
                var index = i;
                var tab = _tabs[index];
                if (tab == null)
                    continue;

                tab.IsSelfShowed
                    .Where(v => v && _selectedIndex.Value != index)
                    .Subscribe(_ => SelectOption(index))
                    .AddTo(ref disposablesBuilder);
            }

            _disposables = disposablesBuilder.Build();
        }

        protected override void DisposeBinding()
        {
            _disposables?.Dispose();
            _disposables = null;
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



        protected override void OnOptionSelected(int index)
        {
            UpdateTabsVisibility();
        }

        private void UpdateTabsVisibility()
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                var tab = _tabs[i];
                if (tab == null)
                    continue;

                if (i == _selectedIndex.Value && !tab.IsSelfShowed.CurrentValue)
                    tab.Show();
                else if (i != _selectedIndex.Value && tab.IsSelfShowed.CurrentValue)
                    tab.Hide();
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
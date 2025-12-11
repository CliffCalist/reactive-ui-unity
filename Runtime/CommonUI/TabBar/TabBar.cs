using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public class TabBar : Selector<UIView, SelectorOption<UIView>>
    {
        [SerializeField] private bool _closeTabsManuallyOnHide;
        [SerializeField, Min(0)] private int _initTabIndex = 0;
        [SerializeField] private List<TabBarEntry> _tabs;



        public override sealed bool UseAutoConfirm => true;



        protected override sealed List<SelectorOption<UIView>> BuildOptions()
        {
            return _tabs.Select(entry =>
            {
                entry.Option.Bind(entry.Tab);
                return entry.Option;
            }).ToList();
        }

        protected override sealed IDisposable CreateSubscriptionsCore()
        {
            var disposablesBuilder = new DisposableBuilder();

            base.CreateSubscriptionsCore().AddTo(ref disposablesBuilder);
            SelectOption(_initTabIndex);

            for (int i = 0; i < _tabs.Count; i++)
            {
                var tab = _tabs[i].Tab;

                tab.IsSelfShowed
                    .Subscribe(isShowed => OnTabVisibilityChanged(isShowed, tab))
                    .AddTo(ref disposablesBuilder);
            }

            return disposablesBuilder.Build();
        }

        private void OnTabVisibilityChanged(bool isShowed, UIView tab)
        {
            if (isShowed && CurrentSelection.CurrentValue.Item != tab)
            {
                SelectOption(tab);
                return;
            }

            var isAllTabsHidden = _tabs.All(t => !t.Tab.IsSelfShowed.CurrentValue);
            if (isAllTabsHidden)
                SelectOption(_initTabIndex);
        }


        protected override void OnSelectionConfirmed(Selection<UIView> selection)
        {
            UpdateTabsVisibility();
        }

        private void UpdateTabsVisibility()
        {
            var selection = ConfirmedSelection.CurrentValue;

            if (selection.Item != null && !selection.Item.IsSelfShowed.CurrentValue)
                selection.Item.Show();

            for (int i = 0; i < _tabs.Count; i++)
            {
                if (i == selection.Index)
                    continue;

                var tab = _tabs[i].Tab;
                if (tab.IsSelfShowed.CurrentValue)
                    tab.Hide();
            }
        }



        protected override void OnHidedCore()
        {
            if (_closeTabsManuallyOnHide)
            {
                foreach (var tabEntry in _tabs)
                {
                    if (tabEntry.Tab.IsSelfShowed.CurrentValue)
                        tabEntry.Tab.Hide();
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
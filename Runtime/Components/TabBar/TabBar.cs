using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrow.ReactiveUI.Components
{
    public class TabBar : Selector<UIView, SelectorOption<UIView>>
    {
        [SerializeField] private bool _closeTabsManuallyOnHide;
        [SerializeField] private TransitionConfig _transitionConfig;

        [Space]
        [SerializeField, Min(0)] private int _initTabIndex = 0;
        [SerializeField] private List<TabBarEntry> _tabs;



        private ITransitionHandler _tabsTransition;


        public override sealed bool UseAutoConfirm => true;
        public bool IsTabsSwitching => _tabsTransition != null && !_tabsTransition.IsCompleted;



        #region Bindings
        protected override sealed List<SelectorOption<UIView>> BuildOptions(List<SelectorOption<UIView>> currentOptions)
        {
            return _tabs.Select(entry =>
            {
                entry.Option.Bind(entry.View);
                return entry.Option;
            }).ToList();
        }

        protected override sealed void CreateBindings(CompositeDisposable bindings)
        {
            base.CreateBindings(bindings);
            SelectOption(_initTabIndex);

            for (int i = 0; i < _tabs.Count; i++)
            {
                var tab = _tabs[i].View;

                tab.Visibility.IsSelfShowed
                    .Subscribe(_ => HandleExternalTabVisibilityOverride(tab))
                    .AddTo(bindings);
            }
        }

        protected override void OnBindingsCleared()
        {
            _tabsTransition?.Dispose();
        }
        #endregion



        #region Tab transitions
        protected override void OnSelectionConfirmed(Selection<UIView> selection)
        {
            var from = PreviousConfirmedSelection?.Item;
            var to = ConfirmedSelection.CurrentValue?.Item ?? _tabs[_initTabIndex].View;

            _tabsTransition?.Dispose();
            ExecuteTabTransition(from, to);
            ForceHideNonSelectedTabs(from, to);
        }

        private void ExecuteTabTransition(UIView from, UIView to)
        {
            if (from == null)
            {
                if (!to.Visibility.IsSelfShowed.CurrentValue)
                    to.Show();
                return;
            }

            var isStandardSwitch =
                from.Visibility.IsSelfShowed.CurrentValue &&
                !to.Visibility.IsSelfShowed.CurrentValue;

            if (isStandardSwitch)
            {
                _tabsTransition = ViewUtils.Switch(from, to, _transitionConfig);
            }
            else
            {
                if (!to.Visibility.IsSelfShowed.CurrentValue)
                    to.Show();

                if (from.Visibility.IsSelfShowed.CurrentValue)
                    from.Hide();
            }
        }

        private void ForceHideNonSelectedTabs(UIView from, UIView to)
        {
            foreach (var entry in _tabs)
            {
                var tab = entry.View;

                if (tab == from || tab == to)
                    continue;

                if (tab.Visibility.IsSelfShowed.CurrentValue)
                    tab.Hide(true);
            }
        }
        #endregion



        #region External override
        private void HandleExternalTabVisibilityOverride(UIView tab)
        {
            if (IsTabsSwitching)
                return;

            if (tab.Visibility.IsSelfShowed.CurrentValue
                && HasConfirmedSelection.CurrentValue
                && ConfirmedSelection.CurrentValue.Item != tab)
            {
                SelectOption(tab);
                return;
            }

            var isAllTabsHidden = _tabs.All(t => !t.View.Visibility.IsSelfShowed.CurrentValue);
            if (isAllTabsHidden)
                SelectOption(_initTabIndex);
        }
        #endregion



        #region Common
        protected override void OnHidden()
        {
            if (_closeTabsManuallyOnHide)
            {
                foreach (var tabEntry in _tabs)
                {
                    if (tabEntry.View.Visibility.IsSelfShowed.CurrentValue)
                        tabEntry.View.Hide(true);
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
        #endregion
    }
}
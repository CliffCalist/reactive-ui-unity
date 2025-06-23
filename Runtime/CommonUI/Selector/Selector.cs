using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public abstract class Selector : UIView
    {
        [SerializeField] private Transform _content;
        [SerializeField] private SelectorOption _optionPrefab;



        private readonly List<SelectorOption> _options = new();
        protected readonly ReactiveProperty<int> _selectedIndex = new(-1);



        public ReadOnlyReactiveProperty<int> SelectedIndex => _selectedIndex;
        public abstract int TargetOptionsCount { get; }



        protected override void BindFromCache()
        {
            UpdateOptionsCount();
            UpdateOptionsIndexes();
            UpdateOptionsStatus();
        }



        private void UpdateOptionsCount()
        {
            while (_options.Count < TargetOptionsCount)
            {
                var newOption = CreateOption(_optionPrefab);
                newOption.transform.SetParent(_content, false);

                OnOptionPostInstantiated(newOption);
                _options.Add(newOption);

                newOption.Selected
                    .Subscribe(SelectOption)
                    .AddTo(this);
            }

            while (_options.Count > TargetOptionsCount)
            {
                var last = _options.LastOrDefault();
                if (last == null)
                    break;

                _options.Remove(last);
                OnOptionPreDestroy(last);
                Destroy(last.gameObject);
            }
        }

        protected virtual SelectorOption CreateOption(SelectorOption prefab)
        {
            return Instantiate(prefab);
        }

        protected virtual void OnOptionPostInstantiated(SelectorOption option)
        { }

        protected virtual void OnOptionPreDestroy(SelectorOption option)
        { }



        private void UpdateOptionsIndexes()
        {
            for (int i = 0; i < _options.Count; i++)
                _options[i].SetIndex(i);
        }

        private void UpdateOptionsStatus()
        {
            for (int i = 0; i < _options.Count; i++)
            {
                var option = _options[i];
                option.SetSelectedStatus(i == _selectedIndex.CurrentValue);
            }
        }



        public void SelectOption(int index)
        {
            _selectedIndex.Value = index;

            if (IsInHierarchyShowed.CurrentValue)
                UpdateOptionsStatus();
        }
    }
}
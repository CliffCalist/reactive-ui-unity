using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public abstract class Selector<T> : UIView
        where T : SelectorOption
    {
        [SerializeField] private Transform _content;
        [SerializeField] private T _optionPrefab;



        private readonly List<T> _options = new();
        protected readonly ReactiveProperty<int> _selectedIndex = new(-1);



        public abstract int TargetOptionsCount { get; }
        public int OptionsCount => _options.Count;

        public IReadOnlyList<T> Options => _options;

        public ReadOnlyReactiveProperty<int> SelectedIndex => _selectedIndex;



        protected override void BindFromCache()
        {
            UpdateOptionsCount();
            UpdateOptionLinkedIndexes();
            UpdateOptionsStatus();
        }



        private void UpdateOptionsCount()
        {
            while (_options.Count < TargetOptionsCount)
            {
                var newOption = CreateOption(_optionPrefab, _options.Count);
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

        protected virtual T CreateOption(T prefab, int index)
        {
            return Instantiate(prefab);
        }

        protected virtual void OnOptionPostInstantiated(T option)
        { }

        protected virtual void OnOptionPreDestroy(T option)
        { }



        private void UpdateOptionLinkedIndexes()
        {
            for (int i = 0; i < _options.Count; i++)
                _options[i].SetLinkedIndex(i);
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
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace WhiteArrow.MVVM.UI
{
    public abstract class Selector : UIView
    {
        [SerializeField] private Transform _content;
        [SerializeField] private SelectorOption _optionPrefab;

        private readonly List<SelectorOption> _options = new();

        protected readonly ReactiveProperty<int> _selectedIndex = new(-1);
        public ReadOnlyReactiveProperty<int> SelectedIndex => _selectedIndex;



        public abstract int OptionsCount { get; }



        protected override void BindFromCache()
        {
            UpdateOptionsCount();
            UpdateOptionsIndexes();
            UpdateOptionsStatus();
        }



        private void UpdateOptionsCount()
        {
            while (_options.Count < OptionsCount)
            {
                var newOption = Instantiate(_optionPrefab, _content);
                OnOptionInstantiated(newOption);
                _options.Add(newOption);

                newOption.Clicked
                    .Subscribe(OnOptionSelected)
                    .AddTo(this);
            }

            while (_options.Count > OptionsCount)
            {
                var last = _options.LastOrDefault();
                if (last == null)
                    break;

                _options.Remove(last);
                OnOptionDestroyed(last);
                Destroy(last.gameObject);
            }
        }

        protected virtual void OnOptionInstantiated(SelectorOption option) { }
        protected virtual void OnOptionDestroyed(SelectorOption option) { }



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



        private void OnOptionSelected(int index)
        {
            _selectedIndex.Value = index;
            UpdateOptionsStatus();
        }
    }
}
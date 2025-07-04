using System;
using System.Collections.Generic;
using R3;

namespace WhiteArrow.ReactiveUI
{
    public abstract class Selector<T> : UIView
        where T : SelectorOption
    {
        private IDisposable _optionsSubscription;
        private readonly List<T> _options = new();
        protected readonly ReactiveProperty<int> _selectedIndex = new(-1);



        protected abstract ISelectorOptionsSynchronizer<T> _optionsSynchronizer { get; }

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
            if (_optionsSynchronizer == null)
                throw new InvalidOperationException("Options provider cannot be null.");

            _optionsSynchronizer.SyncTo(_options);

            var subscriptionBuilder = new DisposableBuilder();
            foreach (var option in _options)
            {
                option.Selected
                    .Subscribe(SelectOption)
                    .AddTo(ref subscriptionBuilder);
            }

            _optionsSubscription?.Dispose();
            _optionsSubscription = subscriptionBuilder.Build();
        }



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
            if (index < 0 || index >= _options.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            _selectedIndex.Value = index;
            OnOptionSelected(index);

            if (IsSelfShowed.CurrentValue)
                UpdateOptionsStatus();
        }

        protected abstract void OnOptionSelected(int index);
    }
}
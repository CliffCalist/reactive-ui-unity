using System;
using System.Collections.Generic;
using R3;

namespace WhiteArrow.ReactiveUI
{
    public abstract class Selector<TData, TOption> : UIView
        where TOption : SelectorOption<TData>
    {
        private List<TOption> _options = new();
        private readonly ReactiveProperty<Selection<TData>> _currentSelection = new(null);
        private readonly ReactiveProperty<Selection<TData>> _confirmedSelection = new(null);



        public abstract bool UseAutoConfirm { get; }

        public int OptionsCount => _options.Count;
        public IReadOnlyList<TOption> Options => _options;
        public ReadOnlyReactiveProperty<Selection<TData>> CurrentSelection => _currentSelection;
        public ReadOnlyReactiveProperty<Selection<TData>> ConfirmedSelection => _confirmedSelection;




        protected override IDisposable CreateSubscriptionsCore()
        {
            _options = BuildOptions(_options)
                ?? throw new InvalidOperationException("BuildOptions returned null.");

            var subscriptionBuilder = new DisposableBuilder();
            for (var x = 0; x < _options.Count; x++)
            {
                var index = x;
                var option = _options[index];

                option.Clicked
                    .Subscribe(_ => SelectOption(index))
                    .AddTo(ref subscriptionBuilder);
            }

            UpdateOptionsStatus();
            return subscriptionBuilder.Build(); ;
        }

        protected abstract List<TOption> BuildOptions(List<TOption> currentOptions);



        public void SelectOption(TData item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var index = _options.FindIndex(o => o.Item.Equals(item));

            if (index >= 0)
                SelectOption(index);
            else throw new ArgumentException("Item is not part of this selector.", nameof(item));
        }

        public void SelectOption(int index)
        {
            if (index < 0 || index >= _options.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var option = _options[index];
            var selection = new Selection<TData>(index, option.Item);
            _currentSelection.Value = selection;
            OnSelected(selection);

            if (IsSelfShowed.CurrentValue)
                UpdateOptionsStatus();

            if (UseAutoConfirm)
                ConfirmCurrentSelection();
        }

        protected virtual void OnSelected(Selection<TData> selection)
        { }



        public void ConfirmCurrentSelection()
        {
            if (_currentSelection.Value == null)
                throw new InvalidOperationException("Current selection is null.");

            if (_currentSelection.Value == _confirmedSelection.Value)
                return;

            _confirmedSelection.Value = _currentSelection.Value;
            OnSelectionConfirmed(_confirmedSelection.Value);
        }

        protected virtual void OnSelectionConfirmed(Selection<TData> selection) { }


        private void UpdateOptionsStatus()
        {
            var isSelectionExist = _currentSelection.CurrentValue != null;

            for (int i = 0; i < _options.Count; i++)
            {
                var option = _options[i];
                option.OnSelectionChanged(isSelectionExist ?
                    i == _currentSelection.CurrentValue.Index :
                    false
                );
            }
        }
    }
}
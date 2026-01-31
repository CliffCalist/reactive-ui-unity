using System;
using System.Collections.Generic;
using R3;

namespace WhiteArrow.ReactiveUI
{
    public abstract class Selector : UIView
    {
        public abstract bool UseAutoConfirm { get; }
        public abstract int OptionsCount { get; }

        public abstract IReadOnlyList<SelectorOption> UntypedOptions { get; }
        public abstract ReadOnlyReactiveProperty<ISelection> UntypedCurrentSelection { get; }
        public abstract ReadOnlyReactiveProperty<ISelection> UntypedConfirmedSelection { get; }



        public abstract void SelectOption(int index);
        public abstract void ClearSelection();
        public abstract void ConfirmCurrentSelection();
    }



    public abstract class Selector<TData, TOption> : Selector
        where TOption : SelectorOption<TData>
    {
        private List<TOption> _options = new();

        private readonly ReactiveProperty<Selection<TData>> _currentSelection = new(null);
        private ReadOnlyReactiveProperty<ISelection> _untypedCurrentSelection;

        private readonly ReactiveProperty<Selection<TData>> _confirmedSelection = new(null);
        private ReadOnlyReactiveProperty<ISelection> _untypedConfirmedSelection;



        public override sealed int OptionsCount => _options.Count;
        public IReadOnlyList<TOption> Options => _options;

        public override sealed IReadOnlyList<SelectorOption> UntypedOptions => _options;

        public ReadOnlyReactiveProperty<Selection<TData>> CurrentSelection => _currentSelection;
        public override sealed ReadOnlyReactiveProperty<ISelection> UntypedCurrentSelection
        {
            get
            {
                InitIfFalse();
                return _untypedCurrentSelection;
            }
        }

        public ReadOnlyReactiveProperty<Selection<TData>> ConfirmedSelection => _confirmedSelection;
        public override sealed ReadOnlyReactiveProperty<ISelection> UntypedConfirmedSelection
        {
            get
            {
                InitIfFalse();
                return _untypedConfirmedSelection;
            }
        }



        protected override void InitCore()
        {
            _untypedCurrentSelection = _currentSelection
                .Select(v => (ISelection)v)
                .ToReadOnlyReactiveProperty();

            _untypedConfirmedSelection = _confirmedSelection
                .Select(v => (ISelection)v)
                .ToReadOnlyReactiveProperty();
        }

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
            return subscriptionBuilder.Build();
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

        public override sealed void SelectOption(int index)
        {
            if (index < 0 || index >= _options.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var option = _options[index];
            var selection = new Selection<TData>(index, option.Item);
            _currentSelection.Value = selection;
            OnSelectedInternal();
        }

        public override sealed void ClearSelection()
        {
            if (_currentSelection.Value == null)
                return;

            _currentSelection.Value = null;
            OnSelectedInternal();
        }

        private void OnSelectedInternal()
        {
            OnSelected(_currentSelection.Value);

            if (IsSelfShowed.CurrentValue)
                UpdateOptionsStatus();

            if (UseAutoConfirm)
                ConfirmCurrentSelection();
        }

        protected virtual void OnSelected(Selection<TData> selection)
        { }



        public override sealed void ConfirmCurrentSelection()
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
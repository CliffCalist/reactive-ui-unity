using System;
using R3;

namespace WhiteArrow.ReactiveUI
{
    public abstract class SelectorOption : UIButton
    {
        public abstract object UntypedItem { get; }



        private readonly Subject<Unit> _clicked = new();
        public Observable<Unit> Clicked => _clicked;



        protected override sealed void OnClicked()
        {
            _clicked.OnNext(Unit.Default);
        }



        public virtual void OnSelectionChanged(bool isSelected)
        { }
    }

    public abstract class SelectorOption<TData> : SelectorOption
    {
        private TData _item;



        public TData Item => _item;
        public override sealed object UntypedItem => _item;



        public void Bind(TData item)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
            RecreateSubscriptionsIfVisible();
        }
    }
}
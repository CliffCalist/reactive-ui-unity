using System;
using R3;

namespace WhiteArrow.ReactiveUI
{
    public abstract class SelectorOption<TData> : UIButton
    {
        private TData _item;



        public TData Item => _item;


        private readonly Subject<Unit> _clicked = new();
        public Observable<Unit> Clicked => _clicked;




        public void Bind(TData item)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
            RecreateSubscriptionsIfVisible();
        }


        protected override sealed void OnClicked()
        {
            _clicked.OnNext(Unit.Default);
        }



        public virtual void OnSelectionChanged(bool isSelected)
        { }
    }
}
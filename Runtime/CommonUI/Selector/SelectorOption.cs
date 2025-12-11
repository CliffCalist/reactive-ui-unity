using System;
using R3;

namespace WhiteArrow.ReactiveUI
{
    public class SelectorOption : UIButton
    {
        protected int _linkedIndex { get; private set; } = -1;

        private readonly Subject<int> _selected = new();
        public Observable<int> Selected => _selected;




        public void SetLinkedIndex(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            _linkedIndex = index;
            RecreateSubscriptionsIfVisible();
        }


        protected override sealed void OnClicked()
        {
            _selected.OnNext(_linkedIndex);
        }



        public virtual void SetSelectedStatus(bool isSelected)
        { }
    }
}
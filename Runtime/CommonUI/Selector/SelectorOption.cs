using System;
using R3;

namespace WhiteArrow.MVVM.UI
{
    public abstract class SelectorOption : ViewButton
    {
        protected int _index { get; private set; } = -1;

        private readonly Subject<int> _clicked = new();
        public Observable<int> Clicked => _clicked;




        public void SetIndex(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            _index = index;
            RebindIfShowedInHierarchy();
        }


        protected override void OnClicked()
        {
            _clicked.OnNext(_index);
        }



        public abstract void SetSelectedStatus(bool isSelected);
    }
}
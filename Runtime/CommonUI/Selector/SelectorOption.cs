using System;
using R3;

namespace WhiteArrow.ReactiveUI
{
    public abstract class SelectorOption : ViewButton
    {
        protected int _index { get; private set; } = -1;

        private readonly Subject<int> _selected = new();
        public Observable<int> Selected => _selected;




        public void SetIndex(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            _index = index;
            RebindIfShowedInHierarchy();
        }


        protected override void OnClicked()
        {
            _selected.OnNext(_index);
        }



        public abstract void SetSelectedStatus(bool isSelected);
    }
}
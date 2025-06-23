using R3;

namespace WhiteArrow.ReactiveUI
{
    public class TabButton : ViewButton
    {
        private readonly Subject<Unit> _selected = new();
        public Observable<Unit> Clicked => _selected;



        protected override sealed void OnClicked() => _selected.OnNext(Unit.Default);
        public virtual void SetSelectedStatus(bool isSelected) { }
    }
}
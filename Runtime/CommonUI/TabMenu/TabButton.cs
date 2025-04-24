using R3;

namespace WhiteArrow.ReactiveUI
{
    public class TabButton : ViewButton
    {
        private readonly Subject<Unit> _clicked = new();
        public Observable<Unit> Clicked => _clicked;



        protected override sealed void OnClicked() => _clicked.OnNext(Unit.Default);
        public virtual void SetActive(bool isActive) => _object.SetActive(isActive);
    }
}
using R3;
using R3.Triggers;

namespace WhiteArrow.MVVM.UI
{
    public class ObservableViewAnimatorTrigger : ObservableTriggerBase
    {
        private readonly Subject<Unit> _onshowEnded = new();
        public Observable<Unit> OnShowEnded => _onshowEnded;

        private readonly Subject<Unit> _onHideEnded = new();
        public Observable<Unit> OnHideEnded => _onHideEnded;


        public void OnAnimationShowEnded()
        {
            _onshowEnded.OnNext(Unit.Default);
        }

        public void OnAnimationCloseEnded()
        {
            _onHideEnded.OnNext(Unit.Default);
        }


        protected override void RaiseOnCompletedOnDestroy()
        {
            _onHideEnded?.Dispose();
            _onHideEnded?.Dispose();
        }
    }
}

using R3;
using R3.Triggers;

namespace WhiteArrow.MVVM.UI
{
    public class ObservableViewAnimatorTrigger : ObservableTriggerBase
    {
        private readonly Subject<Unit> _showEnded = new();
        public Observable<Unit> ShowEnded => _showEnded;

        private readonly Subject<Unit> _hideEnded = new();
        public Observable<Unit> HideEnded => _hideEnded;


        public void OnAnimationShowEnded()
        {
            _showEnded.OnNext(Unit.Default);
        }

        public void OnAnimationCloseEnded()
        {
            _hideEnded.OnNext(Unit.Default);
        }


        protected override void RaiseOnCompletedOnDestroy()
        {
            _hideEnded?.Dispose();
            _hideEnded?.Dispose();
        }
    }
}

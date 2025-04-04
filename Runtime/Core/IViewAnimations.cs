using R3;

namespace WhiteArrow.MVVM.UI
{
    public interface IViewAnimations
    {
        bool IsEnabled { get; }
        Observable<Unit> OnShowEnded { get; }
        Observable<Unit> OnHideEnded { get; }

        void Init(UIView view);
        void PlayShow();
        void PlayHide();
    }
}
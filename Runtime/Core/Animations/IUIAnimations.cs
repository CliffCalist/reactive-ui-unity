using System;
using R3;

namespace WhiteArrow.ReactiveUI
{
    public interface IUIAnimations : IDisposable
    {
        internal bool IsInitialized { get; }
        bool IsEnabled { get; }

        Observable<Unit> ShowEnded { get; }
        Observable<Unit> HideEnded { get; }



        internal void Init(UIView view);

        internal void PlayShow();
        internal void PlayHide();
    }
}
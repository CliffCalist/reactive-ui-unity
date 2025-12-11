using System;
using R3;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public class AnimatorUIAnimations : MonoUIAnimations
    {
        [SerializeField] private string _showAnimationName = "show";
        [SerializeField] private string _hideAnimationName = "hide";


        private Animator _animator;
        private IDisposable _disposable;



        protected override void InitCore(UIView view)
        {
            if (view is null)
                throw new ArgumentNullException(nameof(view));

            if (!view.TryGetComponent(out _animator))
                throw new NullReferenceException($"The {view.name} doesn't have {nameof(Animator)} component for {nameof(AnimatorUIAnimations)}.");

            if (!view.TryGetComponent(out ObservableUIAnimatorTrigger observableAnimator))
                observableAnimator = view.gameObject.AddComponent<ObservableUIAnimatorTrigger>();

            var disposableBuilder = new DisposableBuilder();
            observableAnimator.ShowEnded.Subscribe(_ => _showEnded.OnNext(Unit.Default)).AddTo(ref disposableBuilder);
            observableAnimator.HideEnded.Subscribe(_ => _hideEnded.OnNext(Unit.Default)).AddTo(ref disposableBuilder);
            _disposable = disposableBuilder.Build();
        }



        protected override void PlayShowCore()
        {
            _animator.Play(_showAnimationName);
        }

        protected override void PlayHideCore()
        {
            _animator.Play(_hideAnimationName);
        }



        protected override void DisposeCore()
        {
            _disposable?.Dispose();
        }
    }
}
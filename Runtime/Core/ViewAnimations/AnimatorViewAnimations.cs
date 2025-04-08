using System;
using R3;
using UnityEngine;

namespace WhiteArrow.MVVM.UI
{
    [Serializable]
    public class AnimatorViewAnimations : ViewAnimations
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
                throw new NullReferenceException($"The {view.name} doesn't have {nameof(Animator)} component for {nameof(AnimatorViewAnimations)}.");

            if (!view.TryGetComponent(out ObservableViewAnimatorTrigger observableAnimator))
                observableAnimator = view.gameObject.AddComponent<ObservableViewAnimatorTrigger>();

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



        public override void Dispose()
        {
            base.Dispose();
            _disposable?.Dispose();
        }
    }
}
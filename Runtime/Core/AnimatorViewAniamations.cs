using System;
using R3;
using UnityEngine;

namespace WhiteArrow.MVVM.UI
{
    [Serializable]
    public class AnimatorViewAniamations : IViewAnimations
    {
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private string _showAnimationName = "show";
        [SerializeField] private string _hideAnimationName = "hide";


        private Animator _animator;
        private bool _isInitialized;


        public bool IsEnabled => _isEnabled;

        private ObservableViewAnimatorTrigger _observableAnimator;
        public Observable<Unit> OnShowEnded => _observableAnimator.OnShowEnded;
        public Observable<Unit> OnHideEnded => _observableAnimator.OnHideEnded;



        public void Init(UIView view)
        {
            if (_isInitialized || !_isEnabled)
                return;

            if (view is null)
                throw new ArgumentNullException(nameof(view));


            if (!view.TryGetComponent(out _animator))
            {
                Debug.LogWarning($"{nameof(AnimatorViewAniamations)} in {view.name} is enabled, but {nameof(GameObject)} don't have {nameof(Animator)} component. {nameof(IsEnabled)} switched to false.", view);
                _isEnabled = false;
                return;
            }


            if (!view.TryGetComponent(out _observableAnimator))
                _observableAnimator = view.gameObject.AddComponent<ObservableViewAnimatorTrigger>();


            _isInitialized = true;
        }

        private bool LogIfNonInitialized()
        {
            if (!_isInitialized)
                Debug.LogError($"The {nameof(AnimatorViewAniamations)} isn't initialized.", _animator);
            return !_isInitialized;
        }



        public void PlayShow()
        {
            if (LogIfNonInitialized())
                return;

            _animator.Play(_showAnimationName);
        }

        public void PlayHide()
        {
            if (LogIfNonInitialized())
                return;

            _animator.Play(_hideAnimationName);
        }
    }
}
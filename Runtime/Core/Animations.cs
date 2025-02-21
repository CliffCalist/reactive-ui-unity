using System;
using UnityEngine;
using R3;

namespace WhiteArrow.MVVM.UI
{
    [Serializable]
    public class Animations
    {
        private bool _isInitialized;


        [SerializeField] private bool _isEnabled = true;
        public bool IsEnabled => _isEnabled;

        private Animator _animator;

        [SerializeField] private string _showAnimationName = "show";
        [SerializeField] private string _hideAnimationName = "hide";


        private ObservableViewAnimatorTrigger _observableAnimator;
        public Observable<Unit> OnShowEnded => _observableAnimator.OnShowEnded;
        public Observable<Unit> OnHideEnded => _observableAnimator.OnHideEnded;



        public void Initialize(UiView view)
        {
            if (_isInitialized || !_isEnabled)
                return;
            if (view is null)
                throw new ArgumentNullException(nameof(view));


            if (!view.TryGetComponent(out _animator))
            {
                Debug.LogWarning($"{nameof(Animations)} in {view.name} is enabled, but {nameof(GameObject)} don't have {nameof(Animator)} component. {nameof(IsEnabled)} switched to false.", view);
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
                Debug.LogError($"The {nameof(Animations)} isn't initialized.", _animator);
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
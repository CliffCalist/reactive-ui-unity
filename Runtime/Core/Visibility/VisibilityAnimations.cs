using System;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Core
{
    public abstract class VisibilityAnimations : MonoBehaviour
    {
        [SerializeField] private bool _useShowAnimation = true;
        [SerializeField] private bool _useHideAnimation = true;



        protected UIView _view { get; private set; }



        public bool IsViewAttached => _view != null;



        internal void AttachView(UIView view)
        {
            if (_view != null)
                throw new InvalidOperationException($"The {_view.GetType().Name} is already attached. Use another {nameof(VisibilityAnimations)}.");

            _view = view ?? throw new ArgumentNullException(nameof(view));
            OnViewAttached();
        }

        protected abstract void OnViewAttached();



        internal void DetachView()
        {
            if (_view == null)
                return;

            _view = null;
            KillCurrentAnimation();
            OnViewDetached();
        }

        protected virtual void OnViewDetached() { }



        internal void PlayShowAnimationInternal(Action onComplete = null)
        {
            LogIfViewNotAttached();

            if (_useShowAnimation)
                PlayShowAnimation(onComplete);
            else
            {
                SetEndStateOfShowAnimation();
                onComplete?.Invoke();
            }
        }

        protected abstract void PlayShowAnimation(Action onComplete = null);
        internal protected abstract void SetEndStateOfShowAnimation();



        internal void PlayHideAnimationInternal(Action onComplete = null)
        {
            LogIfViewNotAttached();

            if (_useHideAnimation)
                PlayHideAnimation(onComplete);
            else
            {
                SetEndStateOfHideAnimation();
                onComplete?.Invoke();
            }
        }

        protected abstract void PlayHideAnimation(Action onComplete = null);
        internal protected abstract void SetEndStateOfHideAnimation();



        internal protected abstract void KillCurrentAnimation();



        internal void LogIfViewNotAttached()
        {
            if (_view == null)
                Debug.LogError($"The {nameof(UIView)} is not attached. Use {nameof(AttachView)} method.");
        }
    }
}
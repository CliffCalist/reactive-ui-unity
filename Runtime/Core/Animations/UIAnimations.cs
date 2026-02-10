using System;
using R3;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public abstract class UIAnimations : IUIAnimations
    {
        [SerializeField] private bool _isEnabled = true;


        private bool _isInitialized;



        bool IUIAnimations.IsInitialized => _isInitialized;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }


        protected readonly Subject<Unit> _showEnded = new();
        public Observable<Unit> ShowEnded => _showEnded;

        protected readonly Subject<Unit> _hideEnded = new();
        public Observable<Unit> HideEnded => _hideEnded;



        void IUIAnimations.Init(UIView view)
        {
            if (_isInitialized)
                throw new Exception($"The {GetType().Name} is already initialized.");

            InitCore(view);
            _isInitialized = true;
        }

        protected abstract void InitCore(UIView view);



        void IUIAnimations.StopAllWithoutNotify()
        {
            ThrowIfNonInitialized();
            StopAllWithoutNotifyCore();
        }

        protected abstract void StopAllWithoutNotifyCore();



        void IUIAnimations.PlayShow()
        {
            ThrowIfNonInitialized();
            PlayShowCore();
        }

        protected abstract void PlayShowCore();



        void IUIAnimations.PlayHide()
        {
            ThrowIfNonInitialized();
            PlayHideCore();
        }

        protected abstract void PlayHideCore();



        protected void ThrowIfNonInitialized()
        {
            if (!_isInitialized)
                throw new Exception($"The {GetType().Name} isn't initialized.");
        }
    }
}
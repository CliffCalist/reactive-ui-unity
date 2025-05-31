using System;
using R3;
using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public abstract class MonoViewAnimations : MonoBehaviour, IViewAnimations
    {
        [SerializeField] private bool _isEnabled;


        private bool _isInitialized;


        public bool IsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }


        protected readonly Subject<Unit> _showEnded = new();
        public Observable<Unit> ShowEnded => _showEnded;

        protected readonly Subject<Unit> _hideEnded = new();
        public Observable<Unit> HideEnded => _hideEnded;



        void IViewAnimations.Init(UIView view)
        {
            if (_isInitialized)
                throw new Exception($"The {GetType().Name} is already initialized.");

            InitCore(view);
            _isInitialized = true;
        }

        protected abstract void InitCore(UIView view);



        void IViewAnimations.PlayShow()
        {
            ThrowIfNonInitialized();
            PlayShowCore();
        }
        protected abstract void PlayShowCore();



        void IViewAnimations.PlayHide()
        {
            ThrowIfNonInitialized();
            PlayHideCore();
        }

        protected abstract void PlayHideCore();



        public void Dispose()
        {
            _showEnded.Dispose();
            _hideEnded.Dispose();
            DisposeCore();
        }

        protected virtual void DisposeCore() { }



        protected void ThrowIfNonInitialized()
        {
            if (!_isInitialized)
                throw new Exception($"The {GetType().Name} isn't initialized.");
        }
    }
}
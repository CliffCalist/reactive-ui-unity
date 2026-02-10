using System;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WhiteArrow.ReactiveUI
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private Button _btnHide;
        [SerializeField] private Selectable _defaultFocusOnShow;
        [SerializeField] private Selectable _defaultFocusOnHide;



        public bool IsInitializationInProgress { get; private set; }
        public bool IsInitialized { get; private set; }


        protected GameObject _object { get; private set; }

        private IUIAnimations _animations;
        private IDisposable _currentAnimationSubscription;
        private bool _useShowAnimationOnEnable;

        private IDisposable _subscriptions;



        public Selectable HideSelectable => _btnHide;


        public bool IsAnimationsEnabled
        {
            get
            {
                InitIfFalse();
                return _animations != null && _animations.IsEnabled;
            }
        }



        private readonly ReactiveProperty<bool> _isSelfShowed = new();
        public ReadOnlyReactiveProperty<bool> IsSelfShowed
        {
            get
            {
                InitIfFalse();
                return _isSelfShowed;
            }
        }

        private readonly ReactiveProperty<bool> _isInHierarchyShowed = new();
        public ReadOnlyReactiveProperty<bool> IsInHierarchyShowed
        {
            get
            {
                InitIfFalse();
                return _isInHierarchyShowed;
            }
        }


        private readonly ReactiveProperty<UIShowState> _showInHierarchyState = new();
        public ReadOnlyReactiveProperty<UIShowState> ShowInHierarchyState
        {
            get
            {
                InitIfFalse();
                return _showInHierarchyState;
            }
        }

        private readonly ReactiveProperty<UIHideState> _hideInHierarchyState = new();
        public ReadOnlyReactiveProperty<UIHideState> HideInHierarchyState
        {
            get
            {
                InitIfFalse();
                return _hideInHierarchyState;
            }
        }




        public void SetAnimations(IUIAnimations animations)
        {
            InitIfFalse();

            _currentAnimationSubscription?.Dispose();

            if (_animations != null)
                _animations.StopAllWithoutNotify();

            _animations = animations;
            _animations.Init(this);
        }



        #region Initialization
        private void Awake()
        {
            InitIfFalse();
        }

        protected void InitIfFalse()
        {
            if (!IsInitialized)
                Init();
        }

        private void Init()
        {
            if (IsInitialized || IsInitializationInProgress)
                return;

            IsInitializationInProgress = true;
            _object = gameObject;
            _isSelfShowed.Value = _object.activeSelf;
            _isInHierarchyShowed.Value = _object.activeInHierarchy;

            if (_object.activeInHierarchy)
            {
                _showInHierarchyState.Value = UIShowState.AnimationEnded;
                _hideInHierarchyState.Value = UIHideState.None;
            }
            else
            {
                _showInHierarchyState.Value = UIShowState.None;
                _hideInHierarchyState.Value = UIHideState.Hided;
            }

            if (TryGetComponent(out IUIAnimations animations))
                SetAnimations(animations);

            if (_btnHide != null)
            {
                _btnHide.OnClickAsObservable()
                    .Subscribe(_ => Hide())
                    .AddTo(this);
            }

            InitCore();
            IsInitializationInProgress = false;
            IsInitialized = true;
        }

        protected virtual void InitCore() { }
        #endregion



        #region Subscriptions
        protected void RecreateSubscriptionsIfVisible()
        {
            if (_showInHierarchyState.Value == UIShowState.Showed ||
                _showInHierarchyState.Value == UIShowState.AnimationEnded)
            {
                RecreateSubscriptions();
            }
        }

        protected void RecreateSubscriptions()
        {
            DisposeSubscriptions();
            _subscriptions = CreateSubscriptionsCore();
        }

        protected virtual IDisposable CreateSubscriptionsCore() => null;

        private void DisposeSubscriptions()
        {
            _subscriptions?.Dispose();
            _subscriptions = null;
            DisposeSubscriptionsCore();
        }

        protected virtual void DisposeSubscriptionsCore() { }
        #endregion



        #region Show/Hide
        public bool Show(bool skipAnimations = false)
        {
            InitIfFalse();
            if (_isSelfShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already opened.", _object);
                return false;
            }

            _showInHierarchyState.Value = UIShowState.Requested;

            _useShowAnimationOnEnable =
                !skipAnimations &&
                _isInHierarchyShowed.CurrentValue;

            SetActiveInternal(true);
            return true;
        }

        private void OnEnable()
        {
            InitIfFalse();

            if (_defaultFocusOnShow != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(_defaultFocusOnShow.gameObject);

            _hideInHierarchyState.Value = UIHideState.None;
            _isInHierarchyShowed.Value = true;
            _showInHierarchyState.Value = UIShowState.Showed;

            _currentAnimationSubscription?.Dispose();

            if (_animations != null)
                _animations.StopAllWithoutNotify();

            if (IsAnimationsEnabled && _useShowAnimationOnEnable)
                PlayShowAnimation();
            else _showInHierarchyState.Value = UIShowState.AnimationEnded;

            _useShowAnimationOnEnable = false;
            RecreateSubscriptions();
            OnShowedCore();
        }

        private void PlayShowAnimation()
        {
            _currentAnimationSubscription = _animations
                .ShowEnded
                .Take(1)
                .Subscribe(_ => _showInHierarchyState.Value = UIShowState.AnimationEnded);

            _animations.PlayShow();
        }

        protected virtual void OnShowedCore() { }



        public bool Hide(bool skipAnimations = false)
        {
            InitIfFalse();
            if (!_isSelfShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already closed.", _object);
                return false;
            }

            _hideInHierarchyState.Value = UIHideState.Requested;


            _currentAnimationSubscription?.Dispose();

            if (_animations != null)
                _animations.StopAllWithoutNotify();

            if (IsAnimationsEnabled && !skipAnimations && _isInHierarchyShowed.CurrentValue)
                PlayHideAnimation();
            else
                SetActiveInternal(false);

            return true;
        }

        private void PlayHideAnimation()
        {
            _currentAnimationSubscription = _animations.HideEnded
                .Take(1)
                .Subscribe(_ => SetActiveInternal(false));

            _animations.PlayHide();
        }

        private void OnDisable()
        {
            InitIfFalse();

            if (_defaultFocusOnHide != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(_defaultFocusOnHide.gameObject);

            _showInHierarchyState.Value = UIShowState.None;
            _isInHierarchyShowed.Value = false;
            _hideInHierarchyState.Value = UIHideState.Hided;

            DisposeSubscriptions();
            OnHidedCore();
        }

        protected virtual void OnHidedCore() { }



        private void SetActiveInternal(bool isActive)
        {
            _object.SetActive(isActive);
            _isSelfShowed.Value = _object.activeSelf;
            _isInHierarchyShowed.Value = _object.activeInHierarchy;
        }
        #endregion



        #region Destroy
        private void OnDestroy()
        {
            _currentAnimationSubscription?.Dispose();

            if (_animations != null)
                _animations.StopAllWithoutNotify();

            DisposeSubscriptions();
            OnDestroyCore();
        }

        protected virtual void OnDestroyCore() { }
        #endregion
    }
}
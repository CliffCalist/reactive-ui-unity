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
        private bool _isSelfHideRequested;

        protected IUIAnimations _animations { get; private set; }
        private bool _skipAnimationsOnce;

        private IDisposable _subscriptions;



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
            _animations = animations;
            _animations.Init(this);

            _animations.ShowEnded
                .Where(_ => IsAnimationsEnabled)
                .Subscribe(_ => _showInHierarchyState.Value = UIShowState.AnimationEnded)
                .AddTo(this);

            _animations.HideEnded
                .Where(_ => IsAnimationsEnabled)
                .Subscribe(_ => _object.SetActive(false))
                .AddTo(this);
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



        #region Show
        public bool Show(bool skipAnimations = false)
        {
            InitIfFalse();
            if (_isSelfShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already opened.", _object);
                return false;
            }

            _skipAnimationsOnce = skipAnimations;

            _isSelfShowed.Value = true;
            _showInHierarchyState.Value = UIShowState.Requested;
            _object.SetActive(true);

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

            if (IsAnimationsEnabled && !_skipAnimationsOnce)
                _animations.PlayShow();
            else
            {
                _skipAnimationsOnce = false;
                _showInHierarchyState.Value = UIShowState.AnimationEnded;
            }

            RecreateSubscriptions();
            OnShowedCore();
        }

        protected virtual void OnShowedCore() { }
        #endregion



        #region Hide
        public bool Hide(bool skipAnimations = false)
        {
            InitIfFalse();
            if (!_isSelfShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already closed.", _object);
                return false;
            }

            _skipAnimationsOnce = skipAnimations;
            _hideInHierarchyState.Value = UIHideState.Requested;

            if (_isInHierarchyShowed.CurrentValue && IsAnimationsEnabled && !_skipAnimationsOnce)
            {
                _isSelfHideRequested = true;
                _animations.PlayHide();
            }
            else
            {
                _object.SetActive(false);
                _isSelfShowed.Value = false;
            }

            return true;
        }

        private void OnDisable()
        {
            InitIfFalse();
            _skipAnimationsOnce = false;

            if (_defaultFocusOnHide != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(_defaultFocusOnHide.gameObject);

            if (_isSelfHideRequested)
            {
                _isSelfShowed.Value = false;
                _isSelfHideRequested = false;
            }

            _showInHierarchyState.Value = UIShowState.None;
            _isInHierarchyShowed.Value = false;
            _hideInHierarchyState.Value = UIHideState.Hided;

            DisposeSubscriptions();
            OnHidedCore();
        }

        protected virtual void OnHidedCore() { }
        #endregion



        #region Destroy
        private void OnDestroy()
        {
            DisposeSubscriptions();
            OnDestroyCore();
        }

        protected virtual void OnDestroyCore() { }
        #endregion
    }
}
using System;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WhiteArrow.ReactiveUI.Core
{
    [DisallowMultipleComponent]
    public class UIView : MonoBehaviour
    {
        [SerializeField] private Button _btnHide;

        [FormerlySerializedAs("_defaultFocusOnShow")]
        public Selectable FocusOnShowed;

        [FormerlySerializedAs("_defaultFocusOnHide")]
        public Selectable FocusOnHidden;



        public bool IsInitInProgress { get; private set; }
        public bool IsInited { get; private set; }


        private Transform _transform;

        private Visibility _visibility;
        private VisibilityAnimations _animations;
        private readonly ReactiveProperty<VisibilityPhase> _phase = new();


        private IDisposable _bindings;



        public IVisibilityContext Visibility
        {
            get
            {
                InitIfNeeded();
                return _visibility;
            }
        }

        public bool IsVisibilityAnimationsUsed
        {
            get
            {
                InitIfNeeded();
                return _animations != null;
            }
        }

        public ReadOnlyReactiveProperty<VisibilityPhase> Phase
        {
            get
            {
                InitIfNeeded();
                return _phase;
            }
        }



        #region Initialization
        private void Awake()
        {
            InitIfNeeded();
        }

        protected void InitIfNeeded()
        {
            if (!IsInited)
                InitInternal();
        }

        private void InitInternal()
        {
            if (IsInited || IsInitInProgress)
                return;

            IsInitInProgress = true;

            _transform = transform;
            _visibility = new(gameObject);
            _phase.Value = _visibility.IsShowedInHierarchy.CurrentValue
                ? VisibilityPhase.Shown
                : VisibilityPhase.Hidden;

            if (TryGetComponent(out VisibilityAnimations animations))
                AttachVisibilityAnimations(animations);

            if (_btnHide != null)
            {
                _btnHide.OnClickAsObservable()
                    .Where(_ => _visibility.IsSelfShowed.CurrentValue)
                    .Subscribe(_ => Hide())
                    .AddTo(this);
            }

            Init();

            IsInited = true;
            IsInitInProgress = false;
        }

        protected virtual void Init() { }
        #endregion



        #region Animations
        public void AttachVisibilityAnimations(VisibilityAnimations animations)
        {
            InitIfNeeded();

            if (_animations != null)
                _animations.DetachView();

            _animations = animations;
            _animations.AttachView(this);
        }
        #endregion



        #region Visibility
        public void Show(bool skipAnimation = false)
        {
            InitIfNeeded();

            if (_visibility.IsSelfShowed.CurrentValue && _phase.Value != VisibilityPhase.Hiding)
            {
                Debug.LogWarning($"{GetType().Name} is already showed.", _visibility.ControlledObject);
                return;
            }

            if (_transform.parent == null || _transform.parent.gameObject.activeInHierarchy)
            {
                _phase.Value = VisibilityPhase.Showing;
            }

            _visibility.SetActive(true);

            if (_visibility.IsShowedInHierarchy.CurrentValue)
            {
                if (IsVisibilityAnimationsUsed)
                {
                    _animations?.KillCurrentAnimation();

                    if (!skipAnimation)
                    {
                        _animations.PlayShowAnimationInternal(
                            () => _phase.Value = VisibilityPhase.Shown
                        );
                    }
                    else
                    {
                        _animations.SetEndStateOfShowAnimation();
                        _phase.Value = VisibilityPhase.Shown;
                    }
                }
                else _phase.Value = VisibilityPhase.Shown;
            }
        }

        private void OnEnable()
        {
            InitIfNeeded();
            _visibility.OnEnable();

            if (_phase.Value != VisibilityPhase.Showing)
                _phase.Value = VisibilityPhase.Shown;

            if (FocusOnShowed != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(FocusOnShowed.gameObject);

            Rebind();
            OnShown();
        }

        protected virtual void OnShown() { }



        public void Hide(bool skipAnimation = false)
        {
            InitIfNeeded();

            if (!_visibility.IsSelfShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already hidden.", _visibility.ControlledObject);
                return;
            }

            if (_visibility.IsShowedInHierarchy.CurrentValue)
            {
                _animations?.KillCurrentAnimation();
                _phase.Value = VisibilityPhase.Hiding;

                if (IsVisibilityAnimationsUsed)
                {
                    if (skipAnimation)
                    {
                        _animations.SetEndStateOfHideAnimation();
                        _visibility.SetActive(false);
                    }
                    else
                    {
                        _animations.PlayHideAnimationInternal(
                            () => _visibility.SetActive(false)
                        );
                    }
                }
                else _visibility.SetActive(false);
            }
            else _visibility.SetActive(false);
        }

        private void OnDisable()
        {
            InitIfNeeded();

            _visibility.OnDisable();
            _phase.Value = VisibilityPhase.Hidden;

            _animations?.KillCurrentAnimation();
            ClearBindings();

            if (FocusOnHidden != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(FocusOnHidden.gameObject);

            OnHidden();
        }

        protected virtual void OnHidden() { }
        #endregion



        #region Binding
        protected void RebindIfVisible()
        {
            InitIfNeeded();

            if (_visibility.IsShowedInHierarchy.CurrentValue)
                Rebind();
        }

        private void Rebind()
        {
            ClearBindings();

            var builder = new CompositeDisposable();
            CreateBindings(builder);

            if (builder.Count > 0)
                _bindings = builder;
        }

        protected virtual void CreateBindings(CompositeDisposable bindings) { }

        protected void ClearBindings()
        {
            _bindings?.Dispose();
            _bindings = null;
            OnBindingsCleared();
        }

        protected virtual void OnBindingsCleared() { }
        #endregion



        #region Destroy
        private void OnDestroy()
        {
            if (_animations != null)
            {
                _animations.KillCurrentAnimation();
                _animations.DetachView();
            }

            ClearBindings();
            OnDestroying();
        }

        protected virtual void OnDestroying() { }
        #endregion
    }
}
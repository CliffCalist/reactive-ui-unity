using R3;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteArrow.ReactiveUI
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private Button _btnHide;



        protected GameObject _object;
        protected IViewAnimations _animations { get; private set; }


        public bool IsInitialized { get; private set; }

        private bool _skipAnimationsOnce;

        public bool IsAnimationsEnabled => _animations != null && _animations.IsEnabled;


        private bool _isSelfHideRequested;
        private readonly ReactiveProperty<bool> _isSelfShowed = new();
        public ReadOnlyReactiveProperty<bool> IsSelfShowed => _isSelfShowed;

        private readonly ReactiveProperty<bool> _isInHierarchyShowed = new();
        public ReadOnlyReactiveProperty<bool> IsInHierarchyShowed => _isSelfShowed;


        private readonly ReactiveProperty<UIViewShowState> _showInHierarchyState = new();
        public ReadOnlyReactiveProperty<UIViewShowState> ShowInHierarchyState => _showInHierarchyState;

        private readonly ReactiveProperty<UIViewHideState> _hideInHierarchyState = new();
        public ReadOnlyReactiveProperty<UIViewHideState> HideInHierarchyState => _hideInHierarchyState;




        public void SetAnimations(IViewAnimations animations)
        {
            _animations = animations;
            _animations.Init(this);

            _animations.ShowEnded
                .Where(_ => IsAnimationsEnabled)
                .Subscribe(_ => _showInHierarchyState.Value = UIViewShowState.AnimationEnded)
                .AddTo(this);

            _animations.HideEnded
                .Where(_ => IsAnimationsEnabled)
                .Subscribe(_ => _object.SetActive(false))
                .AddTo(this);
        }



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
            if (IsInitialized)
                return;

            _object = gameObject;
            _isSelfShowed.Value = _object.activeSelf;
            _isInHierarchyShowed.Value = _object.activeInHierarchy;

            if (TryGetComponent(out IViewAnimations animations))
                SetAnimations(animations);

            if (_btnHide != null)
            {
                _btnHide.OnClickAsObservable()
                    .Subscribe(_ => Hide())
                    .AddTo(this);
            }


            InitCore();
            IsInitialized = true;
        }

        protected virtual void InitCore() { }



        protected void RebindIfShowedInHierarchy()
        {
            if (_showInHierarchyState.Value == UIViewShowState.Showed ||
                _showInHierarchyState.Value == UIViewShowState.AnimationEnded)
            {
                Rebind();
            }
        }

        protected void Rebind()
        {
            DisposeBinding();
            BindFromCache();
        }

        protected virtual void DisposeBinding() { }
        protected virtual void BindFromCache() { }



        public bool Show(bool skipAnimations = false)
        {
            InitIfFalse();
            if (_isSelfShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already opened.", _object);
                return false;
            }

            _skipAnimationsOnce = skipAnimations;
            _object.SetActive(true);
            _isSelfShowed.Value = true;
            _showInHierarchyState.Value = UIViewShowState.Requested;
            return true;
        }

        private void OnEnable()
        {
            InitIfFalse();
            Rebind();

            _hideInHierarchyState.Value = UIViewHideState.None;
            _isInHierarchyShowed.Value = true;
            _showInHierarchyState.Value = UIViewShowState.Showed;

            if (IsAnimationsEnabled && !_skipAnimationsOnce)
                _animations.PlayShow();
            else
            {
                _skipAnimationsOnce = false;
                _showInHierarchyState.Value = UIViewShowState.AnimationEnded;
            }

            OnShowed();
        }

        protected virtual void OnShowed() { }



        public bool Hide(bool skipAnimations = false)
        {
            InitIfFalse();
            if (!_isSelfShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already closed.", _object);
                return false;
            }

            _skipAnimationsOnce = skipAnimations;
            _hideInHierarchyState.Value = UIViewHideState.Requested;

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

            if (_isSelfHideRequested)
            {
                _isSelfShowed.Value = false;
                _isSelfHideRequested = false;
            }

            _showInHierarchyState.Value = UIViewShowState.None;
            _isInHierarchyShowed.Value = false;
            _hideInHierarchyState.Value = UIViewHideState.Hided;

            DisposeBinding();
            OnHided();
        }

        protected virtual void OnHided() { }



        private void OnDestroy()
        {
            DisposeBinding();
            OnDestroyCore();
        }

        protected virtual void OnDestroyCore() { }
    }
}
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteArrow.MVVM.UI
{
    public abstract class UIView : MonoBehaviour
    {
        [SerializeField] private Button _btnHide;



        private GameObject _object;
        private IViewAnimations _animations;


        public bool IsInitialized { get; private set; }
        public bool IsAnimationsEnabled => _animations != null && _animations.IsEnabled;

        private readonly ReactiveProperty<bool> _isShowed = new();
        public ReadOnlyReactiveProperty<bool> IsShowed => _isShowed;


        private readonly ReactiveProperty<UIViewShowState> _showState = new();
        public ReadOnlyReactiveProperty<UIViewShowState> ShowState => _showState;

        private readonly ReactiveProperty<UIViewHideState> _hideState = new();
        public ReadOnlyReactiveProperty<UIViewHideState> HideState => _hideState;




        public void SetAnimations(IViewAnimations animations)
        {
            _animations = animations;
            _animations.Init(this);
        }


        private void Awake()
        {
            InitIfFalse();
        }

        protected void InitIfFalse()
        {
            if (!IsInitialized)
                InitInternal();
        }

        private void InitInternal()
        {
            if (IsInitialized)
                return;

            _object = gameObject;


            if (IsAnimationsEnabled)
            {
                _animations.OnShowEnded.Subscribe(_ => _showState.Value = UIViewShowState.AnimationEnded).AddTo(this);
                _animations.OnHideEnded.Subscribe(_ => _object.SetActive(false)).AddTo(this);
            }


            if (_btnHide != null)
            {
                _btnHide.OnClickAsObservable()
                    .Subscribe(_ => Hide())
                    .AddTo(this);
            }


            Init();
            IsInitialized = true;
        }

        protected virtual void Init() { }



        protected void Rebind()
        {
            DisposeBinding();
            BindFromCache();
        }

        protected abstract void DisposeBinding();
        protected abstract void BindFromCache();



        public bool Show()
        {
            InitIfFalse();
            if (_isShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already opened.", _object);
                return false;
            }

            _showState.Value = UIViewShowState.Requested;
            _object.SetActive(true);
            return true;
        }

        private void OnEnable()
        {
            InitIfFalse();
            Rebind();

            // Because, hide states are ended, and show states are started
            _hideState.Value = UIViewHideState.None;

            _isShowed.Value = true;
            _showState.Value = UIViewShowState.Showed;

            if (IsAnimationsEnabled)
                _animations.PlayShow();
            else _showState.Value = UIViewShowState.AnimationEnded; // Because, animations is disabled

            OnShowed();
        }

        protected virtual void OnShowed() { }



        public bool Hide()
        {
            InitIfFalse();
            if (!_isShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already closed.", _object);
                return false;
            }

            _hideState.Value = UIViewHideState.Requested;
            if (IsAnimationsEnabled)
                _animations.PlayHide();
            else _object.SetActive(false);

            return true;
        }

        private void OnDisable()
        {
            InitIfFalse();

            // Because, show states are ended, and hide states are started
            _showState.Value = UIViewShowState.None;

            _isShowed.Value = false;
            _hideState.Value = UIViewHideState.Hided;
            DisposeBinding();
            OnHided();
        }

        protected virtual void OnHided() { }



        private void OnDestroy()
        {
            DisposeBinding();
        }
    }
}
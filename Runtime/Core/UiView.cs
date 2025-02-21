using R3;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteArrow.MVVM.UI
{
    public abstract class UiView : MonoBehaviour
    {
        [SerializeField] Animations _animations;
        [SerializeField] private Button _btnHide;


        public bool IsIniialized { get; private set; }


        private GameObject _object;

        private readonly ReactiveProperty<bool> _isShowed = new();
        public ReadOnlyReactiveProperty<bool> IsShowed => _isShowed;


        private readonly ReactiveProperty<UiViewShowState> _showState = new();
        public ReadOnlyReactiveProperty<UiViewShowState> ShowState => _showState;

        private readonly ReactiveProperty<UiViewHideState> _hideState = new();
        public ReadOnlyReactiveProperty<UiViewHideState> HideState => _hideState;



        private void Awake()
        {
            InitIfFalse();
        }

        protected void InitIfFalse()
        {
            if (!IsIniialized)
                Init();
        }

        private void Init()
        {
            if (IsIniialized)
                return;

            _object = gameObject;


            _animations.Initialize(this);
            if (_animations.IsEnabled)
            {
                _animations.OnShowEnded.Subscribe(_ => _showState.Value = UiViewShowState.AnimationEnded).AddTo(this);
                _animations.OnHideEnded.Subscribe(_ => _object.SetActive(false)).AddTo(this);
            }


            if (_btnHide != null)
            {
                _btnHide.OnClickAsObservable()
                    .Subscribe(_ => Hide())
                    .AddTo(this);
            }


            OnInit();
            IsIniialized = true;
        }

        protected virtual void OnInit() { }



        protected void Rebind()
        {
            DisposeBinding();
            OnRebind();
        }

        protected abstract void DisposeBinding();
        protected abstract void OnRebind();






        public bool Show()
        {
            InitIfFalse();
            if (_isShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already opened.", _object);
                return false;
            }

            _showState.Value = UiViewShowState.Requested;
            _object.SetActive(true);
            return true;
        }

        private void OnEnable()
        {
            InitIfFalse();
            Rebind();

            // Because, hide states are ended, and show states are started
            _hideState.Value = UiViewHideState.None;

            _isShowed.Value = true;
            _showState.Value = UiViewShowState.Showed;

            if (_animations.IsEnabled)
                _animations.PlayShow();
            else _showState.Value = UiViewShowState.AnimationEnded; // Because, animations is disabled

            OnEnabled();
        }

        protected virtual void OnEnabled() { }



        public bool Hide()
        {
            InitIfFalse();
            if (!_isShowed.CurrentValue)
            {
                Debug.LogWarning($"{GetType().Name} is already closed.", _object);
                return false;
            }

            _hideState.Value = UiViewHideState.Requested;
            if (_animations.IsEnabled)
                _animations.PlayHide();
            else _object.SetActive(false);

            return true;
        }

        private void OnDisable()
        {
            InitIfFalse();

            // Because, show states are ended, and hide states are started
            _showState.Value = UiViewShowState.None;

            _isShowed.Value = false;
            _hideState.Value = UiViewHideState.Hided;
            DisposeBinding();
            OnDisabled();
        }

        protected virtual void OnDisabled() { }
    }
}
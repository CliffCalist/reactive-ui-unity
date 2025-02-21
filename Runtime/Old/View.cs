// using System.Collections.Generic;
// using System.Linq;
// using R3;
// using UnityEngine;

// namespace WhiteArrow.UI.MVVM
// {
//     public abstract class View<TData> : MonoBehaviour, IView
//     {
//         [SerializeField] private List<InterfaceReference<IView, MonoBehaviour>> _childeViews;

//         [Space]
//         [SerializeField] private string _openAnimationName = "open";
//         [SerializeField] private string _closeAnimationName = "close";


//         private ReactiveProperty<bool> _isInitialised = new();
//         public ReadOnlyReactiveProperty<bool> IsInitialized => _isInitialised;


//         private string _guid;
//         public string GUID
//         {
//             get
//             {
//                 if (string.IsNullOrEmpty(_guid))
//                     _guid = System.Guid.NewGuid().ToString();
//                 return _guid;
//             }
//         }

//         public ViewFrame Frame { get; private set; }


//         protected GameObject _object { get; private set; }


//         protected Animator _animator { get; private set; }
//         private ViewAnimationsEndingListener _animationsEndingListener;
//         protected bool _isOpenCloseAnimationUsed => _animator != null;
//         protected bool _canListenAnimationEnd => _animationsEndingListener != null;



//         protected TData _sharedData { get; private set; }


//         private readonly ReactiveProperty<bool> _isOpened = new();
//         public ReadOnlyReactiveProperty<bool> IsOpened => _isOpened;

//         private readonly Subject<ViewOpenState> _onViewOpen = new();
//         public Observable<ViewOpenState> OnViewOpen => _onViewOpen;

//         private readonly Subject<ViewCloseState> _onViewClose = new();
//         public Observable<ViewCloseState> OnViewClose => _onViewClose;



//         #region Initialization
//         private void Awake()
//         {
//             InitCheck();
//         }

//         public void Init()
//         {
//             if (_isInitialised.Value)
//                 return;

//             _object = gameObject;
//             _isOpened.Value = _object.activeInHierarchy;

//             // Animations support
//             if (TryGetComponent(out Animator animator))
//             {
//                 _animator = animator;
//                 if (TryGetComponent(out _animationsEndingListener))
//                 {
//                     _animationsEndingListener.OnOpenEnded
//                         .Subscribe(_ => _onViewOpen.OnNext(ViewOpenState.AnimationEnded))
//                         .AddTo(this);
//                     _animationsEndingListener.OnCloseEnded.Subscribe(_ => _object.SetActive(false))
//                         .AddTo(this);
//                 }
//             }

//             InitFrame();
//             BindToViewModel();
//             OnInit();
//             _isInitialised.Value = true;
//         }

//         private void InitFrame()
//         {
//             var childes = _childeViews
//                 .Where(r => r.Value != null)
//                 .Select(r => r.Value)
//                 .ToList();

//             Frame = new(childes);

//             Frame.OnAnyChildWillOpen
//                 .Where(_ => !_isOpened.Value)
//                 .Subscribe(_ => Open(_sharedData))
//                 .AddTo(this);
//         }

//         protected virtual void BindToViewModel() { }
//         protected virtual void OnInit() { }


//         private void InitCheck()
//         {
//             if (!_isInitialised.Value)
//                 Init();
//         }
//         #endregion



//         #region Open & Close
//         IView IView.Open(object data)
//         {
//             if (data == null)
//                 return Open(default);
//             else if (data is TData castedData)
//                 return Open(castedData);
//             else throw new System.InvalidCastException($"The passed data is not of type {nameof(TData)} and cannot be used in {GetType().FullName} view.");
//         }

//         public View<TData> Open(TData data)
//         {
//             InitCheck();
//             if (_object.activeInHierarchy)
//             {
//                 Debug.LogWarning($"{nameof(View)} is already opened.");
//                 return this;
//             }

//             _onViewOpen.OnNext(ViewOpenState.WillOpen);

//             _sharedData = data;
//             _object.SetActive(true);

//             if (_isOpenCloseAnimationUsed)
//                 _animator.Play(_openAnimationName);

//             return this;
//         }

//         public IView Close(bool useAnimation = true)
//         {
//             InitCheck();
//             if (!_object.activeInHierarchy)
//             {
//                 Debug.LogWarning($"{nameof(View)} is already closed.");
//                 return this;
//             }

//             _onViewClose.OnNext(ViewCloseState.WillClose);

//             if (useAnimation && _isOpenCloseAnimationUsed)
//             {
//                 if (_canListenAnimationEnd)
//                     _animator.Play(_closeAnimationName);
//                 else
//                 {
//                     Debug.LogWarning($"The {nameof(View)} tried to play the closing animation but failed. The {nameof(View)} object must have a configured {nameof(ViewAnimationsEndingListener)} component.");
//                     _object.SetActive(false);
//                 }
//             }
//             else _object.SetActive(false);

//             return this;
//         }



//         private void OnEnable()
//         {
//             Prepare();
//             OnOpenedHandler();
//             _isOpened.Value = true;
//             _onViewOpen.OnNext(ViewOpenState.Activated);
//         }

//         protected virtual void Prepare() { }
//         protected virtual void OnOpenedHandler() { }



//         private void OnDisable()
//         {
//             OnClosedHandler();
//             _isOpened.Value = false;
//             _onViewClose.OnNext(ViewCloseState.Deactivated);
//         }

//         protected virtual void OnClosedHandler() { }
//         #endregion



//         #region Common
//         private void OnDestroy()
//         {
//             _onViewOpen.OnCompleted();
//             _onViewClose.OnCompleted();
//             Frame.Dispose();
//         }
//         #endregion
//     }



//     public class View : View<NullData>
//     { }
// }
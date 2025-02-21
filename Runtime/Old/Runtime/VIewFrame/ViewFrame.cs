// using System;
// using System.Collections.Generic;
// using ObservableCollections;
// using R3;
// using UnityEngine;

// namespace WhiteArrow.UI.MVVM
// {
//     public class ViewFrame : IViewFrame, IDisposable
//     {
//         #region Data
//         private readonly Dictionary<string, IView> _childes = new();
//         private readonly Stack<string> _history = new();

//         private readonly CompositeDisposable _disposables = new();
//         private readonly Subject<Unit> _onAnyChildWillOpen = new();
//         private readonly Subject<Unit> _onAllChildesClosed = new();
//         private readonly ObservableList<IView> _openedChildes = new();


//         public Observable<Unit> OnAnyChildWillOpen => _onAnyChildWillOpen;
//         public Observable<Unit> OnAllChildesClosed => _onAllChildesClosed;
//         public IObservableCollection<IView> OpenedChildes => _openedChildes;
//         #endregion



//         #region Initialization
//         public ViewFrame(IEnumerable<IView> childeViews = null)
//         {
//             if (childeViews != null)
//             {
//                 foreach (var childe in childeViews)
//                     Register(childe);
//             }
//         }


//         public void Register(IView childe)
//         {
//             if (childe == null)
//                 throw new ArgumentNullException(nameof(childe));

//             var guid = childe.GUID;
//             if (!_childes.ContainsKey(guid))
//             {
//                 _childes.Add(guid, childe);
//                 if (childe.IsOpened.CurrentValue)
//                     childe.Close(false);

//                 childe.OnViewOpen
//                     .Subscribe(state => OnChildeOpen(guid, state))
//                     .AddTo(_disposables);

//                 childe.OnViewClose
//                     .Subscribe(state => OnChildeClose(guid, state))
//                     .AddTo(_disposables);
//             }
//             else
//             {
//                 if (childe is MonoBehaviour childeMono)
//                     Debug.LogError($"View GUID:{guid} is already registered! This happen when trying register {childeMono.name}");
//             }
//         }
//         #endregion



//         #region Event Handlers
//         private void OnChildeOpen(string guid, ViewOpenState state)
//         {
//             switch (state)
//             {
//                 case ViewOpenState.WillOpen:
//                     _onAnyChildWillOpen.OnNext(Unit.Default);
//                     break;

//                 case ViewOpenState.Activated:
//                     OnChildeActivate(guid);
//                     break;
//             }
//         }

//         private void OnChildeActivate(string guid)
//         {
//             if (_history.Count == 0 || _history.Peek() != guid)
//                 _history.Push(guid);

//             if (_childes.TryGetValue(guid, out var childe) && !_openedChildes.Contains(childe))
//                 _openedChildes.Add(childe);
//         }



//         private void OnChildeClose(string guid, ViewCloseState state)
//         {
//             if (state == ViewCloseState.Deactivated)
//             {
//                 OnChildeDeactivated(guid);
//                 CheckIfAllChildesClosed();
//             }
//         }

//         private void OnChildeDeactivated(string guid)
//         {
//             if (_history.Count > 0 && _history.Peek() == guid)
//                 _history.Pop();

//             if (_childes.TryGetValue(guid, out var childe) && _openedChildes.Contains(childe))
//                 _openedChildes.Remove(childe);
//         }

//         private void CheckIfAllChildesClosed()
//         {
//             foreach (var childe in _childes.Values)
//             {
//                 if (childe.IsOpened.CurrentValue)
//                     return;
//             }
//             _onAllChildesClosed.OnNext(Unit.Default);
//         }
//         #endregion



//         #region Controll childes
//         public T Open<T>(string guid, object data = null)
//             where T : IView
//         {
//             return (T)Open(guid, data);
//         }

//         public IView Open(string guid, object data = null)
//         {
//             if (_childes.TryGetValue(guid, out var childe))
//                 childe.Open(data);
//             else Debug.LogError($"View GUID:{guid} is not registered!");
//             return childe;
//         }



//         public T Close<T>(string guid, bool useAnimation = true)
//              where T : IView
//         {
//             return (T)Close(guid, useAnimation);
//         }

//         public IView Close(string guid, bool useAnimation = true)
//         {
//             if (_childes.TryGetValue(guid, out var childe))
//                 childe.Close(useAnimation);
//             else Debug.LogError($"View GUID:{guid} is not registered!");
//             return childe;
//         }



//         public IView Back(object data = null)
//         {
//             IView openedView = null;
//             if (_history.Count > 1)
//             {
//                 var currentViewModelType = _history.Pop();
//                 Close(currentViewModelType);

//                 var previousViewModelType = _history.Peek();
//                 openedView = Open(previousViewModelType, data);
//             }
//             else Debug.Log("No more View in history to go back to.");
//             return openedView;
//         }
//         #endregion



//         #region Common
//         /// <summary>
//         /// Dispose of all subscriptions when no longer needed
//         /// </summary>
//         public void Dispose()
//         {
//             _disposables.Dispose();
//         }
//         #endregion
//     }
// }

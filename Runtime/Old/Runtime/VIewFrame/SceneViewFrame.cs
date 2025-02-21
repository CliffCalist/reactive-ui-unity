// using R3;
// using UnityEngine;

// namespace WhiteArrow.UI.MVVM
// {
//     public class SceneViewFrame : MonoBehaviour, IViewFrame
//     {
//         private readonly ViewFrame _frame = new();
//         private string _homeViewGuid;


//         public Observable<Unit> OnAnyChildWillOpen => _frame.OnAnyChildWillOpen;


//         private static SceneViewFrame s_instance;

//         public static SceneViewFrame Ref
//         {
//             get
//             {
//                 if (s_instance == null)
//                 {
//                     var instance = new GameObject("SCENE VIEW FRAME (WA-MVVM)");
//                     s_instance = instance.AddComponent<SceneViewFrame>();
//                 }
//                 return s_instance;
//             }
//         }



//         public void Register(IView view)
//         {
//             _frame.Register(view);
//         }

//         public void Register(IView view, bool isHome)
//         {
//             _frame.Register(view);

//             if (isHome)
//             {
//                 _homeViewGuid = view.GUID;
//                 view.Open(null);
//             }
//         }



//         public T Open<T>(string guid, object data = null)
//             where T : IView
//         {
//             return _frame.Open<T>(guid);
//         }

//         public IView Open(string guid, object data = null)
//         {
//             return _frame.Open(guid, data);
//         }



//         public T Close<T>(string guid, bool useAnimation = true)
//              where T : IView
//         {
//             return _frame.Close<T>(guid, useAnimation);
//         }

//         public IView Close(string guid, bool useAnimation = true)
//         {
//             return _frame.Close(guid, useAnimation);
//         }



//         public IView Back(object data = null)
//         {
//             return _frame.Back(data);
//         }

//         public IView GoToHome(object data = null)
//         {
//             IView homeView = null;
//             if (_homeViewGuid != null)
//                 homeView = _frame.Open(_homeViewGuid, data);
//             else Debug.LogError("Home View is not registered!");
//             return homeView;
//         }


//         private void OnDestroy()
//         {
//             _frame.Dispose();
//         }
//     }
// }
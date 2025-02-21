// using System;
// using R3;

// namespace WhiteArrow.UI.MVVM
// {
//     public interface IViewFrame
//     {
//         public Observable<Unit> OnAnyChildWillOpen { get; }


//         public void Register(IView childe);



//         public T Open<T>(string guid, object data = null)
//             where T : IView;

//         public IView Open(string guid, object data = null);



//         public T Close<T>(string guid, bool useAnimation = true)
//              where T : IView;

//         public IView Close(string guid, bool useAnimation = true);


//         public IView Back(object data = null);
//     }
// }

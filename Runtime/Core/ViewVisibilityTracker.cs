using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace WhiteArrow.MVVM.UI
{
    [Serializable]
    public class ViewVisibilityTracker : IDisposable
    {
        [SerializeField] private List<UIView> _views = new();

        private IDisposable _disposable;


        public ReadOnlyReactiveProperty<bool> IsAnyHided { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsAnyShowed { get; private set; }

        public ReadOnlyReactiveProperty<bool> IsAllHided { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsAllShowed { get; private set; }



        public void Init()
        {
            var disposableBuilder = new DisposableBuilder();


            IsAnyShowed = Observable.CombineLatest(_views.Select(v => v.IsShowed))
                .Select(v => v.Any(v => v))
                .ToReadOnlyReactiveProperty()
                .AddTo(ref disposableBuilder);

            IsAnyHided = Observable.CombineLatest(_views.Select(v => v.IsShowed))
                .Select(v => v.Any(v => !v))
                .ToReadOnlyReactiveProperty()
                .AddTo(ref disposableBuilder);

            IsAllShowed = Observable.CombineLatest(_views.Select(v => v.IsShowed))
                .Select(v => v.All(v => v))
                .ToReadOnlyReactiveProperty()
                .AddTo(ref disposableBuilder);

            IsAllHided = Observable.CombineLatest(_views.Select(v => v.IsShowed))
                .Select(v => v.All(v => !v))
                .ToReadOnlyReactiveProperty()
                .AddTo(ref disposableBuilder);


            _disposable = disposableBuilder.Build();
        }

        public void Dispose() => _disposable?.Dispose();
    }
}
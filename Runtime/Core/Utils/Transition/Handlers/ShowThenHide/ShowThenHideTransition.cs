using System;
using R3;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Core
{
    internal sealed class ShowThenHideTransition : ITransitionHandler
    {
        private readonly UIView _from;
        private readonly UIView _to;
        private readonly TransitionConfig _config;


        private IDisposable _layerPolicy;

        private readonly CompositeDisposable _disposables = new();
        private readonly Subject<Unit> _completed = new();
        private bool _isCompleted;



        public Observable<Unit> Completed => _completed;
        public bool IsCompleted => _isCompleted;



        public ShowThenHideTransition(UIView from, UIView to, TransitionConfig config)
        {
            _from = from;
            _to = to;
            _config = config ?? TransitionConfig.Default;
        }



        public void Execute()
        {
            if (_to == null)
            {
                if (_from != null)
                {
                    _from.Phase
                        .Where(p => p == VisibilityPhase.Hidden)
                        .Take(1)
                        .Subscribe(_ => TryComplete())
                        .AddTo(_disposables);

                    _from.Hide(_config.SkipHideAnimation);
                }
                else TryComplete();
                return;
            }

            if (_from == null)
            {
                _to.Phase
                    .Where(p => p == VisibilityPhase.Shown)
                    .Take(1)
                    .Subscribe(_ => TryComplete())
                    .AddTo(_disposables);

                _to.Show(_config.SkipShowAnimation);
                return;
            }

            _from.Phase
                .Where(p => p == VisibilityPhase.Hidden)
                .Take(1)
                .Subscribe(_ =>
                {
                    _layerPolicy?.Dispose();
                    TryComplete();
                })
                .AddTo(_disposables);

            _to.Phase
                .Where(p => p == VisibilityPhase.Shown)
                .Take(1)
                .Subscribe(_ => _from.Hide(_config.SkipHideAnimation))
                .AddTo(_disposables);

            _to.Show(_config.SkipShowAnimation);
            ApplyLayerPolicy();
        }

        private void TryComplete()
        {
            if (_isCompleted)
                return;

            _isCompleted = true;
            _completed.OnNext(Unit.Default);
            _completed.OnCompleted();
        }



        private void ApplyLayerPolicy()
        {
            if (_config.LayerPolicy == TransitionLayerPolicy.None)
                return;

            var fromCanvas = _from.GetComponent<Canvas>();
            var toCanvas = _to.GetComponent<Canvas>();

            if (fromCanvas == null || toCanvas == null)
            {
                Debug.LogWarning($"{_from.name} and {_to.name} must have Canvas component for layer policy to work.");
                return;
            }

            if (!_from.Visibility.IsSelfShowed.CurrentValue)
            {
                Debug.Log($"{_from.name} must be showed for layer policy to work.");
                return;
            }

            if (!_to.Visibility.IsSelfShowed.CurrentValue)
            {
                Debug.Log($"{_to.name} must be showed for layer policy to work.");
                return;
            }

            if (!fromCanvas.overrideSorting && _from.Visibility.IsSelfShowed.CurrentValue)
                fromCanvas.overrideSorting = true;

            if (!toCanvas.overrideSorting && _to.Visibility.IsSelfShowed.CurrentValue)
                toCanvas.overrideSorting = true;

            int fromOriginal = fromCanvas.sortingOrder;
            int toOriginal = toCanvas.sortingOrder;

            switch (_config.LayerPolicy)
            {
                case TransitionLayerPolicy.IncomingOnTop:
                    toCanvas.sortingOrder = fromOriginal + 1;
                    break;

                case TransitionLayerPolicy.OutgoingOnTop:
                    fromCanvas.sortingOrder = toOriginal + 1;
                    break;
            }

            _layerPolicy = Disposable.Create(() =>
            {
                fromCanvas.sortingOrder = fromOriginal;
                toCanvas.sortingOrder = toOriginal;
            });
        }



        public void Dispose()
        {
            if (_isCompleted)
                return;

            _disposables.Dispose();
            _layerPolicy?.Dispose();
            _isCompleted = true;
            _completed.OnCompleted();
        }
    }
}
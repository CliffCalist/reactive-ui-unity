using R3;

namespace WhiteArrow.ReactiveUI.Core
{
    internal sealed class HideThenShowTransition : ITransitionHandler
    {
        private readonly UIView _from;
        private readonly UIView _to;
        private readonly TransitionConfig _config;



        private readonly CompositeDisposable _disposables = new();
        private readonly Subject<Unit> _completed = new();
        private bool _isCompleted;



        public Observable<Unit> Completed => _completed;
        public bool IsCompleted => _isCompleted;



        public HideThenShowTransition(UIView from, UIView to, TransitionConfig config)
        {
            _from = from;
            _to = to;
            _config = config ?? TransitionConfig.Default;
        }

        public void Execute()
        {
            if (_from == null)
            {
                if (_to != null)
                {
                    _to.Phase
                        .Where(p => p == VisibilityPhase.Shown)
                        .Take(1)
                        .Subscribe(_ => TryComplete())
                        .AddTo(_disposables);

                    _to.Show(_config.SkipShowAnimation);
                }
                else TryComplete();
                return;
            }

            if (_to == null)
            {
                _from.Phase
                    .Where(p => p == VisibilityPhase.Hidden)
                    .Take(1)
                    .Subscribe(_ => TryComplete())
                    .AddTo(_disposables);

                _from.Hide(_config.SkipHideAnimation);
                return;
            }

            _to.Phase
                .Where(p => p == VisibilityPhase.Shown)
                .Take(1)
                .Subscribe(_ => TryComplete())
                .AddTo(_disposables);

            _from.Phase
                .Where(p => p == VisibilityPhase.Hidden)
                .Take(1)
                .Subscribe(_ => _to.Show(_config.SkipShowAnimation))
                .AddTo(_disposables);

            _from.Hide(_config.SkipHideAnimation);
        }

        private void TryComplete()
        {
            if (_isCompleted)
                return;

            _isCompleted = true;
            _completed.OnNext(Unit.Default);
            _completed.OnCompleted();
        }

        public void Dispose()
        {
            if (_isCompleted)
                return;

            _disposables.Dispose();
            _isCompleted = true;
            _completed.OnCompleted();
        }
    }
}
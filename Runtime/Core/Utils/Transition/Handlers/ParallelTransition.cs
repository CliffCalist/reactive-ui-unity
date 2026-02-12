using R3;

namespace WhiteArrow.ReactiveUI.Core
{
    internal sealed class ParallelTransition : ITransitionHandler
    {
        private readonly UIView _from;
        private readonly UIView _to;
        private readonly TransitionConfig _config;


        private readonly CompositeDisposable _disposables = new();

        private readonly Subject<Unit> _completed = new();
        private bool _isCompleted;



        public Observable<Unit> Completed => _completed;
        public bool IsCompleted => _isCompleted;



        public ParallelTransition(UIView from, UIView to, TransitionConfig config)
        {
            _from = from;
            _to = to;
            _config = config ?? TransitionConfig.Default;
        }

        public void Execute()
        {
            if (_to != null)
            {
                _to.Phase
                    .Where(phase => phase == VisibilityPhase.Shown)
                    .Take(1)
                    .Subscribe(_ => TryComplete())
                    .AddTo(_disposables);

                _to.Show(_config.SkipShowAnimation);
            }


            if (_from != null)
            {
                _from.Phase
                    .Where(phase => phase == VisibilityPhase.Hidden)
                    .Take(1)
                    .Subscribe(_ => TryComplete())
                    .AddTo(_disposables);

                _from?.Hide(_config.SkipHideAnimation);
            }
        }

        private void TryComplete()
        {
            if (_isCompleted)
                return;

            if (_from != null && _from.Phase.CurrentValue != VisibilityPhase.Hidden)
                return;

            if (_to != null && _to.Phase.CurrentValue != VisibilityPhase.Shown)
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
using System;

namespace WhiteArrow.ReactiveUI.Core
{
    internal static class TransitionCoordinator
    {
        public static ITransitionHandler Switch(UIView from, UIView to, TransitionConfig config)
        {
            if (from == null && to == null)
                throw new ArgumentException($"{nameof(from)} and {nameof(to)} can't be null at the same time.");

            config ??= TransitionConfig.Default;

            ITransitionHandler handler = config.Flow switch
            {
                TransitionFlow.Parallel => new ParallelTransition(from, to, config),
                TransitionFlow.ShowThenHide => new ShowThenHideTransition(from, to, config),
                TransitionFlow.HideThenShow => new HideThenShowTransition(from, to, config),
                _ => null
            };

            handler?.Execute();
            return handler;
        }
    }
}
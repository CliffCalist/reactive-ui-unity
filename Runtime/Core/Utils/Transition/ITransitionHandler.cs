using System;
using R3;

namespace WhiteArrow.ReactiveUI.Core
{
    public interface ITransitionHandler : IDisposable
    {
        Observable<Unit> Completed { get; }
        bool IsCompleted { get; }


        void Execute();
    }
}
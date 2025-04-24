using System;
using System.Collections.Generic;
using R3;

namespace WhiteArrow.ReactiveUI
{
    public class CombinedViewAnimations : ViewAnimations
    {
        private readonly List<ViewAnimations> _animations;
        private IDisposable _showDisposable;
        private IDisposable _hideDisposable;



        public CombinedViewAnimations(IEnumerable<ViewAnimations> animations)
        {
            _animations = new List<ViewAnimations>(animations);
        }



        protected override void InitCore(UIView view)
        {
            _animations.ForEach(a => a.Init(view));
        }


        protected override void PlayShowCore()
        {
            var remaining = _animations.Count;
            if (remaining == 0)
            {
                _showEnded.OnNext(Unit.Default);
                return;
            }

            _showDisposable?.Dispose();
            var builder = new DisposableBuilder();

            foreach (var anim in _animations)
            {
                anim.ShowEnded
                    .Subscribe(_ =>
                    {
                        remaining--;
                        if (remaining == 0)
                            _showEnded.OnNext(Unit.Default);
                    })
                    .AddTo(ref builder);

                anim.PlayShow();
            }

            _showDisposable = builder.Build();
        }

        protected override void PlayHideCore()
        {
            var remaining = _animations.Count;
            if (remaining == 0)
            {
                _hideEnded.OnNext(Unit.Default);
                return;
            }

            _hideDisposable?.Dispose();
            var builder = new DisposableBuilder();

            foreach (var anim in _animations)
            {
                anim.HideEnded
                    .Subscribe(_ =>
                    {
                        remaining--;
                        if (remaining == 0)
                            _hideEnded.OnNext(Unit.Default);
                    })
                    .AddTo(ref builder);

                anim.PlayHide();
            }

            _hideDisposable = builder.Build();
        }



        public override void Dispose()
        {
            base.Dispose();
            _showDisposable?.Dispose();
            _hideDisposable?.Dispose();
        }
    }
}
using System;

namespace WhiteArrow.ReactiveUI.Core
{
    [Serializable]
    public class TransitionConfig
    {
        public TransitionFlow Flow = TransitionFlow.HideThenShow;
        public TransitionLayerPolicy LayerPolicy = TransitionLayerPolicy.None;
        public bool SkipShowAnimation;
        public bool SkipHideAnimation;


        public static readonly TransitionConfig Default = new();
    }
}
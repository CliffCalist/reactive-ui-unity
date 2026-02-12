using R3;

namespace WhiteArrow.ReactiveUI.Core
{
    public interface IVisibilityContext
    {
        ReadOnlyReactiveProperty<bool> IsSelfShowed { get; }
        ReadOnlyReactiveProperty<bool> IsShowedInHierarchy { get; }
    }
}
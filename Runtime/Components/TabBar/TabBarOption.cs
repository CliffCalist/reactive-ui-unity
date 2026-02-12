using R3;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrow.ReactiveUI.Components
{
    public class TabBarOption : SelectorOption<UIView>
    {
        private readonly ReactiveProperty<bool> _isSelected = new(false);
        public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected;



        public override void OnSelectionChanged(bool isSelected)
        {
            _isSelected.Value = isSelected;
        }
    }
}
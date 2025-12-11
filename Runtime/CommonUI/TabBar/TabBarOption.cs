using R3;

namespace WhiteArrow.ReactiveUI
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
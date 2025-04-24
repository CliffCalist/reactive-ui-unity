using UnityEngine;

namespace WhiteArrow.MVVM.UI
{
    public class ShowViewButton : ViewButton
    {
        [SerializeField] private UIView _view;



        protected override void OnClicked()
        {
            if (!_view.IsSelfShowed.CurrentValue)
                _view.Show();
        }
    }
}

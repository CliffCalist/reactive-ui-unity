using UnityEngine;

namespace WhiteArrow.ReactiveUI
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

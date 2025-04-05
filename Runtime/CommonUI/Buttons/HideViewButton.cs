using UnityEngine;

namespace WhiteArrow.MVVM.UI
{
    public class HideViewButton : ViewButton
    {
        [SerializeField] private UIView _view;



        protected override void OnClicked()
        {
            if (_view.IsShowed.CurrentValue)
                _view.Hide();
        }
    }
}
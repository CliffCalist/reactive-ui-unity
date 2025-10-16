using UnityEngine;

namespace WhiteArrow.ReactiveUI.Auth
{
    public class SwitchViewButton : ViewButton
    {
        [SerializeField] private UIView _hideView;
        [SerializeField] private UIView _showView;



        protected override void OnClicked()
        {
            _hideView?.Hide();
            _showView?.Show();
        }
    }
}
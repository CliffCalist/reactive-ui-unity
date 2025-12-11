using UnityEngine;

namespace WhiteArrow.ReactiveUI.Auth
{
    public class SwitchUIButton : UIButton
    {
        [SerializeField] private UIView _uiForHide;
        [SerializeField] private UIView _uiForShow;



        protected override void OnClicked()
        {
            _uiForHide?.Hide();
            _uiForShow?.Show();
        }
    }
}
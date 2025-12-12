using UnityEngine;
using UnityEngine.Serialization;

namespace WhiteArrow.ReactiveUI.Auth
{
    public class SwitchUIButton : UIButton
    {
        [FormerlySerializedAs("_hideView")]
        [SerializeField] private UIView _uiViewForHide;

        [FormerlySerializedAs("_showView")]
        [SerializeField] private UIView _uiViewForShow;



        protected override void OnClicked()
        {
            _uiViewForHide?.Hide();
            _uiViewForShow?.Show();
        }
    }
}
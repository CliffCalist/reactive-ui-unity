using UnityEngine;
using UnityEngine.Serialization;

namespace WhiteArrow.ReactiveUI
{
    public class HideUIButton : UIButton
    {
        [FormerlySerializedAs("_view")]
        [SerializeField] private UIView _uiView;



        protected override void OnClicked()
        {
            if (_uiView.IsSelfShowed.CurrentValue)
                _uiView.Hide();
        }
    }
}
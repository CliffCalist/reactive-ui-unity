using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public class HideUIButton : UIButton
    {
        [SerializeField] private UIView _ui;



        protected override void OnClicked()
        {
            if (_ui.IsSelfShowed.CurrentValue)
                _ui.Hide();
        }
    }
}
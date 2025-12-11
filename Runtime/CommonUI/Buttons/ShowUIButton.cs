using UnityEngine;

namespace WhiteArrow.ReactiveUI
{
    public class ShowUIButton : UIButton
    {
        [SerializeField] private UIView _ui;



        protected override void OnClicked()
        {
            if (!_ui.IsSelfShowed.CurrentValue)
                _ui.Show();
        }
    }
}

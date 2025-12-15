using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WhiteArrow.ReactiveUI.Auth
{
    public class SwitchUIButton : UIButton
    {
        [FormerlySerializedAs("_hideView")]
        [SerializeField] private UIView _uiViewForHide;

        [FormerlySerializedAs("_showView")]
        [SerializeField] private UIView _uiViewForShow;

        [SerializeField] private Selectable _focusObject;



        protected override void OnClicked()
        {
            _uiViewForHide.Hide();
            _uiViewForShow.Show();

            if (_focusObject != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(_focusObject.gameObject);
        }
    }
}
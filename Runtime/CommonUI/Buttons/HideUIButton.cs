using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WhiteArrow.ReactiveUI
{
    public class HideUIButton : UIButton
    {
        [FormerlySerializedAs("_view")]
        [SerializeField] private UIView _uiView;

        [SerializeField] private Selectable _focusObject;



        protected override void OnClicked()
        {
            if (_uiView.IsSelfShowed.CurrentValue)
            {
                _uiView.Hide();

                if (_focusObject != null && EventSystem.current != null)
                    EventSystem.current.SetSelectedGameObject(_focusObject.gameObject);
            }
        }
    }
}
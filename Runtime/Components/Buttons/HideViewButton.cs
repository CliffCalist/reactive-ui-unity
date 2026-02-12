using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrow.ReactiveUI.Components
{
    public class HideViewButton : ViewButton
    {
        [FormerlySerializedAs("_uiView")]
        [SerializeField] private UIView _view;

        [SerializeField] private Selectable _focusObject;



        protected override void OnClicked()
        {
            _view.Hide();

            if (_focusObject != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(_focusObject.gameObject);
        }
    }
}
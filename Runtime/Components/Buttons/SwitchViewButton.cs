using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrow.ReactiveUI.Components
{
    public class SwitchViewButton : ViewButton
    {
        [FormerlySerializedAs("_hideView")]
        [FormerlySerializedAs("_uiViewForHide")]
        [SerializeField] private UIView _viewForHide;

        [FormerlySerializedAs("_showView")]
        [FormerlySerializedAs("_uiViewForShow")]
        [SerializeField] private UIView _viewForShow;

        [SerializeField] private TransitionConfig _transition;

        [Space]
        [SerializeField] private Selectable _focusObject;



        protected override void OnClicked()
        {
            ViewUtils.Switch(_viewForHide, _viewForShow, _transition);

            if (_focusObject != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(_focusObject.gameObject);
        }
    }
}
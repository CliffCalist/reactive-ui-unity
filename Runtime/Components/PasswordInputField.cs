using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrow.ReactiveUI.Components
{
    public class PasswordInputField : UIView
    {
        [Header("Components")]
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _btnSwitchType;

        [Header("States")]
        [SerializeField] private TMP_InputField.ContentType _hidedState = TMP_InputField.ContentType.Password;
        [SerializeField] private TMP_InputField.ContentType _showedState = TMP_InputField.ContentType.Standard;

        [Header("Icons")]
        [SerializeField] private Image _imgHideIcon;
        [SerializeField] private Sprite _hidedIcon;
        [SerializeField] private Sprite _showedIcon;



        protected override void CreateBindings(CompositeDisposable bindings)
        {
            SetHideState(true);

            _btnSwitchType.OnClickAsObservable()
                .Subscribe(_ => SwitchHideState())
                .AddTo(bindings);
        }



        private void SwitchHideState()
        {
            var isHided = _inputField.contentType == _hidedState;
            SetHideState(!isHided);
        }

        private void SetHideState(bool isHided)
        {
            if (isHided)
            {
                _inputField.contentType = _hidedState;
                _imgHideIcon.sprite = _hidedIcon;
            }
            else
            {
                _inputField.contentType = _showedState;
                _imgHideIcon.sprite = _showedIcon;
            }

            _inputField.ForceLabelUpdate();
        }
    }
}
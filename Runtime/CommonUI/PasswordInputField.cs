using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteArrow.ReactiveUI
{
    public class PasswordInputField : UIView
    {
        [Header("Components")]
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _btnSwitchType;

        [Header("States")]
        [SerializeField] private TMP_InputField.ContentType _hidedState = TMP_InputField.ContentType.Standard;
        [SerializeField] private TMP_InputField.ContentType _showedState = TMP_InputField.ContentType.Password;

        [Header("Icons")]
        [SerializeField] private Image _imgHideIcon;
        [SerializeField] private Sprite _hidedIcon;
        [SerializeField] private Sprite _showedIcon;



        private IDisposable _disposable;



        protected override void CreateSubscriptions()
        {
            SetHideState(true);

            _disposable = _btnSwitchType.OnClickAsObservable()
                .Subscribe(_ => SwitchHideState());
        }

        protected override void DisposeSubscriptions()
        {
            _disposable?.Dispose();
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
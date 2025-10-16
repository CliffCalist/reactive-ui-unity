using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Auth
{
    public abstract class ChangeUserPasswordViewBase : ChangeUserDataViewBase
    {
        [Header("Input")]
        [SerializeField] private int _minLength = 3;
        [SerializeField] private TMP_InputField _inputPassword;
        [SerializeField] private TMP_InputField _inputConfirmPassword;



        protected override bool IsInputValid()
        {
            return AuthInputValidator.IsValidPassword(_inputPassword.text, _inputConfirmPassword.text);
        }

        protected override sealed void PerformChange(Action<AuthOperationResult> callback)
        {
            ChangePassword(_inputPassword.text, callback);
        }

        protected abstract void ChangePassword(string newPassword, Action<AuthOperationResult> callback);
    }
}
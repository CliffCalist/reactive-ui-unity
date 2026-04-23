using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public abstract class ChangeUserPasswordViewBase : ChangeUserDataViewBase
    {
        [Header("Input")]
        [SerializeField] private TMP_InputField _inputPassword;
        [SerializeField] private TMP_InputField _inputConfirmPassword;



        protected override AuthValidationResult ValidateInput()
        {
            var passwordValidation = AuthInputValidator.ValidatePassword(_inputPassword.text);
            if (!passwordValidation.IsValid)
                return passwordValidation;

            return AuthInputValidator.ValidatePasswordConfirmation(_inputPassword.text, _inputConfirmPassword.text);
        }

        protected override sealed void PerformChange(Action<AuthOperationResult> callback)
        {
            ChangePassword(_inputPassword.text, callback);
        }

        protected abstract void ChangePassword(string newPassword, Action<AuthOperationResult> callback);
    }
}

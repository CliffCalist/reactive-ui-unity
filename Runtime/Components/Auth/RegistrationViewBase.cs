using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public abstract class RegistrationViewBase : AuthViewBase
    {
        [Header("Inputs")]
        [SerializeField] private TMP_InputField _inputEmail;
        [SerializeField] private TMP_InputField _inputPassword;
        [SerializeField] private TMP_InputField _inputConfirmPassword;

        [Space]
        [SerializeField] private bool _isNameUsed;
        [SerializeField] private TMP_InputField _inputName;



        protected override sealed AuthValidationResult ValidateInput()
        {
            var emailValidation = AuthInputValidator.ValidateEmail(_inputEmail.text);
            if (!emailValidation.IsValid)
                return emailValidation;

            var passwordValidation = AuthInputValidator.ValidatePassword(_inputPassword.text);
            if (!passwordValidation.IsValid)
                return passwordValidation;

            var confirmPasswordValidation = AuthInputValidator
                .ValidatePasswordConfirmation(_inputPassword.text, _inputConfirmPassword.text);

            if (!confirmPasswordValidation.IsValid)
                return confirmPasswordValidation;

            if (_isNameUsed)
                return AuthInputValidator.ValidateName(_inputName.text);

            return AuthValidationResult.Valid;
        }

        protected override sealed void PerformAction(Action<AuthOperationResult> callback)
        {
            var data = _isNameUsed ?
                new AuthCredentials(_inputEmail.text, _inputPassword.text, _inputName.text) :
                new AuthCredentials(_inputEmail.text, _inputPassword.text);

            Registration(data, callback);
        }

        protected abstract void Registration(AuthCredentials data, Action<AuthOperationResult> callback);
    }
}

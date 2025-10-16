using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Auth
{
    public abstract class RegistrationViewBase : AuthFormViewBase
    {
        [Header("Inputs")]
        [SerializeField] private TMP_InputField _inputEmail;
        [SerializeField] private TMP_InputField _inputPassword;
        [SerializeField] private TMP_InputField _inputConfirmPassword;



        protected override sealed bool IsInputValid()
        {
            return AuthInputValidator.IsValidEmail(_inputEmail.text) &&
                AuthInputValidator.IsValidPassword(_inputPassword.text, _inputConfirmPassword.text);
        }

        protected override sealed void PerformAction(Action<AuthOperationResult> callback)
        {
            var data = new AuthCredentials(_inputEmail.text, _inputPassword.text);
            Registration(data, callback);
        }

        protected abstract void Registration(AuthCredentials data, Action<AuthOperationResult> callback);
    }
}
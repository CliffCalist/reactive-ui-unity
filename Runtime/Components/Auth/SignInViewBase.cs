using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public abstract class SignInViewBase : AuthViewBase
    {
        [Header("Inputs")]
        [SerializeField] private TMP_InputField _inputEmail;
        [SerializeField] private TMP_InputField _inputPassword;


        protected override sealed AuthValidationResult ValidateInput()
        {
            var emailValidation = AuthInputValidator.ValidateEmail(_inputEmail.text);
            if (!emailValidation.IsValid)
                return emailValidation;

            return AuthInputValidator.ValidatePassword(_inputPassword.text);
        }

        protected override sealed void PerformAction(Action<AuthOperationResult> callback)
        {
            var data = new AuthCredentials(_inputEmail.text, _inputPassword.text);
            SignIn(data, callback);
        }

        protected abstract void SignIn(AuthCredentials data, Action<AuthOperationResult> callback);
    }
}

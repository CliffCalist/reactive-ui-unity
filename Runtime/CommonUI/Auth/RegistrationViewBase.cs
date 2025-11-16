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

        [Space]
        [SerializeField] private bool _isNameUsed;
        [SerializeField] private TMP_InputField _inputName;



        protected override sealed bool IsInputValid()
        {
            return AuthInputValidator.IsValidEmail(_inputEmail.text) &&
                AuthInputValidator.IsValidPassword(_inputPassword.text, _inputConfirmPassword.text) &&
                _isNameUsed ? AuthInputValidator.IsValidName(_inputName.text) : true;
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
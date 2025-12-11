using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Auth
{
    public abstract class ResetUserPasswordUIBase : AuthFormUIBase
    {
        [SerializeField] private TMP_InputField _inputEmail;



        protected override sealed bool IsInputValid()
        {
            return AuthInputValidator.IsValidEmail(_inputEmail.text);
        }

        protected override sealed void PerformAction(Action<AuthOperationResult> callback)
        {
            SendResetPasswordEmail(_inputEmail.text, callback);
        }

        protected abstract void SendResetPasswordEmail(string email, Action<AuthOperationResult> callback);
    }
}
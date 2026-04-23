using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public abstract class ChangeUserEmailViewBase : ChangeUserDataViewBase
    {
        [Header("Input")]
        [SerializeField] private TMP_InputField _inputEmail;



        protected override sealed AuthValidationResult ValidateInput()
        {
            return AuthInputValidator.ValidateEmail(_inputEmail.text);
        }

        protected override void PerformChange(Action<AuthOperationResult> callback)
        {
            ChangeEmail(_inputEmail.text, callback);
        }

        protected abstract void ChangeEmail(string email, Action<AuthOperationResult> callback);
    }
}

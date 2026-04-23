using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public abstract class ChangeUserNameViewBase : ChangeUserDataViewBase
    {
        [Header("Input")]
        [SerializeField] private TMP_InputField _inputName;



        protected override sealed AuthValidationResult ValidateInput()
        {
            return AuthInputValidator.ValidateName(_inputName.text);
        }

        protected override sealed void PerformChange(Action<AuthOperationResult> callback)
        {
            ChangeName(_inputName.text, callback);
        }

        protected abstract void ChangeName(string newName, Action<AuthOperationResult> callback);
    }
}

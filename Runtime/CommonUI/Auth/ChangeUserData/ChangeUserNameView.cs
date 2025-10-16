using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Auth
{
    public abstract class ChangeUserNameViewBase : ChangeUserDataViewBase
    {
        [Header("Input")]
        [SerializeField, Min(1)] private int _minLength = 3;
        [SerializeField] private TMP_InputField _inputName;



        protected override sealed bool IsInputValid()
        {
            return AuthInputValidator.IsValidName(_inputName.text);
        }

        protected override sealed void PerformChange(Action<AuthOperationResult> callback)
        {
            ChangeName(_inputName.text, callback);
        }

        protected abstract void ChangeName(string newName, Action<AuthOperationResult> callback);
    }
}
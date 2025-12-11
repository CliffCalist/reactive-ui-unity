using System;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Auth
{
    public abstract class ChangeUserDataUIBase : AuthFormUIBase
    {
        [Header("Credential confirm")]
        [SerializeField] private bool _useCredentialConfirm = true;
        [SerializeField] private CredentialConfirmUIBase _confirmCredentialUI;



        protected override sealed void PerformAction(Action<AuthOperationResult> callback)
        {
            if (_useCredentialConfirm)
            {
                _confirmCredentialUI.Bind(isSuccess =>
                {
                    if (isSuccess)
                        PerformChange(callback);
                    else
                        callback(AuthOperationResult.Fail("Credential confirm failed"));
                });
            }
            else
                PerformChange(callback);
        }

        protected abstract void PerformChange(Action<AuthOperationResult> callback);
    }
}
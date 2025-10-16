using System;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Auth
{
    public abstract class ChangeUserDataViewBase : AuthFormViewBase
    {
        [Header("Credential confirm")]
        [SerializeField] private bool _useCredentialConfirm = true;
        [SerializeField] private CredentialConfirmViewBase _confirmCredentialView;



        protected override sealed void PerformAction(Action<AuthOperationResult> callback)
        {
            if (_useCredentialConfirm)
            {
                _confirmCredentialView.Bind(isSuccess =>
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
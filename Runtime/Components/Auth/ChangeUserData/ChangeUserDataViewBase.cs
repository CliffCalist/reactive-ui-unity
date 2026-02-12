using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public abstract class ChangeUserDataViewBase : AuthViewBase
    {
        [Header("Credential confirm")]
        [SerializeField] private bool _useCredentialConfirm = true;

        [FormerlySerializedAs("_confirmCredentialUI")]
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

                _confirmCredentialView.Show();
            }
            else
                PerformChange(callback);
        }

        protected abstract void PerformChange(Action<AuthOperationResult> callback);
    }
}
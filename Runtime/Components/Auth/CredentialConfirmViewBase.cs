using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public abstract class CredentialConfirmViewBase : AuthViewBase
    {
        [SerializeField] private TMP_InputField _inputPassword;


        private Action<bool> _callback;



        public void Bind(Action<bool> callback)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
            RebindIfVisible();
        }


        protected override sealed bool IsInputValid()
        {
            return AuthInputValidator.IsValidPassword(_inputPassword.text);
        }

        protected override sealed void PerformAction(Action<AuthOperationResult> callback)
        {
            Confirm(_inputPassword.text, result =>
            {
                if (result.IsSuccess)
                {
                    _callback?.Invoke(true);
                    _callback = null;
                }

                callback(result);
            });
        }

        protected abstract void Confirm(string password, Action<AuthOperationResult> result);



        protected override void OnHidden()
        {
            _callback?.Invoke(false);
            _callback = null;
        }
    }
}
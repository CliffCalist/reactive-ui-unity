using System;
using TMPro;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Auth
{
    public abstract class CredentialConfirmViewBase : AuthFormViewBase
    {
        [SerializeField] private TMP_InputField _inputPassword;


        private Action<bool> _callback;



        public void Bind(Action<bool> callback)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
            RebindIfShowedInHierarchy();
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



        protected override void OnHided()
        {
            base.OnHided();
            _callback?.Invoke(false);
            _callback = null;
        }
    }
}
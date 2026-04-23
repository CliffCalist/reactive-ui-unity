using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public abstract class AuthViewBase : UIView
    {
        [SerializeField] private Button _btnConfirm;

        [Header("Error Display")]
        [SerializeField] private GameObject _errorObject;
        [SerializeField] private TextMeshProUGUI _txtError;



        private bool _isPending;



        protected override sealed void CreateBindings(CompositeDisposable bindings)
        {
            _errorObject?.SetActive(false);

            _btnConfirm.OnClickAsObservable()
                .Where(_ =>
                {
                    if (!_isPending)
                        return true;

                    Debug.LogWarning($"{GetType().Name}: action is already in progress.");
                    return false;
                })
                .Subscribe(_ => OnConfirmPressed())
                .AddTo(bindings);
        }



        private void OnConfirmPressed()
        {
            var validationResult = ValidateInput();
            if (!validationResult.IsValid)
            {
                ShowError(GetValidationMessage(validationResult));
                return;
            }

            _errorObject.SetActive(false);
            _isPending = true;

            PerformAction(result =>
            {
                _isPending = false;

                if (result.IsSuccess)
                    Hide();
                else
                    ShowError(result.DisplayErrorMessage);
            });
        }

        protected abstract AuthValidationResult ValidateInput();

        protected abstract void PerformAction(Action<AuthOperationResult> callback);



        protected virtual string GetValidationMessage(AuthValidationResult result)
        {
            return AuthInputValidator.GetErrorMessage(result);
        }



        private void ShowError(string message)
        {
            if (_errorObject != null)
                _errorObject.SetActive(true);

            if (_txtError != null)
                _txtError.SetText(message ?? "Unknown error");
        }
    }
}

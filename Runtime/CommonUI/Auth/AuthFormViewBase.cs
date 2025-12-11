using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteArrow.ReactiveUI.Auth
{
    public abstract class AuthFormViewBase : UIView
    {
        [SerializeField] private Button _btnConfirm;

        [Header("Error Display")]
        [SerializeField] private GameObject _errorObject;
        [SerializeField] private TextMeshProUGUI _txtError;



        private IDisposable _disposable;
        private bool _isPending;



        protected override sealed void CreateSubscriptions()
        {
            _errorObject?.SetActive(false);

            _disposable = _btnConfirm.OnClickAsObservable()
                .Where(_ =>
                {
                    if (!_isPending)
                        return true;

                    Debug.LogWarning($"{GetType().Name}: action is already in progress.");
                    return false;
                })
                .Subscribe(_ => OnConfirmPressed());
        }

        protected override sealed void DisposeSubscriptions()
        {
            _disposable?.Dispose();
        }



        private void OnConfirmPressed()
        {
            if (!IsInputValid())
            {
                ShowError("Input is invalid");
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

        protected abstract bool IsInputValid();

        protected abstract void PerformAction(Action<AuthOperationResult> callback);



        private void ShowError(string message)
        {
            if (_errorObject != null)
                _errorObject.SetActive(true);

            if (_txtError != null)
                _txtError.SetText(message ?? "Unknown error");
        }
    }
}
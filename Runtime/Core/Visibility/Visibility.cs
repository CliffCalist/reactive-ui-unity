using System;
using R3;
using UnityEngine;

namespace WhiteArrow.ReactiveUI.Core
{
    internal class Visibility : IVisibilityContext
    {
        public readonly GameObject ControlledObject;



        private readonly ReactiveProperty<bool> _isSelfShowed = new();
        private readonly ReactiveProperty<bool> _isShowedInHierarchy = new();



        public ReadOnlyReactiveProperty<bool> IsSelfShowed => _isSelfShowed;
        public ReadOnlyReactiveProperty<bool> IsShowedInHierarchy => _isShowedInHierarchy;



        public Visibility(GameObject controlledObject)
        {
            ControlledObject = controlledObject ?? throw new ArgumentNullException(nameof(controlledObject));
            Refresh();
        }



        public void SetActive(bool value)
        {
            ThrowIfObjectNull();
            ControlledObject.SetActive(value);
            Refresh();
        }

        public void OnEnable()
        {
            ThrowIfObjectNull();
            Refresh();
        }

        public void OnDisable()
        {
            ThrowIfObjectNull();
            Refresh();
        }

        public void Refresh()
        {
            ThrowIfObjectNull();

            if (_isSelfShowed.Value != ControlledObject.activeSelf)
                _isSelfShowed.Value = ControlledObject.activeSelf;

            if (_isShowedInHierarchy.Value != ControlledObject.activeInHierarchy)
                _isShowedInHierarchy.Value = ControlledObject.activeInHierarchy;
        }



        private void ThrowIfObjectNull()
        {
            if (ControlledObject == null)
                throw new NullReferenceException($"{nameof(ControlledObject)} is null. Maybe he was destroyed.");
        }
    }
}
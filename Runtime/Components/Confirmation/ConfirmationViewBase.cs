using R3;
using UnityEngine;
using UnityEngine.UI;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrow.ReactiveUI.Components
{
    public abstract class ConfirmationViewBase : UIView
    {
        [SerializeField] private Button _btnConfirm;


        private bool _isConfirmed;



        protected override void Init()
        {
            _btnConfirm.OnClickAsObservable()
                .Subscribe(_ => Confirm())
                .AddTo(this);
        }



        protected override void CreateBindings(CompositeDisposable bindings)
        {
            _isConfirmed = false;
        }



        public void Confirm()
        {
            if (!Visibility.IsSelfShowed.CurrentValue)
                return;

            _isConfirmed = true;
            Hide();
        }

        protected override void OnHidden()
        {
            OnChoiceMade(_isConfirmed);
        }



        protected abstract void OnChoiceMade(bool isConfirmed);
    }
}

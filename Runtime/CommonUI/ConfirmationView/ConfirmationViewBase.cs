using R3;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteArrow.ReactiveUI
{
    public abstract class ConfirmationViewBase : UIView
    {
        [SerializeField] private Button _btnConfirm;


        private bool _isConfirmed;



        protected override void InitCore()
        {
            _btnConfirm.OnClickAsObservable()
                .Subscribe(_ => Confirm())
                .AddTo(this);
        }



        protected override void BindFromCache()
        {
            _isConfirmed = false;
        }



        public void Confirm()
        {
            if (!IsSelfShowed.CurrentValue)
                return;

            _isConfirmed = true;
            Hide();
        }

        protected override void OnHided()
        {
            OnChoiceMade(_isConfirmed);
        }



        protected abstract void OnChoiceMade(bool isConfirmed);
    }
}

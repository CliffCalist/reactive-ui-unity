using R3;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteArrow.ReactiveUI
{
    public class ConfirmationPopUp : UIView
    {
        [SerializeField] private Button _btnConfirm;


        private bool _isConfirmed;

        private readonly Subject<bool> _onChoiceMade = new();
        public Observable<bool> OnChoiceMade => _onChoiceMade;



        protected override void InitCore()
        {
            _btnConfirm.OnClickAsObservable()
                .Subscribe(_ => Confirm())
                .AddTo(this);
        }



        protected override void OnShowed()
        {
            _isConfirmed = false;
        }



        public void Confirm()
        {
            if (!IsSelfShowed.CurrentValue)
                return;

            _isConfirmed = true;
            OnConfirmed();
        }

        protected virtual void OnConfirmed()
        {
            Hide();
        }


        protected override void OnHided()
        {
            _onChoiceMade.OnNext(_isConfirmed);
        }
    }
}

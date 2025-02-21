using R3;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteArrow.MVVM.UI
{
    public class ConfirmationPopUp : UiView
    {
        [SerializeField] private Button _btnConfirm;


        private bool _isConfirmed;

        private readonly Subject<bool> _onChoiceMade;
        public Observable<bool> OnChoiceMade => _onChoiceMade;



        protected override void OnInit()
        {
            _btnConfirm.OnClickAsObservable()
                .Subscribe(_ => Confirm())
                .AddTo(this);
        }



        protected override void OnEnabled()
        {
            _isConfirmed = false;
        }



        public void Confirm()
        {
            if (!IsShowed.CurrentValue)
                return;

            _isConfirmed = true;
            Hide();
        }

        protected override void OnDisabled()
        {
            _onChoiceMade.OnNext(_isConfirmed);
        }



        protected override void OnRebind() { }
        protected override void DisposeBinding() { }
    }
}

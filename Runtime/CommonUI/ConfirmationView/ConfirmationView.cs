using System;

namespace WhiteArrow.ReactiveUI
{
    public class ConfirmationView : ConfirmationViewBase
    {
        private Action<bool> _onChoiceMade;



        public void Bind(Action<bool> onChoiceMade)
        {
            _onChoiceMade = onChoiceMade ?? throw new ArgumentNullException(nameof(onChoiceMade));
            RebindIfShowedInHierarchy();
        }


        protected override void OnChoiceMade(bool isConfirmed)
        {
            _onChoiceMade(isConfirmed);
        }
    }
}
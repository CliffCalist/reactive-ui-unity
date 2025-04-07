using R3;
using UnityEngine;
using UnityEngine.UI;

namespace WhiteArrow.MVVM.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ViewButton : UIView
    {
        private Button _btn;


        protected override void InitCore()
        {
            var btn = GetComponent<Button>();

            btn.onClick.AsObservable()
                .Subscribe(_ => OnClicked())
                .AddTo(this);
        }


        protected abstract void OnClicked();
    }
}

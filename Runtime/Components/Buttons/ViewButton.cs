using R3;
using UnityEngine;
using UnityEngine.UI;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrow.ReactiveUI.Components
{
    [RequireComponent(typeof(Button))]
    public abstract class ViewButton : UIView
    {
        protected override void Init()
        {
            var btn = GetComponent<Button>();

            btn.onClick.AsObservable()
                .Subscribe(_ => OnClicked())
                .AddTo(this);
        }


        protected abstract void OnClicked();
    }
}

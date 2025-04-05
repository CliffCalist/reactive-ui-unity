using UnityEngine;
using UnityEngine.UI;

namespace WhiteArrow.MVVM.UI
{
    [RequireComponent(typeof(Button))]
    public class ShowHideViewButton : MonoBehaviour
    {
        private enum Mode { Show, Hide }



        [SerializeField] private Mode _mode;
        [SerializeField] private UIView _view;

        private Button _btn;



        private void Awake()
        {
            _btn = GetComponent<Button>();

            _btn.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _btn.onClick.RemoveListener(OnClick);
        }


        private void OnClick()
        {
            switch (_mode)
            {
                case Mode.Show:
                    _view.Show();
                    break;
                case Mode.Hide:
                    _view.Hide();
                    break;
            }
        }
    }
}

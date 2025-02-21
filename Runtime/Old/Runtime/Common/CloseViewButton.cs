// using UnityEngine;
// using UnityEngine.UI;

// namespace WhiteArrow.UI.MVVM
// {
//     [RequireComponent(typeof(Button))]
//     public class CloseViewButton : MonoBehaviour
//     {
//         [SerializeField] private InterfaceReference<IView, MonoBehaviour> _target;

//         private Button _btn;


//         private void Awake()
//         {
//             _btn = GetComponent<Button>();

//             _btn.onClick.AddListener(OnPressed);
//         }

//         private void OnDestroy()
//         {
//             _btn.onClick.RemoveListener(OnPressed);
//         }


//         private void OnPressed()
//         {
//             _target.Value?.Close(true);
//         }
//     }
// }

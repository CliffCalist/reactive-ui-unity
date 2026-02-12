using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrowEditor.ReactiveUI.Core
{
    [CustomEditor(typeof(VisibilityAnimations), true)]
    public class VisibilityAnimationsEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            var animations = (VisibilityAnimations)target;

            var playShowButton = new Button(PlayShow)
            {
                text = "Play Show"
            };

            var playHideButton = new Button(PlayHide)
            {
                text = "Play Hide"
            };



            UpdateButtonStates();

            EditorApplication.playModeStateChanged += (state) =>
            {
                UpdateButtonStates();
            };

            root.Add(playShowButton);
            root.Add(playHideButton);



            void UpdateButtonStates()
            {
                var isEnabled = EditorApplication.isPlaying;
                playShowButton.SetEnabled(isEnabled);
                playHideButton.SetEnabled(isEnabled);
            }

            void PlayShow()
            {
                if (animations.IsViewAttached)
                    animations.PlayShowAnimationInternal();
                else
                    animations.LogIfViewNotAttached();
            }

            void PlayHide()
            {
                if (animations.IsViewAttached)
                    animations.PlayHideAnimationInternal();
                else
                    animations.LogIfViewNotAttached();
            }

            return root;
        }
    }
}
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using WhiteArrow.ReactiveUI;

namespace WhiteArrowEditor.ReactiveUI
{
    [CustomEditor(typeof(MonoViewAnimations), true)]
    public class MonoViewAnimationsEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            var monoAnimations = (MonoViewAnimations)target;
            var animations = monoAnimations as IViewAnimations;



            var playShowButton = new Button(PlayShow)
            {
                text = "Play Show Animation"
            };

            var playHideButton = new Button(PlayHide)
            {
                text = "Play Hide Animation"
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
                if (animations.IsInitialized)
                    animations.PlayShow();
                else
                    Debug.LogWarning($"The {animations.GetType().Name} isn't initialized.");
            }

            void PlayHide()
            {
                if (animations.IsInitialized)
                    animations.PlayHide();
                else
                    Debug.LogWarning($"The {animations.GetType().Name} isn't initialized.");
            }



            return root;
        }
    }
}
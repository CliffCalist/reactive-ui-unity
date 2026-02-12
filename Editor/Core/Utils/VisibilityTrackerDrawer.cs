using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrowEditor.ReactiveUI.Core
{
    [CustomPropertyDrawer(typeof(VisibilityTracker))]
    public class VisibilityTrackerDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new PropertyField(property.FindPropertyRelative("_views"), property.displayName);
        }
    }
}
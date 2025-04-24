using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using WhiteArrow.ReactiveUI;

namespace WhiteArrowEditor.ReactiveUI
{
    [CustomPropertyDrawer(typeof(ViewVisibilityTracker))]
    public class ViewVisibilityTrackerDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new PropertyField(property.FindPropertyRelative("_views"), property.displayName);
        }
    }
}
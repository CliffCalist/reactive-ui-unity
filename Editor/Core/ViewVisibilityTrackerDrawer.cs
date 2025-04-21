using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using WhiteArrow.MVVM.UI;

namespace WhiteArrowEditor.MVVM.UI
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
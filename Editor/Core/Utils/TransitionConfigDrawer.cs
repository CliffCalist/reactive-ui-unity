using UnityEditor;
using UnityEngine;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrowEditor.ReactiveUI.Core
{
    [CustomPropertyDrawer(typeof(TransitionConfig))]
    public class TransitionConfigDrawer : PropertyDrawer
    {
        private const float VERTICAL_SPACING = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var flowProp = property.FindPropertyRelative(nameof(TransitionConfig.Flow));
            var layerPolicyProp = property.FindPropertyRelative(nameof(TransitionConfig.LayerPolicy));
            var skipShowProp = property.FindPropertyRelative(nameof(TransitionConfig.SkipShowAnimation));
            var skipHideProp = property.FindPropertyRelative(nameof(TransitionConfig.SkipHideAnimation));

            var lineHeight = EditorGUIUtility.singleLineHeight;
            var headerRect = new Rect(position.x, position.y, position.width, lineHeight);

            var foldoutWidth = EditorGUIUtility.labelWidth;
            var foldoutRect = new Rect(headerRect.x, headerRect.y, foldoutWidth, lineHeight);
            var enumRect = new Rect(headerRect.x + foldoutWidth, headerRect.y, headerRect.width - foldoutWidth, lineHeight);

            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            EditorGUI.PropertyField(enumRect, flowProp, GUIContent.none);

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }
            EditorGUI.indentLevel++;

            position.y += lineHeight + VERTICAL_SPACING;

            var lineRect = new Rect(position.x, position.y, position.width, lineHeight);

            if ((TransitionFlow)flowProp.enumValueIndex == TransitionFlow.ShowThenHide)
            {
                EditorGUI.PropertyField(lineRect, layerPolicyProp);
                lineRect.y += lineHeight + VERTICAL_SPACING;
            }

            EditorGUI.PropertyField(lineRect, skipShowProp);
            lineRect.y += lineHeight + VERTICAL_SPACING;

            EditorGUI.PropertyField(lineRect, skipHideProp);

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;

            if (!property.isExpanded)
                return lineHeight;

            float totalLines = 2f;

            var flowProp = property.FindPropertyRelative(nameof(TransitionConfig.Flow));

            if ((TransitionFlow)flowProp.enumValueIndex == TransitionFlow.ShowThenHide)
                totalLines += 1f;

            return lineHeight + (totalLines * lineHeight) + (totalLines * VERTICAL_SPACING);
        }
    }
}
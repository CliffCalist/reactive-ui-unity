using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using WhiteArrow.ReactiveUI.Core;

namespace WhiteArrowEditor.ReactiveUI.Core
{
    [CustomEditor(typeof(UIView), true)]
    public class UIViewEditor : Editor
    {
        private readonly Color BOX_BORDER_COLOR = new Color(0.25f, 0.25f, 0.25f, 1f);
        private const float BOX_BORDER_WIDTH = 3;
        private const float BOX_BORDER_RADIUS = 3;
        private const float BOX_PADDING = 2;

        private const string BTN_HIDE_PROPERTY_PATH = "_btnHide";
        private const string DEFAULT_FOCUS_ON_SHOW_PROPERTY_PATH = "FocusOnShowed";
        private const string DEFAULT_FOCUS_ON_HIDE_PROPERTY_PATH = "FocusOnHidden";



        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            var foldout = new Foldout();
            foldout.text = "Base Settings";
            foldout.value = false;
            root.Add(foldout);

            var box = CreateBoxGUI();
            foldout.Add(box);

            var btnHideProperty = new PropertyField(serializedObject.FindProperty(BTN_HIDE_PROPERTY_PATH));
            box.Add(btnHideProperty);

            var defaultFocusOnShowProperty = new PropertyField(serializedObject.FindProperty(DEFAULT_FOCUS_ON_SHOW_PROPERTY_PATH));
            box.Add(defaultFocusOnShowProperty);

            var defaultFocusOnHideProperty = new PropertyField(serializedObject.FindProperty(DEFAULT_FOCUS_ON_HIDE_PROPERTY_PATH));
            box.Add(defaultFocusOnHideProperty);

            var additionalInspectorGUI = CreateAdditionalInspectorGUI();
            root.Add(additionalInspectorGUI);

            return root;
        }

        private Box CreateBoxGUI()
        {
            var box = new Box();

            box.style.borderTopWidth = BOX_BORDER_WIDTH;
            box.style.borderBottomWidth = BOX_BORDER_WIDTH;
            box.style.borderLeftWidth = BOX_BORDER_WIDTH;
            box.style.borderRightWidth = BOX_BORDER_WIDTH;

            box.style.borderTopColor = BOX_BORDER_COLOR;
            box.style.borderBottomColor = BOX_BORDER_COLOR;
            box.style.borderLeftColor = BOX_BORDER_COLOR;
            box.style.borderRightColor = BOX_BORDER_COLOR;

            box.style.borderTopLeftRadius = BOX_BORDER_RADIUS;
            box.style.borderTopRightRadius = BOX_BORDER_RADIUS;
            box.style.borderBottomLeftRadius = BOX_BORDER_RADIUS;
            box.style.borderBottomRightRadius = BOX_BORDER_RADIUS;

            box.style.paddingRight = 6;
            box.style.paddingLeft = BOX_PADDING;
            box.style.paddingBottom = BOX_PADDING;
            box.style.paddingTop = BOX_PADDING;

            return box;
        }

        protected virtual VisualElement CreateAdditionalInspectorGUI()
        {
            var container = new VisualElement();

            serializedObject.Update();
            var iterator = serializedObject.GetIterator();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (iterator.propertyPath == "m_Script")
                    continue;

                if (iterator.propertyPath == BTN_HIDE_PROPERTY_PATH
                    || iterator.propertyPath == DEFAULT_FOCUS_ON_SHOW_PROPERTY_PATH
                    || iterator.propertyPath == DEFAULT_FOCUS_ON_HIDE_PROPERTY_PATH)
                    continue;

                var field = new PropertyField(iterator.Copy());
                container.Add(field);
            }

            container.Bind(serializedObject);
            return container.childCount > 0 ? container : null;
        }
    }
}
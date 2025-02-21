// using System;
// using UnityEditor;
// using UnityEngine;
// using WhiteArrow.UI.MVVM;

// namespace WhiteArrowEditor.MVVM
// {
//     [CustomEditor(typeof(View<>), true)]
//     public class ViewEditor : Editor
//     {
//         private SerializedProperty _childeViewsProperty;

//         private void OnEnable()
//         {
//             _childeViewsProperty = serializedObject.FindProperty("_childeViews");
//         }

//         public override void OnInspectorGUI()
//         {
//             serializedObject.Update();

//             if (GUILayout.Button("Collect childe views"))
//                 CollectChildeViews();

//             DrawDefaultInspector();
//             serializedObject.ApplyModifiedProperties();
//         }


//         private void CollectChildeViews()
//         {
//             _childeViewsProperty.ClearArray();

//             FindViewsInChildren(((MonoBehaviour)target).transform,
//                 view =>
//                 {
//                     var index = _childeViewsProperty.arraySize;
//                     _childeViewsProperty.InsertArrayElementAtIndex(index);

//                     var element = _childeViewsProperty.GetArrayElementAtIndex(index);
//                     element.FindPropertyRelative("_underlyingValue").objectReferenceValue = view as MonoBehaviour;
//                 });

//             serializedObject.ApplyModifiedProperties();
//             EditorUtility.SetDirty(target);
//         }

//         private void FindViewsInChildren(Transform parent, Action<IView> onFinded)
//         {
//             foreach (Transform child in parent)
//             {
//                 var view = child.GetComponent<IView>();
//                 if (view != null)
//                     onFinded(view);
//                 else
//                     FindViewsInChildren(child, onFinded);
//             }
//         }
//     }
// }
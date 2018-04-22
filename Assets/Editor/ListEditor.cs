using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(PointsOfInterest))]
public class ListEditor : Editor {

    private ReorderableList _listEssential;
    private ReorderableList _listNonEssential;

    private void OnEnable() {

        _listEssential = new ReorderableList(serializedObject, serializedObject.FindProperty("PoiEssential"), true, true, false, false);   
        _listNonEssential = new ReorderableList(serializedObject, serializedObject.FindProperty("PoiNonEssential"), true, true, false, false);
        
        CreateDrawElementCallback(_listEssential);
        CreateDrawElementCallback(_listNonEssential);
        
        _listEssential.drawHeaderCallback = (Rect rect) => {  
            EditorGUI.LabelField(rect, "Essential");
        };
        
        _listNonEssential.drawHeaderCallback = (Rect rect) => {  
            EditorGUI.LabelField(rect, "Non-Essential");
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Space();
        _listEssential.DoLayoutList();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        _listNonEssential.DoLayoutList();
        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties(); 
    }

    private void CreateDrawElementCallback(ReorderableList list)
    {

        list.drawElementCallback =  
            (rect, index, isActive, isFocused) => {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                /*EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, 70, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("Type"), GUIContent.none);*/
                GUI.enabled = false;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element, GUIContent.none);
                GUI.enabled = true;
            };
        
        
    }
}

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneProperty))]
public class ScenePropertyDrawer : PropertyDrawer
{
    override public void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUI.ObjectField(position, label, oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            property.stringValue = newPath;
        }
    }
}
using UnityEditor;

[CustomEditor(typeof(SceneCollider), true)]
public class SceneColliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var picker = target as SceneCollider;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.scene);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("scene");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();

        // DrawDefaultInspector();
    }
}
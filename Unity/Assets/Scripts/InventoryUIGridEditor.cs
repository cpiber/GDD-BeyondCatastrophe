using UnityEditor;

[CustomEditor(typeof(InventoryUIGrid))]
public class InventoryUIGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InventoryUIGrid aglg = (this.target as InventoryUIGrid);
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_Padding"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_Spacing"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_StartCorner"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_StartAxis"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_ChildAlignment"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_ConstraintCount"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("aspectRatio"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_CellSize"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("others"));
        serializedObject.ApplyModifiedProperties();
    }
 }
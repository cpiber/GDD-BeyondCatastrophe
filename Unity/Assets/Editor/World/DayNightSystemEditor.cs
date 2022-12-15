using UnityEditor;

[CustomEditor(typeof(DayNightSystem), true)]
public class DayNightSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var sys = target as DayNightSystem;
        DrawDefaultInspector();
        EditorGUILayout.LabelField("Day", sys.Day.ToString());
        EditorGUILayout.LabelField("Time in Day", sys.TimeInDay.ToString());
        EditorGUILayout.LabelField("Brightness", sys.Brightness.ToString());
        EditorGUILayout.LabelField("Day Section", sys.SectionInDay.ToString());
    }
}
using UnityEditor;

[CustomEditor(typeof(SeasonSystem), true)]
public class SeasonSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var sys = target as SeasonSystem;
        DrawDefaultInspector();
        EditorGUILayout.LabelField("Season", sys.CalculatedSeason.ToString());
        EditorGUILayout.LabelField("Day in Season", sys.DayInYear.ToString());
    }
}
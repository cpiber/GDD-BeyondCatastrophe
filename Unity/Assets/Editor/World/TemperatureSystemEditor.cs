using UnityEditor;

[CustomEditor(typeof(TemperatureSystem), true)]
public class TemperatureSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var sys = target as TemperatureSystem;
        DrawDefaultInspector();
        EditorGUILayout.LabelField("Outside Temperature", sys.OutsideTemperature.ToString());
        EditorGUILayout.LabelField("Temperature", sys.Temperature.ToString());
    }
}
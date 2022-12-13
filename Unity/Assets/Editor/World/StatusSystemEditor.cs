using UnityEditor;

[CustomEditor(typeof(StatusSystem), true)]
public class StatusSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var sys = target as StatusSystem;
        DrawDefaultInspector();
        EditorGUILayout.LabelField("Effective Body Temperature", sys.EffectiveBodyTemperature.ToString());
        EditorGUILayout.LabelField("Temperature Buffs", sys.TemperatureBuffs.ToString());
    }
}
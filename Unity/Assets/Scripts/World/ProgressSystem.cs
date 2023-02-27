using UnityEngine;
using UnityEngine.Events;

public class ProgressSystem : GenericSingleton<ProgressSystem>
{
    [SerializeField] SerializableDictionary<string, bool> progressValues;
    public UnityEvent progressChanged;

    public void setProgress(string name, bool value = true) {
        Debug.Log($"PROGRESS: {name} = {value}");
        progressValues[name] = value;
        progressChanged.Invoke();
    }

    public bool getProgress(string name, bool def = false) {
        return progressValues.ContainsKey(name) ? progressValues[name] : def;
    }
}

using UnityEngine;

public class GlobalSceneState : GenericSingleton<GlobalSceneState>
{
    public class State : ScriptableObject {
        public bool exists = true;
        public Vector3 position = Vector3.zero;
    }

    private SerializableDictionary<string, State> objectdata = new SerializableDictionary<string, State>();

    public State getState(string key) {
        if (!objectdata.ContainsKey(key)) return null;
        return objectdata[key];
    }
    public T getState<T>(string key) where T : State {
        if (!objectdata.ContainsKey(key)) return null;
        return objectdata[key] as T;
    }

    public void setState<T>(string key, T state) where T : State {
        objectdata[key] = state;
    }

    public void setPosition(string key, Vector3 pos) {
        if (!objectdata.ContainsKey(key)) objectdata[key] = ScriptableObject.CreateInstance<State>();
        objectdata[key].position = pos;
    }

    public void setExists(string key, bool exists) {
        if (!objectdata.ContainsKey(key)) objectdata[key] = ScriptableObject.CreateInstance<State>();
        objectdata[key].exists = exists;
    }
}

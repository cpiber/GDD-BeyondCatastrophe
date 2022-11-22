using System;
using UnityEngine;

public class GlobalSceneState : MonoBehaviour
{
    [Serializable]
    public struct State {
        public bool exists;
        public Vector3 position;

        public State(bool exists, Vector3 pos) {
            this.exists = exists;
            this.position = pos;
        }
    }

    static GlobalSceneState instance = null;

    private SerializableDictionary<string, State> objectdata = new SerializableDictionary<string, State>();

    public static GlobalSceneState the() {
        if (instance == null) instance = FindObjectOfType<GlobalSceneState>();
        return instance;
    }

    public State getState(string key, Vector3 defaultPosition) {
        if (!objectdata.ContainsKey(key)) return objectdata[key] = new State(true, defaultPosition);
        return objectdata[key];
    }

    public void setState(string key, State state) {
        objectdata[key] = state;
    }

    public void setPosition(string key, Vector3 pos) {
        if (!objectdata.ContainsKey(key)) objectdata[key] = new State(true, pos);
        var state = objectdata[key];
        state.position = pos;
        objectdata[key] = state;
    }

    public void setExists(string key, bool exists) {
        if (!objectdata.ContainsKey(key)) objectdata[key] = new State(true, Vector3.zero);
        var state = objectdata[key];
        state.exists = exists;
        objectdata[key] = state;
    }
}

using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour
    where T : GenericSingleton<T>
{
    private static T instance = null;

    public static T the() {
        if (instance == null) instance = FindObjectOfType<T>();
        return instance;
    }

    void Awake() {
        Debug.Assert(instance == null);
        instance = (T) this;
    }

    void Destroy() {
        instance = null;
    }
}

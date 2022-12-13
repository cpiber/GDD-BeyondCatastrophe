using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour
    where T : GenericSingleton<T>
{
    private static T instance = null;

    public static T the() {
        if (instance == null) instance = FindObjectOfType<T>();
        return instance;
    }

    public virtual void Awake() {
        Debug.Assert(instance == null || instance == this, this);
        instance = (T) this;
    }

    public virtual void Destroy() {
        instance = null;
    }
}

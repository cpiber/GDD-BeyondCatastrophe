using UnityEngine;

public class ProgressCollider : MonoBehaviour
{
    [SerializeField] string key = Items.Reusable.Axe.AXE_COLLECTED;

    void Start() {
        CheckProgress();
        ProgressSystem.the().progressChanged.AddListener(CheckProgress);
    }

    void OnDestroy() {
        ProgressSystem.the().progressChanged.RemoveListener(CheckProgress);
    }

    void CheckProgress() {
        var hasAxe = ProgressSystem.the().getProgress(key);
        if (!hasAxe) return;
        Destroy(gameObject);
    }
}

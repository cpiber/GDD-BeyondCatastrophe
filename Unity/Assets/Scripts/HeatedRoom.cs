using UnityEngine;

public class HeatedRoom : MonoBehaviour
{
    [SerializeField] Heater heater;
    private PlayerController playerInRange;
    public float Heating => heater.Heating;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.TryGetComponent<PlayerController>(out var possiblePlayer)) {
            Debug.Log($"Setting heated room to {this}");
            playerInRange = possiblePlayer;
            playerInRange.CurrentRoom = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (playerInRange != null && playerInRange.gameObject == collider.gameObject) {
            Debug.Log($"Clearing {this}");
            playerInRange.CurrentRoom = null;
            playerInRange = null;
        }
    }
}

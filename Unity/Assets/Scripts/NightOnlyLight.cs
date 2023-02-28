using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NightOnlyLight : MonoBehaviour
{
    [SerializeField] [HideInInspector] new Light2D light;

    void Start() {
        light = GetComponent<Light2D>();
    }
    
    void FixedUpdate() {
        light.enabled = DayNightSystem.the().SectionInDay == DayNightSystem.DaySection.Night;
    }
}

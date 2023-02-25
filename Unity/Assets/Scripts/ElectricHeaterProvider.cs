using UnityEngine;

public class ElectricHeaterProvider : GenericSingleton<ElectricHeaterProvider>
{
    [SerializeField] [Range(0,1)] float energy = .95f;
    [SerializeField] [Range(0,1)] float energyUsePerDay = .2f;
    [SerializeField] bool hasSolarPanel = false;
    
    public float Energy => energy;
    public bool HasSolarPanel => hasSolarPanel;

    void FixedUpdate() {
        if (hasSolarPanel) return;
        energy = Mathf.Max(0, energy - energyUsePerDay * Time.deltaTime / DayNightSystem.the().GetParams.secondsPerDay);
    }

    public void AddSolarPanel() {
        hasSolarPanel = true;
        energy = 1;
    }
}

using UnityEngine;

public class StatusSystem : GenericSingleton<StatusSystem>
{
    const int EVALUATION_TICKS = 10;

    [SerializeField] GameObject player;

    [Header("Settings/General")]
    [SerializeField] float idleTirednessSubPerDay = 40f;
    [SerializeField] int tirednessMin = 10;
    [SerializeField] int energyMin = 8;
    [Header("Settings/Body Temperature")]
    [SerializeField] float targetBodyTemperature = 25f;
    [SerializeField] float bodyTemperatureSpan = 5f;
    [SerializeField] float bodyTemperatureWideness = 0.5f;
    [SerializeField] float bodyTemperatureEnergyRequirements = 0.03f;
    [SerializeField] float bodyTemperatureCreep = 0.05f;

    [Header("Player Current Status")]
    [SerializeField] [Range(0, 100)] int health = 100;
    [SerializeField] [Range(0, 100)] float tiredness = 100;
    [SerializeField] [Range(0, 100)] float energy = 100;
    [SerializeField] float bodyTemperature;
    int frameCnt = 0;

    // TODO compute buffs from clothing
    public float TemperatureBuffs => 0;

    void Start() {
        health = 100;
        tiredness = 100;
        energy = 100;
        bodyTemperature = targetBodyTemperature;
    }

    void FixedUpdate() {
        frameCnt = (frameCnt + 1) % EVALUATION_TICKS;
        if (frameCnt != 0) return;
        UpdateStatusEffects();
    }

    void UpdateStatusEffects() {
        CalculateBodyTemperature();
        UpdateTiredness();
        UpdateHealth();
    }

    /// <summary>
    /// Based on <see href="https://www.geogebra.org/calculator/a8vycjys" />
    /// </summary>
    void CalculateBodyTemperature() {
        var evaluation = TemperatureSystem.the().Temperature + TemperatureBuffs - targetBodyTemperature;
        var ex = Mathf.Exp(bodyTemperatureWideness * evaluation);
        var target = bodyTemperatureSpan * (ex / (ex + 1) - 0.5f) + targetBodyTemperature;
        var change = target - bodyTemperature;
        var req = bodyTemperatureEnergyRequirements * evaluation * evaluation * Time.deltaTime;
        Debug.Log($"evaluation={evaluation}, bodyTemperature={bodyTemperature}, target={target}/{target-targetBodyTemperature}, change={change}, req={req}");
        bodyTemperature += Mathf.Min(Mathf.Sign(change) * bodyTemperatureCreep, change);
        energy = Mathf.Max(0, energy - req);
    }

    void UpdateTiredness() {
        tiredness = Mathf.Max(0, tiredness - idleTirednessSubPerDay * Time.deltaTime * EVALUATION_TICKS / DayNightSystem.the().GetParams.secondsPerDay);
    }

    void UpdateHealth() {
        if (tiredness < tirednessMin) health = Mathf.Max(0, health - tirednessMin + (int) tiredness);
        if (energy < energyMin) health = Mathf.Max(0, health - energyMin + (int) energy);
        if (health > 0) return;
        // TODO game over
        Destroy(player);
    }
}

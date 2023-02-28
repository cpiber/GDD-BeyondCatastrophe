using System;
using UnityEngine;
using UnityEngine.Events;

public class StatusSystem : GenericSingleton<StatusSystem>
{
    public const int EVALUATION_TICKS = 10;
    public const int STATUS_MAX = 100;
    [Serializable]
    public class StatusChangeEvent : UnityEvent<StatusSystem, bool> { }

    [SerializeField] PlayerController player;
    [SerializeField] public bool godMode = false;

    [Header("Settings/General")]
    [SerializeField] float idleTirednessSubPerDay = 40f;
    [SerializeField] int tirednessMin = 10;
    [SerializeField] int energyMin = 8;
    [SerializeField] float bodyTemperatureDangerMax = 5.5f;
    [SerializeField] float bodyTemperatureDangerMaxMult = .12f;
    [SerializeField] int overeatingDebuff = 3;
    [Header("Settings/Body Temperature")]
    [SerializeField] float targetBodyTemperature = 25f;
    [SerializeField] float bodyTemperatureSpan = 6f;
    [SerializeField] float bodyTemperatureScale = 0.5f;
    [SerializeField] float bodyTemperatureSteepness = 2f;
    [SerializeField] float bodyTemperatureEnergyRequirements = 0.005f;
    [SerializeField] float bodyTemperatureCreep = 0.05f;
    [Header("Settings/Recovery")]
    [SerializeField] [Range(0, STATUS_MAX)] float minTirednessForRecover = STATUS_MAX / 15;
    [SerializeField] [Range(0, STATUS_MAX)] float idleEnergyRecoverPerDay = 10f;
    [SerializeField] [Min(0)] float idleEnergyRecoverTime = 10f; // seconds
    [SerializeField] [Range(0, STATUS_MAX)] float sleepEnergyRecover = 20f;

    [Header("Player Current Status")]
    [SerializeField] [Range(0, STATUS_MAX)] int health = STATUS_MAX;
    [SerializeField] [Range(0, STATUS_MAX)] float tiredness = STATUS_MAX;
    [SerializeField] [Range(0, STATUS_MAX)] float energy = STATUS_MAX;
    [SerializeField] float bodyTemperature;
    int frameCnt = 0;
    public float TargetBodyTemperature => targetBodyTemperature;
    public float BodyTemperature => bodyTemperature;
    public float EffectiveBodyTemperature => bodyTemperature + TemperatureBuffs;
    public int Health => health;
    public float Tiredness => tiredness;
    public float Energy => energy;
    public StatusChangeEvent OnStatusChange;

    public float TemperatureBuffs {
        get {
            float buff = 0;
            foreach (var it in InventoryManager.the().GetArmorItems()) {
                Debug.Assert(it.IsArmor(), "Expected armor here");
                if (it is EmptyItem) continue;
                buff += ((ArmorItem) it).TemperatureBuff();
            }
            return buff;
        }
    }

    void Start() {
        health = STATUS_MAX;
        tiredness = STATUS_MAX;
        energy = STATUS_MAX;
        bodyTemperature = targetBodyTemperature;
    }

    void FixedUpdate() {
        frameCnt = (frameCnt + 1) % EVALUATION_TICKS;
        if (frameCnt != 0) return;
        UpdateStatusEffects();
    }

    [ContextMenu("Update Status Effects")]
    void UpdateStatusEffects() {
        CalculateBodyTemperature();
        UpdateTiredness();
        UpdateEnergy();
        UpdateHealth();
        SendUpdatedEffects();
    }

    /// <summary>
    /// Based on <see href="https://www.geogebra.org/calculator/a8vycjys" />
    /// </summary>
    [ContextMenu("Calculate Body Temperature")]
    void CalculateBodyTemperature() {
        var evaluation = TemperatureSystem.the().Temperature + TemperatureBuffs - targetBodyTemperature;
        var sx = bodyTemperatureScale * evaluation;
        var target = bodyTemperatureSpan * sx / (bodyTemperatureSteepness + Mathf.Abs(sx)) + targetBodyTemperature;
        var change = (target - bodyTemperature) * Time.deltaTime * EVALUATION_TICKS;
        if (Mathf.Abs(change) < 1e-6) change = 0;
        var req = bodyTemperatureEnergyRequirements * evaluation * evaluation * Time.deltaTime * EVALUATION_TICKS;
        bodyTemperature += Mathf.Min(Mathf.Sign(change) * bodyTemperatureCreep, change);
        energy = Mathf.Max(0, energy - req);
    }

    [ContextMenu("Update Tiredness")]
    void UpdateTiredness() {
        tiredness = Mathf.Max(0, tiredness - idleTirednessSubPerDay * Time.deltaTime * EVALUATION_TICKS / DayNightSystem.the().GetParams.secondsPerDay);
    }

    [ContextMenu("Update Energy")]
    void UpdateEnergy() {
        if (player.TimeSinceIdle <= idleEnergyRecoverTime) return;
        energy = Mathf.Min(STATUS_MAX, energy + idleEnergyRecoverPerDay * Time.deltaTime * EVALUATION_TICKS / DayNightSystem.the().GetParams.secondsPerDay);
    }

    [ContextMenu("Update Health")]
    void UpdateHealth() {
        if (tiredness < tirednessMin) health = Mathf.Max(0, health - tirednessMin + (int) tiredness);
        if (energy < energyMin) health = Mathf.Max(0, health - energyMin + (int) energy);
        var tempDiff = Mathf.Abs(bodyTemperature - targetBodyTemperature);
        if (tempDiff > bodyTemperatureDangerMax) health = Mathf.Max(0, health - (int) (tempDiff * bodyTemperatureDangerMaxMult));
    }

    void SendUpdatedEffects() {
        bool dead = (health <= 0 || player == null) && !godMode;
        OnStatusChange.Invoke(this, dead);
    }

    public void Sleep() {
        if (tiredness > minTirednessForRecover) tiredness = STATUS_MAX;
        energy = Mathf.Min(STATUS_MAX, energy + sleepEnergyRecover);
        SendUpdatedEffects();
    }

    public void Eat(float foodEnergy) {
        energy += foodEnergy;
        if (energy > STATUS_MAX) {
            // This will be either 0 or 1, depending on how well-fed the player was before
            health += Mathf.RoundToInt((energy - STATUS_MAX) / foodEnergy);
            if (health > STATUS_MAX * 0.9) health -= overeatingDebuff;
            energy = STATUS_MAX;
        }
        SendUpdatedEffects();
    }

    public void ToggleGodMode() {
        godMode = !godMode;
    }

    public void updateHealth(int adjustment)
    {
        health = health + adjustment;
    }
}

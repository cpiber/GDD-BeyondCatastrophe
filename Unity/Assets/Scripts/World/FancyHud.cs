using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FancyHud : MonoBehaviour
{
    public const float DANGER_INVPERC = 3f;

    [Header("Seasons")]
    [SerializeField] Image seasonIndicator;
    [SerializeField] Sprite fallSprite;
    [SerializeField] Sprite winterSprite;
    [SerializeField] Sprite springSprite;
    [SerializeField] Sprite summerSprite;

    [Header("Status")]
    [SerializeField] Image healthIcon;
    [SerializeField] Image healthIconShadow;
    [SerializeField] Image healthBorder;
    [SerializeField] Image tirednessIcon;
    [SerializeField] Image tirednessIconShadow;
    [SerializeField] Image tirednessBorder;
    [SerializeField] Image energyIcon;
    [SerializeField] Image energyIconShadow;
    [SerializeField] Image energyBorder;

    [Header("Status/Temperature")]
    [SerializeField] Image tempIcon;
    [SerializeField] Image tempIconInner;
    [SerializeField] Image tempBorder;
    [SerializeField] [Range(0, 1)] float tempMaxSquish = .55f;
    [SerializeField] float tempGlobalMin = 17f;
    [SerializeField] float tempGlobalMax = 33f;
    [SerializeField] Gradient tempColorGradient;

    [Header("Status/Pulse")]
    [SerializeField] float pulseOffset = 0.5f;
    [SerializeField] AnimationCurve pulseCurve;
    [SerializeField] Color borderColor;
    [SerializeField] Color dangerBG = Color.red;

    [Header("DebugHud")]
    [SerializeField] GameObject objHud;
    [SerializeField] GameObject objGodMode;
#if UNITY_EDITOR
    [SerializeField] bool enableHud;
#endif

    void Start() {
        SeasonChange(SeasonSystem.the().CurrentSeason);
        StatusChange(StatusSystem.the(), false);

#if !UNITY_EDITOR
        Destroy(objHud);
        Destroy(objGodMode);
#endif
    }

    void FixedUpdate() {
        var pulseHealth = pulseCurve.Evaluate(Time.time);
        healthIcon.transform.localScale = new Vector3(pulseHealth, pulseHealth, 1);
        healthIconShadow.transform.localScale = new Vector3(pulseHealth, pulseHealth, 1);
        var pulseTiredness = pulseCurve.Evaluate(Time.time + pulseOffset);
        tirednessIcon.transform.localScale = new Vector3(pulseTiredness, pulseTiredness, 1);
        tirednessIconShadow.transform.localScale = new Vector3(pulseTiredness, pulseTiredness, 1);
        var pulseEnergy = pulseCurve.Evaluate(Time.time + pulseOffset * 2);
        energyIcon.transform.localScale = new Vector3(pulseEnergy, pulseEnergy, 1);
        energyIconShadow.transform.localScale = new Vector3(pulseEnergy, pulseEnergy, 1);
    }

    public void SeasonChange(SeasonSystem.Seasons newSeason) {
        seasonIndicator.sprite = newSeason switch {
            SeasonSystem.Seasons.Fall   => fallSprite,
            SeasonSystem.Seasons.Winter => winterSprite,
            SeasonSystem.Seasons.Spring => springSprite,
            SeasonSystem.Seasons.Summer => summerSprite,
            _ => throw new ArgumentOutOfRangeException(nameof(newSeason), $"Unexpected season value {newSeason}"),
        };
    }

    public void StatusChange(StatusSystem status, bool dead) {
        healthIcon.fillAmount = (float) status.Health / StatusSystem.STATUS_MAX;
        healthBorder.color = Color.Lerp(dangerBG, borderColor, Mathf.Min(healthIcon.fillAmount * DANGER_INVPERC, 1));
        tirednessIcon.fillAmount = 1f - (float) status.Tiredness / StatusSystem.STATUS_MAX;
        tirednessBorder.color = Color.Lerp(dangerBG, borderColor, Mathf.Min((1f - tirednessIcon.fillAmount) * DANGER_INVPERC, 1));
        energyIcon.fillAmount = (float) status.Energy / StatusSystem.STATUS_MAX;
        energyBorder.color = Color.Lerp(dangerBG, borderColor, Mathf.Min(energyIcon.fillAmount * DANGER_INVPERC, 1));

        var temp = status.BodyTemperature;
        var tempPerc = (temp - tempGlobalMin) / (tempGlobalMax - tempGlobalMin);
        var tempCol = tempColorGradient.Evaluate(tempPerc);
        var tempOff = (tempPerc) * (1 - tempMaxSquish) + tempMaxSquish;
        tempIcon.color = tempCol;
        tempIconInner.color = tempCol;
        tempIconInner.rectTransform.anchorMax = new Vector2(1, tempOff);

        if (dead) SceneManager.LoadScene("GameOver");
    }

#if UNITY_EDITOR
    void OnValidate() {
        foreach (var im in FindObjectsOfType<Image>()) {
            if (im.gameObject.name != "Border") continue;
            im.color = borderColor;
        }

        objHud.SetActive(enableHud);
        objGodMode.SetActive(enableHud);
    }
#endif
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class FancyHud : MonoBehaviour
{
    [Header("Seasons")]
    [SerializeField] Image seasonIndicator;
    [SerializeField] Sprite fallSprite;
    [SerializeField] Sprite winterSprite;
    [SerializeField] Sprite springSprite;
    [SerializeField] Sprite summerSprite; 

    void Start() {
        SeasonChange(SeasonSystem.the().CurrentSeason);
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
}
